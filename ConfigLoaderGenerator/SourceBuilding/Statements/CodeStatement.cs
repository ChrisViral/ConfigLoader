namespace ConfigLoaderGenerator.SourceBuilding.Statements;

/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

/// <summary>
/// Code line statement
/// </summary>
/// <param name="statement">Code line</param>
public sealed class CodeStatement(string statement) : BaseStatement(statement);
