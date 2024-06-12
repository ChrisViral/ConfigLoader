using ConfigLoader.Attributes;
using ConfigLoader.Utils;
using UnityEngine;

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
    public AccessModifier modifier = AccessModifier.Public;
    [ConfigField]
    public FloatCurve floatCurve;
    [ConfigField]
    public ConfigNode configNode;
    [ConfigField]
    public int[] intArray;

    [ConfigField(Name = "OtherName")]
    public Vector3 VectorProperty { get; set; }

    public void Test(ConfigNode node)
    {
    }
}