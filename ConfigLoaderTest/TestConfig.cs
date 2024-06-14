using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ConfigLoader.Attributes;
using ConfigLoader.Exceptions;
using ConfigLoader.Utils;
using UnityEngine;

namespace ConfigLoaderTest;

public struct ConfigTest : IConfigNode
{
    #region Implementation of IConfigNode
    /// <inheritdoc />
    void IConfigNode.Load(ConfigNode node) { }

    /// <inheritdoc />
    void IConfigNode.Save(ConfigNode node) { }
    #endregion
}

[ConfigObject(LoadMethodAccess = AccessModifier.Public, SaveMethodAccess = AccessModifier.Public)]
public partial class TestConfig
{
    [ConfigField]
    public int intValue = 0;
    [ConfigField]
    public float floatValue = 0f;
    [ConfigField]
    public string stringValue = string.Empty;
    [ConfigField(EnumHandling = EnumHandling.Flags)]
    public AccessModifier modifier = AccessModifier.Public;
    [ConfigField]
    public FloatCurve floatCurve;
    [ConfigField]
    public ConfigTest explicitImplementation;
    [ConfigField]
    public ConfigNode configNode;
    [ConfigField]
    public int[] intArray;
    [ConfigField(CollectionSeparator = ',')]
    public List<int> intList;
    [ConfigField]
    public HashSet<string> stringHashSet;
    [ConfigField]
    public LinkedList<long> longLinkedList;
    [ConfigField]
    public Queue<object> objectQueue;
    [ConfigField]
    public Stack<char> charStack;
    [ConfigField]
    public ReadOnlyCollection<double> doubleReadOnlyCollection;
    [ConfigField(KeyValueSeparator = '|')]
    public Dictionary<string, decimal> stringDecimalDictionary;

    [ConfigField(Name = "OtherName", Separator = ' ', SplitOptions = ExtendedSplitOptions.RemoveEmptyEntries)]
    public Vector3 VectorProperty { get; set; }

    public void Test(ConfigNode node)
    {
        HashSet<string> required = [];
        for (int i = 0; i < node.CountValues; i++)
        {
            ConfigNode.Value value = node.values[i];
            switch (value.name)
            {
                case "intValue":
                {
                    if (ParseUtils.TryParse(value.value, out int _intValue, ParseOptions.Defaults))
                    {
                        this.intValue = _intValue;
                        required.Add("intValue");
                    }
                    break;
                }
                case "charStack":
                {
                    if (ParseUtils.TryParse(value.value, out Stack<char> _charStack, ParseUtils.TryParse, ParseOptions.Defaults))
                    {
                        this.charStack = _charStack;
                        required.Add("charStack");
                    }
                    break;
                }
            }
        }

        if (!required.Contains("intValue"))
        {
            throw new MissingRequiredConfigFieldException("Required config field is missing", "intValue");
        }
        if (!required.Contains("intValue"))
        {
            throw new MissingRequiredConfigFieldException("Required config field is missing", "charStack");
        }
    }
}
