/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

/*
 * This file is meant as a way to export some declared names to the generator environment without having to include their respective references.
 * Basically the same idea as C++ forward declarations.
 */

namespace ConfigLoader
{
    public partial interface IGeneratedConfigNode;

    namespace Utils
    {
        public partial class ParseUtils;
    }

    namespace Exceptions
    {
        public partial class MissingRequiredConfigFieldException;
    }
}
