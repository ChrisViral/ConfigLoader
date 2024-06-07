﻿using ConfigLoader.Attributes;

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
}
