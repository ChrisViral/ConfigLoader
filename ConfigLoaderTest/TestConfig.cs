using ConfigLoader.Attributes;

namespace ConfigLoaderTest.Test
{
    [ConfigObject(LoadMethodAccess = AccessModifier.Public, SaveMethodAccess = AccessModifier.Public)]
    public partial class TestConfig
    {
        [ConfigField]
        public int intValue = 0;
        [ConfigField]
        public float floatValue = 0f;
        [ConfigField]
        public string stringValue = string.Empty;

        [ConfigField]
        public string StringProperty { get; set; }

        public void Test(ConfigNode node)
        {
        }
    }
}