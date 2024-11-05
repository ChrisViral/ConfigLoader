/* ConfigLoader is distributed under CC BY-NC-SA 4.0 INTL (https://creativecommons.org/licenses/by-nc-sa/4.0/).                           *\
 * You are free to redistribute, share, adapt, etc. as long as the original author (stupid_chris/Christophe Savard) is properly, clearly, *
\* and explicitly credited, that you do not use this material to a commercial use, and that you distribute it under the same license.     */

/*
 * This file is a counterpart to the ConfigLoader project's NameExport.cs
 * This ensures everything compiles by defining dummy method bodies locally for imported names
 */

namespace ConfigLoader.Utils
{
    public partial class ParseUtils
    {
        public static partial bool TryParse(string? value, out byte result, in ParseOptions options)
        {
            result = default;
            return default;
        }
    }

    public partial class WriteUtils
    {
        public static partial string Write(byte value, in WriteOptions options) => default!;
    }
}
