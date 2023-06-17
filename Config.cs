using BepInEx.Configuration;
using EFT.Settings.Graphics;

namespace ChangeSuperSamplingForOptic.Configs
{
    internal class ChangeSuperSamplingForOpticConfig
    {
        //public class DLSSModeQuality
        //{
        //    public static EDLSSMode Quality;
        //    public static EDLSSMode Balanced;
        //    public static EDLSSMode Performance;
        //    public static EDLSSMode UltraPerformance;
        //}

        public class FSRModeQuality
        {
            public static EFSRMode UltraQuality;
            public static EFSRMode Quality;
            public static EFSRMode Balanced;
            public static EFSRMode Performance;
        }

        public class FSR2ModeQuality
        {
            public static EFSR2Mode Quality;
            public static EFSR2Mode Balanced;
            public static EFSR2Mode Performance;
            public static EFSR2Mode UltraPerformance;
        }

        public static ConfigEntry<bool> EnableMod { get; set; }
        public static ConfigEntry<float> SuperSampling { get; set; }

        //public static ConfigEntry<EDLSSMode> DLSSMode { get; set; }
        public static ConfigEntry<EFSRMode> FSRMode { get; set; }
        public static ConfigEntry<EFSR2Mode> FSR2Mode { get; set; }

        public static void Init(ConfigFile Config)
        {
            string scaling = "Settings";

            EnableMod = Config.Bind(scaling, "Enable Mod", true, new ConfigDescription("Reducing the resolution of the external rendering, when aiming through the telescopic sight.", null, new ConfigurationManagerAttributes { Order = 5 }));
            SuperSampling = Config.Bind(scaling, "Sampling Downgrade", 0.495f, new ConfigDescription("Percentage of how much the external rendering will go down, when aiming through the telescopic sight. Default value 50%.", new AcceptableValueRange<float>(0f, 0.99f), new ConfigurationManagerAttributes { ShowRangeAsPercent = true, Order = 4 }));
            //DLSSMode = Config.Bind(scaling, "DLSS Mode", EDLSSMode.Performance, new ConfigDescription("Percentage of how much the external rendering will go down, when aiming through the telescopic sight. Default value 25%.", null, new ConfigurationManagerAttributes { Order = 3 }));
            FSRMode = Config.Bind(scaling, "FSR Scaling Mode", EFSRMode.Performance, new ConfigDescription("Image scaling mode when using FSR, which will change external rendering to this level when you aiming through the telescopic scope. Default is Performance Mode (50% resolution downgrade).", null, new ConfigurationManagerAttributes { Order = 2 }));
            FSR2Mode = Config.Bind(scaling, "FSR2 Scaling Mode", EFSR2Mode.Performance, new ConfigDescription("Image scaling mode when using FSR2, which will change external rendering to this level when you aiming through the telescopic scope. Default is Performance Mode (50% resolution downgrade).", null, new ConfigurationManagerAttributes { Order = 1 }));
        }
    }
}