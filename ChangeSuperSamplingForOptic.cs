using BepInEx;
using ChangeSuperSamplingForOptic.Configs;
using EFT;

namespace ChangeSuperSamplingForOptic
{
    [BepInPlugin("com.MarsyApp.ChangeSuperSamplingForOptic", "MarsyApp-ChangeSuperSamplingForOptic", "1.0.0")]
    public class ChangeSuperSamplingForOptic : BaseUnityPlugin
    {
        static Player _localPlayer = null;
        
        static CameraClass _camera = null;
        
        public static Player getPlayetInstance()
        {
            if (_localPlayer != null)
            {
                return _localPlayer;
            }
            
            _localPlayer = FindObjectOfType<Player>();
            return _localPlayer;
        }
        
        public static CameraClass getCameraInstance()
        {
            if (_camera != null)
            {
                return _camera;
            }
            
            _camera = CameraClass.Instance;
            return _camera;
        }
        
        private void Awake()
        {
            ChangeSuperSamplingForOpticConfig.Init(Config);
            Patcher.PatchAll();
            Logger.LogInfo($"Plugin ChangeSuperSamplingForOpticMod is loaded!");
        }
        
        private void OnDestroy()
        {
            Patcher.UnpatchAll();
            Logger.LogInfo($"Plugin ChangeSuperSamplingForOpticMod is unloaded!");
        }
    }
}
