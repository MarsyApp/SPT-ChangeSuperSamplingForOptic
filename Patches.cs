using System.Collections.Generic;
using System.Reflection;
using Aki.Reflection.Patching;
using ChangeSuperSamplingForOptic.Configs;
using Comfort.Common;
using EFT;
using EFT.CameraControl;
using UnityEngine;
using EFT.Settings.Graphics;

namespace ChangeSuperSamplingForOptic
{
    class Patcher
    {
        public static void PatchAll()
        {
            new PatchManager().RunPatches();
        }
        
        public static void UnpatchAll()
        {
            new PatchManager().RunUnpatches();
        }
    }

    public class PatchManager
    {
        public PatchManager()
        {
            this._patches = new List<ModulePatch>
            {
                new ChangeSuperSamplingForOpticPatches.OpticSightOnEnablePath(),
                new ChangeSuperSamplingForOpticPatches.OpticSightOnDisablePath(),
                new ChangeSuperSamplingForOpticPatches.ClientFirearmControllerChangeAimingModePath(),
            };
        }

        public void RunPatches()
        {
            foreach (ModulePatch patch in this._patches)
            {
                patch.Enable();
            }
        }
        
        public void RunUnpatches()
        {
            foreach (ModulePatch patch in this._patches)
            {
                patch.Disable();
            }
        }

        private readonly List<ModulePatch> _patches;
    }

    public static class ChangeSuperSamplingForOpticPatches
    {
        private static void SetSuperSamplingAim()
        {
            float defaultSuperSamplingFactor = Singleton<SharedGameSettingsClass>.Instance.Graphics.Settings.SuperSamplingFactor;
            float configSuperSamplingFactor = ChangeSuperSamplingForOpticConfig.SuperSampling.Value;

            if (configSuperSamplingFactor < defaultSuperSamplingFactor)
            {
                SetSuperSampling(1f - configSuperSamplingFactor);
            }
        }

        /*
        private static void SetDLSSAim()
        {
            EDLSSMode defaultDLSSMode = Singleton<SharedGameSettingsClass>.Instance.Graphics.Settings.DLSSMode;
            EDLSSMode configDLSSMode = ChangeSuperSamplingForOpticConfig.DLSSMode.Value;

            if (configDLSSMode != defaultDLSSMode)
            {
                SetDLSS(configDLSSMode);
            }
        }
        */

        private static void SetFSRAim()
        {
            EFSRMode defaultFSRMode = Singleton<SharedGameSettingsClass>.Instance.Graphics.Settings.FSRMode;
            EFSRMode configFSRMode = ChangeSuperSamplingForOpticConfig.FSRMode.Value;

            if (configFSRMode != defaultFSRMode)
            {
                SetFSR(configFSRMode);
            }
        }

        private static void SetFSR2Aim()
        {
            EFSR2Mode defaultFSR2Mode = Singleton<SharedGameSettingsClass>.Instance.Graphics.Settings.FSR2Mode;
            EFSR2Mode configFSR2Mode = ChangeSuperSamplingForOpticConfig.FSR2Mode.Value;

            if (configFSR2Mode != defaultFSR2Mode)
            {
                SetFSR2(configFSR2Mode);
            }
        }


        /*
 private static void ExampleSetDefault()
 {
     //float defaultSuperSamplingFactor = Singleton<SharedGameSettingsClass>.Instance.Graphics.Settings.SuperSamplingFactor;
     //EDLSSMode defaultDLSSMode = Singleton<SharedGameSettingsClass>.Instance.Graphics.Settings.DLSSMode;
     //EDLSSMode configDLSSMode = ChangeSuperSamplingForOpticConfig.DLSSMode.Value;
     //EFSRMode defaultFSRMode = Singleton<SharedGameSettingsClass>.Instance.Graphics.Settings.FSRMode;
     //EFSRMode configFSRMode = ChangeSuperSamplingForOpticConfig.FSRMode.Value;
     bool FSR2Enabled = Singleton<SharedGameSettingsClass>.Instance.Graphics.Settings.FSR2Enabled;
     EFSR2Mode defaultFSR2Mode = Singleton<SharedGameSettingsClass>.Instance.Graphics.Settings.FSR2Mode;
     //EFSR2Mode configFSR2Mode = ChangeSuperSamplingForOpticConfig.FSR2Mode.Value;

     if (!FSR2Enabled)
     {
         SetSuperSampling(defaultSuperSamplingFactor);
     }


     if (defaultDLSSMode == configDLSSMode)
     {
         SetDLSSMode(defaultDLSSMode);
     }
     else
     {
         return;
     }

     if (defaultFSRMode == configFSRMode)
     {
         SetFSRMode(defaultFSRMode);
     }
     else
     {
         return;
     }


     if (FSR2Enabled && (defaultFSR2Mode == configFSR2Mode))
     {
         SetFSR2(defaultFSR2Mode);
     }

 }
 */

        private static void SetSuperSamplingDefault()
        {
            float defaultSuperSamplingFactor = Singleton<SharedGameSettingsClass>.Instance.Graphics.Settings.SuperSamplingFactor;
            SetSuperSampling(defaultSuperSamplingFactor);
        }

        /*
        private static void SetDLSSDefault()
        {
            EDLSSMode defaultDLSSMode = Singleton<SharedGameSettingsClass>.Instance.Graphics.Settings.DLSSMode;
            SetDLSS(defaultDLSSMode);
        }
        */

        private static void SetFSRDefault()
        {
            EFSRMode defaultFSRMode = Singleton<SharedGameSettingsClass>.Instance.Graphics.Settings.FSRMode;
            SetFSR(defaultFSRMode);
        }

        private static void SetFSR2Default()
        {
            EFSR2Mode defaultFSR2Mode = Singleton<SharedGameSettingsClass>.Instance.Graphics.Settings.FSR2Mode;
            SetFSR2(defaultFSR2Mode);
        }

        private static void SetSuperSampling(float value)
        {
            CameraClass camera = ChangeSuperSamplingForOptic.getCameraInstance();
            if (camera != null)
            {
                camera.SetSuperSampling(Mathf.Clamp(value, 0.01f, 1f));
            }
        }

        /*
        private static void SetDLSS(EDLSSMode value)
        {
            CameraClass camera = ChangeSuperSamplingForOptic.getCameraInstance();
            if (camera != null)
            {
                camera.GetDLSSQuality(value);
            }
        }
        */

        private static void SetFSR(EFSRMode value)
        {
            CameraClass camera = ChangeSuperSamplingForOptic.getCameraInstance();
            if (camera != null)
            {
                camera.SetFSR(value);
            }
        }

        private static void SetFSR2(EFSR2Mode value)
        {
            CameraClass camera = ChangeSuperSamplingForOptic.getCameraInstance();
            if (camera != null)
            {
                camera.SetFSR2(value);
            }
        }

        public class OpticSightOnEnablePath : ModulePatch
        {
            protected override MethodBase GetTargetMethod()
            {
                return typeof(OpticSight).GetMethod("OnEnable", BindingFlags.Instance | BindingFlags.NonPublic);
            }

            [PatchPostfix]
            private static void PatchPostfix()
            {
                bool DLSSEnabled = Singleton<SharedGameSettingsClass>.Instance.Graphics.Settings.DLSSEnabled;
                bool FSREnabled = Singleton<SharedGameSettingsClass>.Instance.Graphics.Settings.FSREnabled;
                bool FSR2Enabled = Singleton<SharedGameSettingsClass>.Instance.Graphics.Settings.FSR2Enabled;

                if (!ChangeSuperSamplingForOpticConfig.EnableMod.Value)
                {
                    return;
                }
                Player localPlayer = ChangeSuperSamplingForOptic.getPlayetInstance();
                if(localPlayer != null && localPlayer.ProceduralWeaponAnimation != null && localPlayer.ProceduralWeaponAnimation.IsAiming && localPlayer.ProceduralWeaponAnimation.CurrentAimingMod != null && localPlayer.ProceduralWeaponAnimation.CurrentScope != null)
                {
                    if (localPlayer.ProceduralWeaponAnimation.CurrentScope.IsOptic)
                    {
                        if (!DLSSEnabled && !FSREnabled && !FSR2Enabled)
                        {
                            SetSuperSamplingAim();
                        }
                        //else if (DLSSEnabled)
                        //{
                        //    SetDLSSAim();
                        //}
                        else if (FSREnabled)
                        {
                            SetFSRAim();
                        }
                        else if (FSR2Enabled)
                        {
                            SetFSR2Aim();
                        }
                    }
                }
            }
        }
        
        public class OpticSightOnDisablePath : ModulePatch
        {
            protected override MethodBase GetTargetMethod()
            {
                return typeof(OpticSight).GetMethod("OnDisable", BindingFlags.Instance | BindingFlags.NonPublic);
            }

            [PatchPostfix]
            private static void PatchPostfix()
            {
                bool DLSSEnabled = Singleton<SharedGameSettingsClass>.Instance.Graphics.Settings.DLSSEnabled;
                bool FSREnabled = Singleton<SharedGameSettingsClass>.Instance.Graphics.Settings.FSREnabled;
                bool FSR2Enabled = Singleton<SharedGameSettingsClass>.Instance.Graphics.Settings.FSR2Enabled;

                Player localPlayer = ChangeSuperSamplingForOptic.getPlayetInstance();

                if (localPlayer != null && localPlayer.ProceduralWeaponAnimation != null)
                {
                    if (!DLSSEnabled && !FSREnabled && !FSR2Enabled)
                    {
                        SetSuperSamplingDefault();
                    }
                    //else if (DLSSEnabled)
                    //{
                    //    SetDLSSDefault();
                    //}
                    else if (FSREnabled)
                    {
                        SetFSRDefault();
                    }
                    else if (FSR2Enabled)
                    {
                        SetFSR2Default();
                    }
                }
            }
        }
        
        public class ClientFirearmControllerChangeAimingModePath : ModulePatch
        {
            protected override MethodBase GetTargetMethod()
            {
                return typeof(Player.FirearmController).GetMethod("ChangeAimingMode", BindingFlags.Instance | BindingFlags.Public);
            }

            [PatchPostfix]
            private static void PatchPostfix()
            {
                bool DLSSEnabled = Singleton<SharedGameSettingsClass>.Instance.Graphics.Settings.DLSSEnabled;
                bool FSREnabled = Singleton<SharedGameSettingsClass>.Instance.Graphics.Settings.FSREnabled;
                bool FSR2Enabled = Singleton<SharedGameSettingsClass>.Instance.Graphics.Settings.FSR2Enabled;

                Player localPlayer = ChangeSuperSamplingForOptic.getPlayetInstance();
                
                if(localPlayer != null && localPlayer.ProceduralWeaponAnimation != null && localPlayer.ProceduralWeaponAnimation.IsAiming && localPlayer.ProceduralWeaponAnimation.CurrentAimingMod != null && localPlayer.ProceduralWeaponAnimation.CurrentScope != null)
                {
                    if (localPlayer.ProceduralWeaponAnimation.CurrentScope.IsOptic)
                    {
                        if (!DLSSEnabled && !FSREnabled && !FSR2Enabled)
                        {
                            SetSuperSamplingAim();
                        }
                        //else if (DLSSEnabled)
                        //{
                        //    SetDLSSAim();
                        //}
                        else if (FSREnabled)
                        {
                            SetFSRAim();
                        }
                        else if (FSR2Enabled)
                        {
                            SetFSR2Aim();
                        }
                    }
                    else
                    {
                        if (!DLSSEnabled && !FSREnabled && !FSR2Enabled)
                        {
                            SetSuperSamplingDefault();
                        }
                        //else if (DLSSEnabled)
                        //{
                        //    SetDLSSDefault();
                        //}
                        else if (FSREnabled)
                        {
                            SetFSRDefault();
                        }
                        else if (FSR2Enabled)
                        {
                            SetFSR2Default();
                        }
                    }
                }
            }
        }
    }
}
