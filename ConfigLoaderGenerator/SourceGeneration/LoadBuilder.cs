using System;
using System.Collections.Generic;
using ConfigLoader.Attributes;
using ConfigLoader.Utils;
using ConfigLoaderGenerator.Metadata;
using ConfigLoaderGenerator.Extensions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static ConfigLoaderGenerator.Extensions.SyntaxLiteralExtensions;
using static ConfigLoaderGenerator.Extensions.SyntaxPrefixExpressionExtensions;
using static ConfigLoaderGenerator.Extensions.SyntaxStatementExtensions;
using static ConfigLoaderGenerator.SourceGeneration.GenerationConstants;
using TypeInfo = ConfigLoaderGenerator.Metadata.TypeInfo;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoaderGenerator.SourceGeneration;

/// <summary>
/// Field load code generation
/// </summary>
public static class LoadBuilder
{
    /// <summary>
    /// TryParse invocation generator delegate
    /// </summary>
    /// <param name="tryParse">TryParse member access</param>
    /// <param name="value">Value to parse</param>
    /// <param name="outVar">Output variable</param>
    /// <param name="options">Options argument</param>
    /// <param name="field">Field to parse into</param>
    /// <param name="context">Generation context</param>
    /// <returns>The created <c>TryParse</c> invocation</returns>
    private delegate InvocationExpressionSyntax TryParseInvocation(MemberAccessExpressionSyntax tryParse, ExpressionSyntax value, ArgumentSyntax outVar,
                                                                   ArgumentSyntax options, in ConfigFieldMetadata field, in ConfigBuilderContext context);

    /// <summary>
    /// <see cref="ConfigLoader"/> parse utils namespace
    /// </summary>
    private static readonly string UtilsNamespace = typeof(ParseUtils).Namespace!;
    /// <summary>
    /// TryParse method identifier
    /// </summary>
    private static readonly IdentifierNameSyntax TryParse = nameof(ConfigLoader.Utils.ParseUtils.TryParse).AsName();
    /// <summary>
    /// <see cref="ParseUtils"/> class identifier
    /// </summary>
    private static readonly IdentifierNameSyntax ParseUtils = nameof(ConfigLoader.Utils.ParseUtils).AsName();
    /// <summary>
    /// <see cref="ParseOptions"/> struct identifier
    /// </summary>
    private static readonly IdentifierNameSyntax ParseOptions = nameof(ConfigLoader.Utils.ParseOptions).AsName();
    /// <summary>
    /// <see cref="ParseOptions"/> defaults identifier
    /// </summary>
    private static readonly IdentifierNameSyntax Defaults = nameof(ConfigLoader.Utils.ParseOptions.Defaults).AsName();
    /// <summary>
    /// Default <see cref="ParseOptions"/>
    /// </summary>
    private static readonly ExpressionSyntax DefaultOptions = ParseOptions.Access(Defaults);
    /// <summary>
    /// Types that can be directly assigned to the field
    /// </summary>
    private static readonly HashSet<string> AssignableTypes =
    [
        typeof(string).FullName,
        typeof(object).FullName
    ];

    #region Options
    /// <summary>
    /// Generates parse options for a specific field
    /// </summary>
    /// <param name="field">Field to create the parse options for</param>
    /// <returns>The created parse options, or the default options if none were required</returns>
    private static ExpressionSyntax GenerateParseOptions(in ConfigFieldMetadata field)
    {
        List<ArgumentSyntax> options = new(5);

        if (field.EnumHandling is not ConfigFieldAttribute.DefaultEnumHandling)
        {
            ExpressionSyntax value  = nameof(EnumHandling).Access(EnumUtils.ToString(field.EnumHandling));
            ArgumentSyntax argument = value.AsArgument(nameof(ConfigLoader.Utils.ParseOptions.EnumHandling));
            options.Add(argument);
        }
        if (field.SplitOptions is not ConfigFieldAttribute.DefaultSplitOptions)
        {
            ExpressionSyntax value  = nameof(ExtendedSplitOptions).Access(EnumUtils.ToString(field.SplitOptions));
            ArgumentSyntax argument = value.AsArgument(nameof(ConfigLoader.Utils.ParseOptions.SplitOptions));
            options.Add(argument);
        }
        if (field.ValueSeparator != default)
        {
            ExpressionSyntax value  = MakeLiteral(field.ValueSeparator);
            ArgumentSyntax argument = value.AsArgument(nameof(ConfigLoader.Utils.ParseOptions.ValueSeparator));
            options.Add(argument);
        }
        if (field.CollectionSeparator != default)
        {
            ExpressionSyntax value  = MakeLiteral(field.CollectionSeparator);
            ArgumentSyntax argument = value.AsArgument(nameof(ConfigLoader.Utils.ParseOptions.CollectionSeparator));
            options.Add(argument);
        }
        // ReSharper disable once InvertIf
        if (field.KeyValueSeparator != default)
        {
            ExpressionSyntax value  = MakeLiteral(field.KeyValueSeparator);
            ArgumentSyntax argument = value.AsArgument(nameof(ConfigLoader.Utils.ParseOptions.KeyValueSeparator));
            options.Add(argument);
        }

        return options.Count is not 0 ? ParseOptions.New(options.ToArray()) : DefaultOptions;
    }
    #endregion

    #region Values
    /// <summary>
    /// Generate the value load code implementation
    /// </summary>
    /// <param name="value">Value expression</param>
    /// <param name="field">Field data</param>
    /// <param name="context">Generation context</param>
    /// <returns>The modified statement body</returns>
    /// <exception cref="InvalidOperationException">If the generator does not know how to load the given field type</exception>
    public static BlockSyntax GenerateValueLoad(ExpressionSyntax value, in ConfigFieldMetadata field, in ConfigBuilderContext context)
    {
        // Find best load option
        if (AssignableTypes.Contains(field.Type.FullName))
        {
            return GenerateAssignValueLoad(value, field, context);
        }

        if (field.Type.IsSimpleValueType)
        {
            return GenerateTryParseValueLoad(value, GenerateTryParseValueInvocation, field, context);
        }

        if (field.Type.IsArray)
        {
            if (!field.IsMultipleValuesCollection)
            {
                return GenerateTryParseValueLoad(value, GenerateTryParseSimpleCollectionInvocation, field, context);
            }

            TypeSyntax elementType = field.Type.ElementType!.Identifier;
            return GenerateTryParseValueLoad(value, GenerateTryParseValueInvocation, field, context, elementType);

        }

        if (field.Type.IsKeyValueType)
        {
            return field.Type.IsSupportedDictionary || field.Type.IsKeyValuePair
                ? GenerateTryParseValueLoad(value, GenerateTryParseSimpleDictionaryInvocation, field, context)
                : GenerateTryParseValueLoad(value, GenerateTryParseDictionaryInvocation, field, context);
        }

        if (field.Type.IsSupportedCollection)
        {
            if (!field.IsMultipleValuesCollection)
            {
                return GenerateTryParseValueLoad(value, GenerateTryParseSimpleCollectionInvocation, field, context);
            }

            TypeSyntax elementType = field.Type.ElementType!.Identifier;
            return GenerateTryParseValueLoad(value, GenerateTryParseValueInvocation, field, context, elementType);
        }

        // ReSharper disable once InvertIf
        if (field.Type.IsICollection)
        {
            if (!field.IsMultipleValuesCollection)
            {
                return GenerateTryParseValueLoad(value, GenerateTryParseCollectionInvocation, field, context);
            }

            TypeSyntax elementType = field.Type.ElementType!.Identifier;
            return GenerateTryParseValueLoad(value, GenerateTryParseValueInvocation, field, context, elementType);
        }

        // Unknown type
        throw new InvalidOperationException($"Unknown value type to parse ({field.Type.FullName})");
    }

    /// <summary>
    /// Generate the value load code implementation using assignment
    /// </summary>
    /// <param name="value">Value expression</param>
    /// <param name="field">Field data</param>
    /// <param name="context">Generation context</param>
    /// <returns>The modified statement body</returns>
    private static BlockSyntax GenerateAssignValueLoad(ExpressionSyntax value, in ConfigFieldMetadata field, in ConfigBuilderContext context)
    {
        context.Token.ThrowIfCancellationRequested();

        // !string.IsNullOrEmpty(value.value)
        ExpressionSyntax isNullOrEmpty = SyntaxKind.StringKeyword.AsType().Access(IsNullOrEmpty);
        ExpressionSyntax isNotNullOrEmptyInvocation = Not(isNullOrEmpty.Invoke(value.AsArgument()));

        // this.value = value;
        ExpressionSyntax fieldAssign = This().Access(field.FieldName).Assign(value);
        BlockSyntax block = Block(fieldAssign.AsStatement());

        if (field.IsRequired)
        {
            // required.Add("name");
            ExpressionSyntax addInvocation = Required.Access(Add).Invoke(field.FieldName.AsLiteral().AsArgument());
            block = block.AddStatements(addInvocation.AsStatement());
        }

        // if(!string.IsNullOrEmpty(value.value))
        IfStatementSyntax ifStatement = If(isNotNullOrEmptyInvocation, block);

        // Add if statement and return
        return Block().AddStatements(ifStatement);
    }

    /// <summary>
    /// Generate the value load code implementation using <c>TryParse</c>
    /// </summary>
    /// <param name="value">Value expression</param>
    /// <param name="createTryParse">Delegate which creates the generated TryParse invocation</param>
    /// <param name="field">Field data</param>
    /// <param name="context">Generation context</param>
    /// <param name="type">Type to parse, defaults to the field type</param>
    /// <returns>The modified statement body</returns>
    private static BlockSyntax GenerateTryParseValueLoad(ExpressionSyntax value, TryParseInvocation createTryParse, in ConfigFieldMetadata field,
                                                         in ConfigBuilderContext context, TypeSyntax? type = null)
    {
        context.Token.ThrowIfCancellationRequested();

        // out Type _value
        ArgumentSyntax outVar = field.PrefixedName
                                     .Declaration(type ?? field.Type.Identifier)
                                     .AsArgument()
                                     .WithOut();

        // Create options from metadata
        ArgumentSyntax options = GenerateParseOptions(field).AsArgument();

        // ParseUtils.TryParse
        MemberAccessExpressionSyntax tryParse = ParseUtils.Access(TryParse);
        // Full TryParse invocation
        ExpressionSyntax tryParseInvocation = createTryParse(tryParse, value, outVar, options, field, context);

        ExpressionSyntax fieldAssign = field.IsMultipleValuesCollection
                                           ? field.CollectorName.Access(Add).Invoke(field.PrefixedName.AsArgument()) // valueCollector.Add(_value);
                                           : This().Access(field.FieldName).Assign(field.PrefixedName);              // this.value = _value;
        BlockSyntax block = Block(fieldAssign.AsStatement());

        if (field is { IsRequired: true, IsMultipleValuesCollection: false })
        {
            // required.Add("name");
            ExpressionSyntax addInvocation = Required.Access(Add).Invoke(field.FieldName.AsLiteral().AsArgument());
            block = block.AddStatements(addInvocation.AsStatement());
        }

        // if (ParseUtils.TryParse(value.value, out Type _value, options)) { }
        IfStatementSyntax ifStatement = If(tryParseInvocation, block);

        // Add namespace if the type isn't builtin
        context.UsedNamespaces.AddNamespaceName(UtilsNamespace);
        if (!field.Type.IsBuiltin)
        {
            context.UsedNamespaces.AddNamespace(field.Type.Namespace);
        }

        return Block().AddStatements(ifStatement);
    }

    /// <summary>
    /// Creates a TryParse invocation for simple values
    /// </summary>
    /// <param name="tryParse">TryParse member access</param>
    /// <param name="value">Value to parse</param>
    /// <param name="outVar">Output variable</param>
    /// <param name="options">Options argument</param>
    /// <param name="field">Field to parse into</param>
    /// <param name="context">Generation context</param>
    /// <returns>The created <c>TryParse</c> invocation</returns>
    private static InvocationExpressionSyntax GenerateTryParseValueInvocation(MemberAccessExpressionSyntax tryParse, ExpressionSyntax value, ArgumentSyntax outVar,
                                                                              ArgumentSyntax options, in ConfigFieldMetadata field, in ConfigBuilderContext context)
    {
        // ParseUtils.TryParse(value.value, out T result, options);
        return tryParse.Invoke(value.AsArgument(), outVar, options);
    }

    /// <summary>
    /// Creates a TryParse invocation for arrays
    /// </summary>
    /// <param name="tryParse">TryParse member access</param>
    /// <param name="value">Value to parse</param>
    /// <param name="outVar">Output variable</param>
    /// <param name="options">Options argument</param>
    /// <param name="field">Field to parse into</param>
    /// <param name="context">Generation context</param>
    /// <returns>The created <c>TryParse</c> invocation</returns>
    private static InvocationExpressionSyntax GenerateTryParseSimpleCollectionInvocation(MemberAccessExpressionSyntax tryParse, ExpressionSyntax value, ArgumentSyntax outVar,
                                                                              ArgumentSyntax options, in ConfigFieldMetadata field, in ConfigBuilderContext context)
    {
        // ParseUtils.TryParse(value.value, out T[] result, ParseUtils.TryParse, options);
        return tryParse.Invoke(value.AsArgument(), outVar, tryParse.AsArgument(), options);
    }

    /// <summary>
    /// Creates a TryParse invocation for <see cref="ICollection{T}"/> implementations
    /// </summary>
    /// <param name="tryParse">TryParse member access</param>
    /// <param name="value">Value to parse</param>
    /// <param name="outVar">Output variable</param>
    /// <param name="options">Options argument</param>
    /// <param name="field">Field to parse into</param>
    /// <param name="context">Generation context</param>
    /// <returns>The created <c>TryParse</c> invocation</returns>
    private static InvocationExpressionSyntax GenerateTryParseCollectionInvocation(MemberAccessExpressionSyntax tryParse, ExpressionSyntax value, ArgumentSyntax outVar,
                                                                                   ArgumentSyntax options, in ConfigFieldMetadata field, in ConfigBuilderContext context)
    {
        // Get collection element type
        TypeInfo elementSymbol = field.Type.ElementType!;

        // ParseUtils.TryParse<TCollection, TElement>
        GenericNameSyntax tryParseGenericName = TryParse.AsGenericName(field.Type.Identifier, elementSymbol.Identifier);
        ExpressionSyntax tryParseGeneric = tryParse.WithName(tryParseGenericName);

        // Add element namespace
        context.UsedNamespaces.AddNamespace(elementSymbol.Namespace);

        // ParseUtils.TryParse<TCollection, TElement>(value.value, out TCollection result, ParseUtils.TryParse, options);
        return tryParseGeneric.Invoke(value.AsArgument(), outVar, tryParse.AsArgument(), options);
    }

    /// <summary>
    /// Creates a TryParse invocation for <see cref="IDictionary{TKey,TValue}"/> implementations
    /// </summary>
    /// <param name="tryParse">TryParse member access</param>
    /// <param name="value">Value to parse</param>
    /// <param name="outVar">Output variable</param>
    /// <param name="options">Options argument</param>
    /// <param name="field">Field to parse into</param>
    /// <param name="context">Generation context</param>
    /// <returns>The created <c>TryParse</c> invocation</returns>
    private static InvocationExpressionSyntax GenerateTryParseSimpleDictionaryInvocation(MemberAccessExpressionSyntax tryParse, ExpressionSyntax value, ArgumentSyntax outVar,
                                                                                         ArgumentSyntax options, in ConfigFieldMetadata field, in ConfigBuilderContext context)
    {
        // ParseUtils.TryParse(value.value, out TDict result, ParseUtils.TryParse, ParseUtils.TryParse, options);
        ArgumentSyntax tryParseArgument = tryParse.AsArgument();
        return tryParse.Invoke(value.AsArgument(), outVar, tryParseArgument, tryParseArgument, options);
    }

    /// <summary>
    /// Creates a TryParse invocation for <see cref="IDictionary{TKey,TValue}"/> implementations
    /// </summary>
    /// <param name="tryParse">TryParse member access</param>
    /// <param name="value">Value to parse</param>
    /// <param name="outVar">Output variable</param>
    /// <param name="options">Options argument</param>
    /// <param name="field">Field to parse into</param>
    /// <param name="context">Generation context</param>
    /// <returns>The created <c>TryParse</c> invocation</returns>
    private static InvocationExpressionSyntax GenerateTryParseDictionaryInvocation(MemberAccessExpressionSyntax tryParse, ExpressionSyntax value, ArgumentSyntax outVar,
                                                                                   ArgumentSyntax options, in ConfigFieldMetadata field, in ConfigBuilderContext context)
    {
        // Get dictionary key/value types
        TypeInfo keyType = field.Type.KeyType!;
        TypeInfo valueType = field.Type.ValueType!;

        // ParseUtils.TryParse<TDict, TKey, TValue>
        GenericNameSyntax tryParseGenericName = TryParse.AsGenericName(field.Type.Identifier, keyType.Identifier, valueType.Identifier);
        ExpressionSyntax tryParseGeneric = tryParse.WithName(tryParseGenericName);

        // Add key/value namespaces
        context.UsedNamespaces.AddNamespace(keyType.Namespace);
        context.UsedNamespaces.AddNamespace(valueType.Namespace);

        // ParseUtils.TryParse<TDict, TKey, TValue>(value.value, out TDict result, ParseUtils.TryParse, ParseUtils.TryParse, options);
        ArgumentSyntax tryParseArgument = tryParse.AsArgument();
        return tryParseGeneric.Invoke(value.AsArgument(), outVar, tryParseArgument, tryParseArgument, options);
    }
    #endregion

    #region Nodes
    /// <summary>
    /// Generate the node load code implementation
    /// </summary>
    /// <param name="value">Value expression</param>
    /// <param name="field">Field data</param>
    /// <param name="context">Generation context</param>
    /// <returns>The modified statement body</returns>
    /// <exception cref="InvalidOperationException">If the generator does not know how to load the given field type</exception>
    public static BlockSyntax GenerateNodeLoad(ExpressionSyntax value, in ConfigFieldMetadata field, in ConfigBuilderContext context)
    {
        // Find best save option
        if (field.Type.IsIConfigNode)
        {
            return GenerateInterfaceNodeLoad(value, field, context);
        }

        if (field.Type.IsConfigNode)
        {
            return GenerateNodeAssignLoad(value, field, context);
        }

        // Unknown type
        throw new InvalidOperationException($"Unknown node type to parse ({field.Type.FullName})");
    }

    /// <summary>
    /// Generate the IConfigNode node load code implementation
    /// </summary>
    /// <param name="value">Value expression</param>
    /// <param name="field">Field data</param>
    /// <param name="context">Generation context</param>
    /// <returns>The modified statement body</returns>
    private static BlockSyntax GenerateInterfaceNodeLoad(ExpressionSyntax value, in ConfigFieldMetadata field, in ConfigBuilderContext context)
    {
        context.Token.ThrowIfCancellationRequested();

        // this.value
        ExpressionSyntax fieldAccess = This().Access(field.FieldName);
        // this.value = new Type();
        ExpressionSyntax instantiation = fieldAccess.Assign(field.Type.Identifier.New());

        // Check if method implementation is explicit
        if (field.Type.Symbol.IsInterfaceImplementationExplicit(IConfigNode.AsRaw(), Load.AsRaw()))
        {
            // ((IConfigNode)this.value)
            fieldAccess = fieldAccess.Cast(IConfigNode);
        }

        // this.value.Load(value)
        ExpressionSyntax loadConfig = fieldAccess.Access(Load).Invoke(value.AsArgument());
        BlockSyntax block = Block(instantiation.AsStatement(), loadConfig.AsStatement());

        if (field.IsRequired)
        {
            // required.Add("name");
            ExpressionSyntax addInvocation = Required.Access(Add).Invoke(field.FieldName.AsLiteral().AsArgument());
            block = block.AddStatements(addInvocation.AsStatement());
        }

        // Add statements and return
        return block;
    }

    /// <summary>
    /// Generate the ConfigNode assign node load code implementation
    /// </summary>
    /// <param name="value">Value expression</param>
    /// <param name="field">Field data</param>
    /// <param name="context">Generation context</param>
    /// <returns>The modified statement body</returns>
    private static BlockSyntax GenerateNodeAssignLoad(ExpressionSyntax value, in ConfigFieldMetadata field, in ConfigBuilderContext context)
    {
        context.Token.ThrowIfCancellationRequested();

        // this.value = value;
        ExpressionSyntax fieldAssign = This().Access(field.FieldName).Assign(value);
        BlockSyntax block = Block(fieldAssign.AsStatement());

        if (field.IsRequired)
        {
            // required.Add("name");
            ExpressionSyntax addInvocation = Required.Access(Add).Invoke(field.FieldName.AsLiteral().AsArgument());
            block = block.AddStatements(addInvocation.AsStatement());
        }

        // Add statements and return
        return block;
    }
    #endregion
}
