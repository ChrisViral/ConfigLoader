using ConfigLoader;
using GeneratedCode;
using UnityEngine;

namespace ConfigLoaderTest;

[KSPAddon(KSPAddon.Startup.Instantly, true), ConfigObject]
public class ConfigLoaderTest : MonoBehaviour
{
    [ConfigField]
    public string test;

    private void Start()
    {
        Test.Foo();
    }
}
