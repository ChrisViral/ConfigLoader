using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using ConfigLoader;
using ConfigLoader.Attributes;
using ConfigLoaderGenerator.Extensions;
using ConfigLoaderGenerator.Metadata;
using ConfigLoaderGenerator.Utils;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static ConfigLoaderGenerator.Extensions.SyntaxLiteralExtensions;
using static ConfigLoaderGenerator.Extensions.SyntaxOperationExtensions;
using static ConfigLoaderGenerator.Extensions.SyntaxStatementExtensions;
using static ConfigLoaderGenerator.SourceGeneration.GenerationConstants;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoaderGenerator.SourceGeneration;

/// <summary>
/// <see cref="ConfigBuilder"/> source generation context
/// </summary>
/// <param name="UsedNamespaces">Set of used namespaces</param>
/// <param name="Token">Source generation cancellation token</param>
public readonly record struct ConfigBuilderContext(NamespaceSet UsedNamespaces, CancellationToken Token);

/// <summary>
/// ConfigNode Load/Save source builder
/// </summary>
public static class ConfigBuilder
{
    /// <summary>
    /// Value load implementation delegate
    /// </summary>
    /// <param name="value">Value to load</param>
    /// <param name="field">Field data</param>
    /// <param name="context">Generation context</param>
    /// <returns>A block containing the value load code</returns>
    private delegate BlockSyntax LoadSectionGenerator(ExpressionSyntax value, in ConfigFieldMetadata field, in ConfigBuilderContext context);

    #region Generation
    /// <summary>
    /// Generate the source file for the given template
    /// </summary>
    /// <param name="data">Generation data</param>
    /// <param name="token">The generator-provided cancellation token</param>
    /// <returns>A tuple containing the generated file name and full file source</returns>
    public static (string fileName, string source) GenerateSource(ConfigData data, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        // Compilation root
        CompilationUnitSyntax root = CompilationUnit();
        ConfigBuilderContext context = new([], token);
        context.UsedNamespaces.AddNamespaceName(nameof(ConfigLoader));

        // Reuse the old declaration and strip the things we don't need
        TypeDeclarationSyntax type = data.Syntax
                                         .WithAttributeLists([])    // Attributes
                                         .WithMembers([])           // Type contents
                                         .WithBaseList(null)        // Inherited types
                                         .WithParameterList(null)   // Primary constructors
                                         .WithConstraintClauses([]) // Type parameter constraints
                                         .WithBody();               // Ensures braces are present

        // Add IGeneratedConfigNode implementation
        type = (TypeDeclarationSyntax)type.AddBaseListTypes(nameof(IGeneratedConfigNode).AsBaseType());

        // Method parameter
        ParameterSyntax nodeParam = Node.AsParameter(ConfigNode);

        // Generate base save/load implementation
        type = GenerateImplementation(type, data, nodeParam, context);

        // Add IConfigNode implementation
        type = GenerateInterfaceImplementation(type, data.Attribute, nodeParam, context);

        // Add namespace if needed
        MemberDeclarationSyntax rootDeclaration = type;
        if (data.Type.ContainingNamespace is not null)
        {
            rootDeclaration = data.Type
                                  .FullNamespace()
                                  .AsNamespace()
                                  .AddMembers(type);
        }

        if (context.UsedNamespaces.Count > 0)
        {
            // Add usings
            UsingDirectiveSyntax[] usingDirectives = context.UsedNamespaces
                                                            .GetUsings()
                                                            .ToArray();

            // Add header comment
            usingDirectives[0] = usingDirectives[0].AddLeadingTrivia(GeneratedComment);
            root = root.AddUsings(usingDirectives);
        }
        else
        {
            // Add header comment
            rootDeclaration = rootDeclaration.AddLeadingTrivia(GeneratedComment);
        }

        // Add topmost member to root
        root = root.AddMembers(rootDeclaration);

        // This should get the EOL string from the user settings
        string lineFeed = CarriageReturnLineFeed.ToFullString();

        root = root.NormalizeWhitespace(eol: lineFeed);
        return ($"{data.Type.FullName()}.generated.cs", root.ToFullString() + lineFeed);
    }

    /// <summary>
    /// Generates the load/save implementation
    /// </summary>
    /// <param name="type">Type declaration</param>
    /// <param name="data">Generation data</param>
    /// <param name="nodeParam">The ConfigNode parameter</param>
    /// <param name="context">Generation context</param>
    /// <returns>The modified type declaration with the generated methods added</returns>
    public static TypeDeclarationSyntax GenerateImplementation(TypeDeclarationSyntax type, ConfigData data, ParameterSyntax nodeParam, in ConfigBuilderContext context)
    {
        // Generate methods
        ConfigObjectMetadata objectData = data.Attribute;
        MethodDeclarationSyntax loadMethod = objectData.LoadMethod.DeclareMethod(SyntaxKind.VoidKeyword, objectData.LoadAccessModifier, nodeParam);
        MethodDeclarationSyntax saveMethod = objectData.SaveMethod.DeclareMethod(SyntaxKind.VoidKeyword, objectData.SaveAccessModifier, nodeParam);

        // Generate load and save method code
        loadMethod = GenerateLoadMethodBody(loadMethod, data, context);
        saveMethod = GenerateSaveMethodBody(saveMethod, data, context);

        // Add documentation comments
        loadMethod = loadMethod.AddLeadingTrivia(LoadMethodDoc);
        saveMethod = saveMethod.AddLeadingTrivia(SaveMethodDoc);

        return type.AddMembers(loadMethod, saveMethod);
    }

    /// <summary>
    /// Generates the IConfigNode implementation
    /// </summary>
    /// <param name="type">Type declaration</param>
    /// <param name="data">Generation data</param>
    /// <param name="nodeParam">The ConfigNode parameter</param>
    /// <param name="context">Generation context</param>
    /// <returns>The modifier type declaration with the methods added</returns>
    /// <exception cref="InvalidEnumArgumentException">If the implementation type enum value is invalid</exception>
    public static TypeDeclarationSyntax GenerateInterfaceImplementation(TypeDeclarationSyntax type, in ConfigObjectMetadata data, ParameterSyntax nodeParam, in ConfigBuilderContext context)
    {
        context.Token.ThrowIfCancellationRequested();

        MethodDeclarationSyntax load, save;
        switch (data.Implementation)
        {
            // No need to do anything for these
            case InterfaceImplementation.None:
            case InterfaceImplementation.UseGenerated:
                return type;

            // Create methods as explicit implementations
            case InterfaceImplementation.Explicit:
                load = Load.DeclareExplicitInterfaceMethod(SyntaxKind.VoidKeyword, IConfigNode.AsExplicitInterface(), nodeParam);
                save = Save.DeclareExplicitInterfaceMethod(SyntaxKind.VoidKeyword, IConfigNode.AsExplicitInterface(), nodeParam);
                break;

            // Create methods publicly
            case InterfaceImplementation.Public:
                load = Load.DeclareMethod(SyntaxKind.VoidKeyword, SyntaxKind.PublicKeyword, nodeParam);
                save = Save.DeclareMethod(SyntaxKind.VoidKeyword, SyntaxKind.PublicKeyword, nodeParam);
                break;

            default:
                throw new InvalidEnumArgumentException(nameof(data.Implementation), (int)data.Implementation, typeof(InterfaceImplementation));
        }

        // Add generated calls to methods
        load = load.AddBodyStatements(data.LoadMethod.Invoke(Node.AsArgument()).AsStatement());
        save = save.AddBodyStatements(data.SaveMethod.Invoke(Node.AsArgument()).AsStatement());

        // Wrap in region
        load = load.AddRegionStart(InterfaceRegion);
        save = save.AddRegionEnd();

        // Add members and return
        return type.AddMembers(load, save);
    }

    /// <summary>
    /// Generates a guard statement that ensures the node value is not null
    /// </summary>
    /// <returns>The generated if statement</returns>
    public static IfStatementSyntax GenerateNodeGuard()
    {
        // if (node == null) return;
        return IfStatement(Node.IsNull(), Block(Return()));
    }
    #endregion

    #region Load
    /// <summary>
    /// Generates a load method for the given type
    /// </summary>
    /// <param name="method">Load method declaration</param>
    /// <param name="data">Generation data</param>
    /// <param name="context">Generation context</param>
    /// <returns>The edited load method declaration with the load code generated</returns>
    private static MethodDeclarationSyntax GenerateLoadMethodBody(MethodDeclarationSyntax method, ConfigData data, in ConfigBuilderContext context)
    {
        context.Token.ThrowIfCancellationRequested();

        // Make sure node is not null
        method = method.AddBodyStatements(GenerateNodeGuard());

        // Simple fields loop
        method = GenerateNodeLoop(method, ValueCount, CountValues, Values, ConfigNodeValue, Value.Access(Value), data.ValueFields, LoadBuilder.GenerateValueLoad, context);

        // Config fields loop
        method = GenerateNodeLoop(method, NodeCount, CountNodes, Nodes, ConfigNode, Value, data.NodeFields, LoadBuilder.GenerateNodeLoad, context);

        return method;
    }

    /// <summary>
    /// Generates a section of code which iterates over a set of values from the ConfigNode and switches over their name
    /// </summary>
    /// <param name="method">Load method declaration</param>
    /// <param name="countName">Count variable name</param>
    /// <param name="count">Count getter name</param>
    /// <param name="values">Values to access name</param>
    /// <param name="valueType">Type of values being accessed</param>
    /// <param name="value">Value to load expression</param>
    /// <param name="generateSection">Function which generates a load section for each field</param>
    /// <param name="fields">List of fields to generate load code for</param>
    /// <param name="context">Generation context</param>
    /// <returns>The edited load method declaration with the load code generated</returns>
    private static MethodDeclarationSyntax GenerateNodeLoop(MethodDeclarationSyntax method, IdentifierNameSyntax countName, IdentifierNameSyntax count, IdentifierNameSyntax values, TypeSyntax valueType,
                                                            ExpressionSyntax value, IReadOnlyCollection<ConfigFieldMetadata> fields, LoadSectionGenerator generateSection, in ConfigBuilderContext context)
    {
        context.Token.ThrowIfCancellationRequested();

        // If there are no fields to load, return early
        if (fields.Count == 0) return method;

        // int count = node.count;
        VariableDeclarationSyntax countVariable = countName.DeclareVariable(SyntaxKind.IntKeyword, Node.Access(count));
        // for (int i = 0; i < count; i++)
        ForStatementSyntax forStatement = IncrementingForLoop(Index, MakeLiteral(0), countName);

        // node.values[i]
        ExpressionSyntax currentValue = Node.Access(values).ElementAccess(Index.AsArgument());
        // Type value = node.values[i];
        VariableDeclarationSyntax valueDeclaration = Value.DeclareVariable(valueType, currentValue);

        // switch (value.name)
        SwitchStatementSyntax nameSwitchStatement = Value.Access(Name).AsSwitchStatement();
        // ReSharper disable once LoopCanBeConvertedToQuery
        foreach (ConfigFieldMetadata field in fields)
        {
            // case "name":
            SwitchLabelSyntax label = field.SerializedName.AsLiteral().AsSwitchLabel();

            // Value parsing implementation
            BlockSyntax body = generateSection(value, field, context);

            // Add break statement, then Create section with label and body
            body = body.AddStatements(Break());
            SwitchSectionSyntax section = SwitchSection(label.AsList(), body.AsList<StatementSyntax>());

            // Add sections for the field
            nameSwitchStatement = nameSwitchStatement.AddSections(section);
        }

        // Add statements to loop body
        BlockSyntax forBody = Block().AddStatements(valueDeclaration.AsLocalDeclaration(), nameSwitchStatement);
        forStatement = forStatement.WithStatement(forBody);

        // Add loop to method and return
        return method.AddBodyStatements(countVariable.AsLocalDeclaration(), forStatement);
    }
    #endregion

    #region Save
    /// <summary>
    /// Generates a save method for the given type
    /// </summary>
    /// <param name="method">Save method declaration</param>
    /// <param name="data">Generation data</param>
    /// <param name="context">Generation context</param>
    /// <returns>The edited save method declaration with the save code generated</returns>
    private static MethodDeclarationSyntax GenerateSaveMethodBody(MethodDeclarationSyntax method, ConfigData data, in ConfigBuilderContext context)
    {
        context.Token.ThrowIfCancellationRequested();

        // Make sure node is not null
        method = method.AddBodyStatements(GenerateNodeGuard());

        // ReSharper disable once LoopCanBeConvertedToQuery
        foreach (ConfigFieldMetadata field in data.ValueFields)
        {
            // Add save for every field
            method = GenerateFieldSave(method, field, context);
        }

        // ReSharper disable once LoopCanBeConvertedToQuery
        foreach (ConfigFieldMetadata field in data.NodeFields)
        {
            // Add save for every field
            method = GenerateFieldSave(method, field, context);
        }

        return method;
    }

    /// <summary>
    /// Generates the save code for the given field
    /// </summary>
    /// <param name="method">Save method declaration</param>
    /// <param name="field">Field to generate the save code for</param>
    /// <param name="context">Generation context</param>
    /// <returns>The edited save method declaration with the field save code generated</returns>
    private static MethodDeclarationSyntax GenerateFieldSave(MethodDeclarationSyntax method, in ConfigFieldMetadata field, in ConfigBuilderContext context)
    {
        context.Token.ThrowIfCancellationRequested();

        // Variables
        LiteralExpressionSyntax name = field.SerializedName.AsLiteral();
        ExpressionSyntax value = This().Access(field.FieldName);

        // Value saving implementation
        return SaveBuilder.GenerateFieldSave(method, name, value, field, context);
    }
    #endregion
}
