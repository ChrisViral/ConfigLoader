﻿// <auto-generated/>
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ConfigLoader;
using ConfigLoader.Attributes;
using ConfigLoader.Utils;
using UnityEngine;

namespace ConfigLoaderTest
{
    public partial class TestConfig : IGeneratedConfigNode
    {
        /// <summary>
        /// Auto-generated <see cref="ConfigNode"/> load
        /// </summary>
        /// <param name="node"><see cref="ConfigNode"/> to load from</param>
        public void LoadFromConfig(ConfigNode node)
        {
            if (node == null)
            {
                return;
            }

            int valueCount = node.CountValues;
            for (int i = 0; i < valueCount; i++)
            {
                ConfigNode.Value value = node.values[i];
                switch (value.name)
                {
                    case "intValue":
                    {
                        if (ParseUtils.TryParse(value.value, out int _intValue, ParseOptions.Defaults))
                        {
                            this.intValue = _intValue;
                        }

                        break;
                    }

                    case "floatValue":
                    {
                        if (ParseUtils.TryParse(value.value, out float _floatValue, ParseOptions.Defaults))
                        {
                            this.floatValue = _floatValue;
                        }

                        break;
                    }

                    case "stringValue":
                    {
                        if (!string.IsNullOrEmpty(value.value))
                        {
                            this.stringValue = value.value;
                        }

                        break;
                    }

                    case "modifier":
                    {
                        if (ParseUtils.TryParse(value.value, out AccessModifier _modifier, new ParseOptions(EnumHandling: EnumHandling.Flags)))
                        {
                            this.modifier = _modifier;
                        }

                        break;
                    }

                    case "intArray":
                    {
                        if (ParseUtils.TryParse(value.value, out int[] _intArray, ParseUtils.TryParse, ParseOptions.Defaults))
                        {
                            this.intArray = _intArray;
                        }

                        break;
                    }

                    case "intList":
                    {
                        if (ParseUtils.TryParse(value.value, out List<int> _intList, ParseUtils.TryParse, new ParseOptions(CollectionSeparator: ',')))
                        {
                            this.intList = _intList;
                        }

                        break;
                    }

                    case "stringHashSet":
                    {
                        if (ParseUtils.TryParse(value.value, out HashSet<string> _stringHashSet, ParseUtils.TryParse, ParseOptions.Defaults))
                        {
                            this.stringHashSet = _stringHashSet;
                        }

                        break;
                    }

                    case "longLinkedList":
                    {
                        if (ParseUtils.TryParse(value.value, out LinkedList<long> _longLinkedList, ParseUtils.TryParse, ParseOptions.Defaults))
                        {
                            this.longLinkedList = _longLinkedList;
                        }

                        break;
                    }

                    case "objectQueue":
                    {
                        if (ParseUtils.TryParse(value.value, out Queue<object> _objectQueue, ParseUtils.TryParse, ParseOptions.Defaults))
                        {
                            this.objectQueue = _objectQueue;
                        }

                        break;
                    }

                    case "charStack":
                    {
                        if (ParseUtils.TryParse(value.value, out Stack<char> _charStack, ParseUtils.TryParse, ParseOptions.Defaults))
                        {
                            this.charStack = _charStack;
                        }

                        break;
                    }

                    case "doubleReadOnlyCollection":
                    {
                        if (ParseUtils.TryParse(value.value, out ReadOnlyCollection<double> _doubleReadOnlyCollection, ParseUtils.TryParse, ParseOptions.Defaults))
                        {
                            this.doubleReadOnlyCollection = _doubleReadOnlyCollection;
                        }

                        break;
                    }

                    case "stringDecimalDictionary":
                    {
                        if (ParseUtils.TryParse(value.value, out Dictionary<string, decimal> _stringDecimalDictionary, ParseUtils.TryParse, ParseUtils.TryParse, new ParseOptions(KeyValueSeparator: '|')))
                        {
                            this.stringDecimalDictionary = _stringDecimalDictionary;
                        }

                        break;
                    }

                    case "OtherName":
                    {
                        if (ParseUtils.TryParse(value.value, out Vector3 _VectorProperty, new ParseOptions(SplitOptions: ExtendedSplitOptions.RemoveEmptyEntries, Separator: ' ')))
                        {
                            this.VectorProperty = _VectorProperty;
                        }

                        break;
                    }
                }
            }

            int nodeCount = node.CountNodes;
            for (int i = 0; i < nodeCount; i++)
            {
                ConfigNode value = node.nodes[i];
                switch (value.name)
                {
                    case "floatCurve":
                    {
                        this.floatCurve = new FloatCurve();
                        this.floatCurve.Load(value);
                        break;
                    }

                    case "explicitImplementation":
                    {
                        this.explicitImplementation = new ConfigTest();
                        ((IConfigNode)this.explicitImplementation).Load(value);
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

        /// <summary>
        /// Auto-generated <see cref="ConfigNode"/> save
        /// </summary>
        /// <param name="node"><see cref="ConfigNode"/> to save to</param>
        public void SaveToConfig(ConfigNode node)
        {
            if (node == null)
            {
                return;
            }

            node.AddValue("intValue", WriteUtils.Write(this.intValue, WriteOptions.Defaults));
            node.AddValue("floatValue", WriteUtils.Write(this.floatValue, WriteOptions.Defaults));
            node.AddValue("stringValue", this.stringValue);
            node.AddValue("modifier", WriteUtils.Write(this.modifier, new WriteOptions(EnumHandling: EnumHandling.Flags)));
            node.AddValue("intArray", WriteUtils.Write(this.intArray, WriteUtils.Write, WriteOptions.Defaults));
            node.AddValue("intList", WriteUtils.Write(this.intList, WriteUtils.Write, new WriteOptions(CollectionSeparator: ',')));
            node.AddValue("stringHashSet", WriteUtils.Write(this.stringHashSet, WriteUtils.Write, WriteOptions.Defaults));
            node.AddValue("longLinkedList", WriteUtils.Write(this.longLinkedList, WriteUtils.Write, WriteOptions.Defaults));
            node.AddValue("objectQueue", WriteUtils.Write(this.objectQueue, WriteUtils.Write, WriteOptions.Defaults));
            node.AddValue("charStack", WriteUtils.Write(this.charStack, WriteUtils.Write, WriteOptions.Defaults));
            node.AddValue("doubleReadOnlyCollection", WriteUtils.Write(this.doubleReadOnlyCollection, WriteUtils.Write, WriteOptions.Defaults));
            node.AddValue("stringDecimalDictionary", WriteUtils.Write(this.stringDecimalDictionary, WriteUtils.Write, WriteUtils.Write, new WriteOptions(KeyValueSeparator: '|')));
            node.AddValue("OtherName", WriteUtils.Write(this.VectorProperty, new WriteOptions(Separator: ' ')));
            this.floatCurve?.Save(node.AddNode("floatCurve"));
            ((IConfigNode)this.explicitImplementation).Save(node.AddNode("explicitImplementation"));
            if (this.configNode != null)
            {
                node.AddNode("configNode", this.configNode);
            }
        }

#region IConfigNode Implementation
        void IConfigNode.Load(ConfigNode node)
        {
            LoadFromConfig(node);
        }

        void IConfigNode.Save(ConfigNode node)
        {
            SaveToConfig(node);
        }
#endregion
    }
}
