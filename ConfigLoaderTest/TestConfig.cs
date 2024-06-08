using ConfigLoader.Attributes;

namespace ConfigLoaderTest;

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
        for (int i = 0; i < node.CountValues; i++)
        {
            ConfigNode.Value value = node.values[i];
            switch (value.name)
            {
                case nameof(this.intValue):
                    if (int.TryParse(value.value, out int _intValue))
                    {
                        this.intValue = _intValue;
                    }
                    break;

                case nameof(this.floatValue):
                    if (float.TryParse(value.value, out float _floatValue))
                    {
                        this.floatValue = _floatValue;
                    }
                    break;
            }
        }
    }
}