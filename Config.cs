using BepInEx.Configuration;

namespace ChangeSuperSamplingForOptic.Configs
{
    internal class ChangeSuperSamplingForOpticConfig
    {
        public static ConfigEntry<bool> EnableMod { get; private set; }
        public static ConfigEntry<float> SuperSampling { get; private set; }

        public static void Init(ConfigFile Config)
        {
            string scaling = "Settings";

            EnableMod = Config.Bind(scaling, "Enable Mod", true, new ConfigDescription("Reducing the resolution of the external rendering, when aiming through the telescopic sight.", null, new ConfigurationManagerAttributes { Order = 2 }));
            SuperSampling = Config.Bind(scaling, "Sampling Downgrade", 0.25f, new ConfigDescription("Percentage of how much the external rendering will go down, when aiming through the telescopic sight. Default value 25%.", new AcceptableValueRange<float>(0f, 0.99f), new ConfigurationManagerAttributes { ShowRangeAsPercent = true, Order = 1 }));
        }
    }
}