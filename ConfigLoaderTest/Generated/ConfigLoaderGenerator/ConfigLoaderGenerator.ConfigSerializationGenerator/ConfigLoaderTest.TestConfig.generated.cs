﻿// <auto-generated/>
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
                        if (ParseUtils.TryParse(value.value, out AccessModifier _modifier, ParseOptions.Defaults))
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

                    case "OtherName":
                    {
                        if (ParseUtils.TryParse(value.value, out Vector3 _VectorProperty, ParseOptions.Defaults))
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

        /// <summary>
        /// Auto-generated <see cref="ConfigNode"/> save
        /// </summary>
        /// <param name="node"><see cref="ConfigNode"/> to save to</param>
        public void SaveToConfig(ConfigNode node)
        {
            node.AddValue("intValue", WriteUtils.Write(this.intValue, WriteOptions.Defaults));
            node.AddValue("floatValue", WriteUtils.Write(this.floatValue, WriteOptions.Defaults));
            node.AddValue("stringValue", this.stringValue);
            node.AddValue("modifier", WriteUtils.Write(this.modifier, WriteOptions.Defaults));
            node.AddValue("OtherName", WriteUtils.Write(this.VectorProperty, WriteOptions.Defaults));
            ((IConfigNode)this.floatCurve).Save(node.AddNode("floatCurve"));
            node.AddNode("configNode", this.configNode);
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
