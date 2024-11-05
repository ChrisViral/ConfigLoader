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
    [ConfigField(IsRequired = true)]
    public string stringValue = string.Empty;
    [ConfigField(EnumHandling = EnumHandling.Flags)]
    public AccessModifier modifier = AccessModifier.Public;
    [ConfigField(IsRequired = true)]
    public FloatCurve floatCurve;
    [ConfigField]
    public ConfigTest explicitImplementation;
    [ConfigField]
    public ConfigNode configNode;
    [ConfigField]
    public int[] intArray;
    [ConfigField(IsRequired = true, CollectionSeparator = ',')]
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

    [ConfigField(IsRequired = true, Name = "OtherName", Separator = ' ', SplitOptions = ExtendedSplitOptions.RemoveEmptyEntries)]
    public Vector3 VectorProperty { get; set; }

    public void Test(ConfigNode node)
    {
        string _intList = WriteUtils.Write(this.intList, WriteUtils.Write, WriteOptions.Defaults);
        if (string.IsNullOrEmpty(_intList))
        {
            throw new MissingRequiredConfigFieldException("", "");
        }

        node.AddValue("", _intList);
    }
}
