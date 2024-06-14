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
    [ConfigField(CollectionHandling = CollectionHandling.MultipleValues)]
    public int[] intArray;
    [ConfigField(IsRequired = true, CollectionSeparator = ',', CollectionHandling = CollectionHandling.MultipleValues)]
    public List<int> intList;
    [ConfigField(CollectionHandling = CollectionHandling.MultipleValues)]
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

    [ConfigField(IsRequired = true, Name = "OtherName", ValueSeparator = ' ', SplitOptions = ExtendedSplitOptions.RemoveEmptyEntries)]
    public Vector3 VectorProperty { get; set; }

    public void Test(ConfigNode node)
    {
        for (int i = 0; i < node.CountValues; i++)
        {
            List<string> list = new List<string>();
            ConfigNode.Value value = node.values[i];
            switch (value.name)
            {
                case "stringHashSetValue":
                {
                    if (!string.IsNullOrEmpty(value.value))
                    {
                        list.Add(value.value);
                    }
                    break;
                }
            }

            if (list.Count != 0)
            {
                this.stringHashSet = CollectionUtils.FromList<HashSet<string>, string>(list);
            }
        }
    }
}
