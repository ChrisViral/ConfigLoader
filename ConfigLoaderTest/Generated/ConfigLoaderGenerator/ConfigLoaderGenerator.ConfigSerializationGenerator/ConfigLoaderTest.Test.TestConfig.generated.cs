﻿// <auto-generated/>
using ConfigLoader.Utils;
using UnityEngine;

namespace ConfigLoaderTest.Test
{
    public partial class TestConfig
    {
        public void LoadFromConfig(ConfigNode node)
        {
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
        }

        public void SaveToConfig(ConfigNode node)
        {
            node.AddValue("intValue", this.intValue);
            node.AddValue("floatValue", this.floatValue);
            node.AddValue("stringValue", this.stringValue);
            node.AddValue("OtherName", this.VectorProperty);
        }
    }
}
