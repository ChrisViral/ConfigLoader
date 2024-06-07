using System;
using System.Threading;
using ConfigLoader.Attributes;
using ConfigLoaderGenerator.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

namespace ConfigLoaderGenerator;

/// <summary>
/// ConfigNode serializer load/save generator
/// </summary>
[Generator]
public class ConfigSerializationGenerator : IIncrementalGenerator
{
    #region Generator
    /// <inheritdoc />
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<ConfigBuilder> configDataProvider = context.SyntaxProvider.CreateSyntaxProvider(FilterConfigClasses, CreateConfigTemplate);
        context.RegisterSourceOutput(configDataProvider, GenerateConfigMethods);
    }

    /// <summary>
    /// Filter out classes with the ConfigObject attribute
    /// </summary>
    /// <param name="syntax">Inspected Syntax node</param>
    /// <param name="token">Cancellation token</param>
    /// <returns><see langword="true"/> if the <paramref name="syntax"/> is a class with a ConfigObject attribute, otherwise <see langword="false"/></returns>
    /// <exception cref="OperationCanceledException">If the operation is cancelled through the <paramref name="token"/></exception>
    private static bool FilterConfigClasses(SyntaxNode syntax, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        // Make sure we are on a supported type of object
        TypeDeclarationSyntax? typeSyntax = syntax switch
        {
            ClassDeclarationSyntax classSyntax   => classSyntax,
            StructDeclarationSyntax structSyntax => structSyntax,
            RecordDeclarationSyntax recordSyntax => recordSyntax,
            _                                    => null
        };

        // TODO: check nested types
        return typeSyntax is not null                                // Valid kind of type
            && typeSyntax.Modifiers.Any(SyntaxKind.PartialKeyword)   // Is declared as partial
            && !typeSyntax.Modifiers.Any(SyntaxKind.AbstractKeyword) // Is not declared abstract
            && typeSyntax.HasAttribute<ConfigObjectAttribute>();
    }

    /// <summary>
    /// Creates the <see cref="ConfigBuilder"/> associated with the current syntax context
    /// </summary>
    /// <param name="context">Current generator context</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>The created <see cref="ConfigBuilder"/></returns>
    /// <exception cref="OperationCanceledException">If the operation is cancelled through the <paramref name="token"/></exception>
    private static ConfigBuilder CreateConfigTemplate(GeneratorSyntaxContext context, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        // Create config template source generation
        return new ConfigBuilder(context);
    }

    /// <summary>
    /// Generates the source file for the given template
    /// </summary>
    /// <param name="context">Current source context</param>
    /// <param name="builder">Config template to generate the source for</param>
    /// <exception cref="OperationCanceledException">If the operation is cancelled through the <paramref name="context"/></exception>
    private static void GenerateConfigMethods(SourceProductionContext context, ConfigBuilder builder)
    {
        context.CancellationToken.ThrowIfCancellationRequested();

        // Generate source file and add to compilation
        (string fileName, string source) = builder.GenerateSource();
        context.AddSource(fileName, source);
    }
    #endregion
}