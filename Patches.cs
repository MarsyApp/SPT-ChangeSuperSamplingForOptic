using System.Collections.Generic;
using System.Reflection;
using Aki.Reflection.Patching;
using ChangeSuperSamplingForOptic.Configs;
using Comfort.Common;
using EFT;
using EFT.CameraControl;
using UnityEngine;

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
        
        private static void SetSuperSamplingDefault()
        {
            float defaultSuperSamplingFactor = Singleton<SharedGameSettingsClass>.Instance.Graphics.Settings.SuperSamplingFactor;
            SetSuperSampling(defaultSuperSamplingFactor);
        }
        
        private static void SetSuperSampling(float value)
        {
            CameraClass camera = ChangeSuperSamplingForOptic.getCameraInstance();
            if (camera != null)
            {
                camera.SetSuperSampling(Mathf.Clamp(value, 0.01f, 1f));
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
                if (!ChangeSuperSamplingForOpticConfig.EnableMod.Value)
                {
                    return;
                }
                Player localPlayer = ChangeSuperSamplingForOptic.getPlayetInstance();
                if(localPlayer != null && localPlayer.ProceduralWeaponAnimation != null && localPlayer.ProceduralWeaponAnimation.IsAiming && localPlayer.ProceduralWeaponAnimation.CurrentAimingMod != null && localPlayer.ProceduralWeaponAnimation.CurrentScope != null)
                {
                    if (localPlayer.ProceduralWeaponAnimation.CurrentScope.IsOptic)
                    {
                        SetSuperSamplingAim();
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
                Player localPlayer = ChangeSuperSamplingForOptic.getPlayetInstance();
                if(localPlayer != null && localPlayer.ProceduralWeaponAnimation != null)
                {
                    SetSuperSamplingDefault();
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
                Player localPlayer = ChangeSuperSamplingForOptic.getPlayetInstance();
                if(localPlayer != null && localPlayer.ProceduralWeaponAnimation != null && localPlayer.ProceduralWeaponAnimation.IsAiming && localPlayer.ProceduralWeaponAnimation.CurrentAimingMod != null && localPlayer.ProceduralWeaponAnimation.CurrentScope != null)
                {
                    if (localPlayer.ProceduralWeaponAnimation.CurrentScope.IsOptic)
                    {
                        SetSuperSamplingAim();
                    }
                    else
                    {
                        SetSuperSamplingDefault();
                    }
                }
            }
        }
    }
}
