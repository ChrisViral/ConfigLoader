using System;
using ConfigLoader.Attributes;
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

    [ConfigField(Name = "OtherName")]
    public Vector3 VectorProperty { get; set; }

    public void Test(ConfigNode node)
    {
        for (int i = 0; i < node.CountNodes; i++)
        {
            ConfigNode value = node.nodes[i];
            switch (value.name)
            {
                case "floatCurve":
                {
                    this.floatCurve = new FloatCurve();
                    ((IConfigNode)this.floatCurve).Load(value);
                    break;
                }

                case "configNode":
                {
                    this.configNode = value;
                    break;
                }
            }
        }
    }
}