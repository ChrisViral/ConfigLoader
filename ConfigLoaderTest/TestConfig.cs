using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ConfigLoader.Attributes;
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
    [ConfigField]
    public AccessModifier modifier = AccessModifier.Public;
    [ConfigField]
    public FloatCurve floatCurve;
    [ConfigField]
    public ConfigTest explicitImplementation;
    [ConfigField]
    public ConfigNode configNode;
    [ConfigField]
    public int[] intArray;
    [ConfigField]
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
    [ConfigField]
    public Dictionary<string, decimal> stringDecimalDictionary;

    [ConfigField(Name = "OtherName")]
    public Vector3 VectorProperty { get; set; }

    public void Test(ConfigNode node)
    {
        if (ParseUtils.TryParse("value.value", out Dictionary<string, decimal> _stringDecimalDictionary, ParseUtils.TryParse, ParseUtils.TryParse, ParseOptions.Defaults))
        {
            this.stringDecimalDictionary = _stringDecimalDictionary;
        }

        WriteUtils.Write(this.stringDecimalDictionary, WriteUtils.Write, WriteUtils.Write, WriteOptions.Defaults);
    }
}