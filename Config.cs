using BepInEx.Configuration;

namespace ChangeSuperSamplingForOptic.Configs
{
    internal class ChangeSuperSamplingForOpticConfig
    {
        public static ConfigEntry<bool> EnableMod { get; private set; }
        public static ConfigEntry<float> SuperSampling { get; private set; }

        public static void Init(ConfigFile Config)
        {
            string debugmode = "Settings";

            EnableMod = Config.Bind(debugmode, "Включить мод", true, "Уменьшает суперсемплинг для для основного изображения, когда смотрим через оптику.");
            SuperSampling = Config.Bind(debugmode, "Суперсемплинг", 0.5f, "Значение суперсемплинга для основного изображения, когда смотрим через оптику. От 0.01 до 1.0. По умолчанию 0.5");
        }
    }
}