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

public class DerivedConfig : TestConfig;

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
    public Queue<Color> colorQueue;
    [ConfigField]
    public Stack<char> charStack;
    [ConfigField(CollectionHandling = CollectionHandling.MultipleValues)]
    public ReadOnlyCollection<double> doubleReadOnlyCollection;
    [ConfigField(KeyValueSeparator = '|', CollectionHandling = CollectionHandling.MultipleValues)]
    public Dictionary<string, decimal> stringDecimalDictionary;
    [ConfigField(CollectionHandling = CollectionHandling.MultipleValues)]
    public ReadOnlyDictionary<float, float> floatReadOnlyDictionary;
    [ConfigField(CollectionHandling = CollectionHandling.MultipleValues)]
    public SortedList<int, int> intSortedList;
    [ConfigField]
    public KeyValuePair<string, float> stringFloatPair;

    [ConfigField(IsRequired = true, Name = "OtherName", ValueSeparator = ' ', SplitOptions = ExtendedSplitOptions.RemoveEmptyEntries)]
    public Vector3 VectorProperty { get; set; }

    public void Test(ConfigNode node)
    {
        ParseUtils.TryParse("", out KeyValuePair<string, float> pair, ParseUtils.TryParse, ParseUtils.TryParse, ParseOptions.Defaults);
    }
}
