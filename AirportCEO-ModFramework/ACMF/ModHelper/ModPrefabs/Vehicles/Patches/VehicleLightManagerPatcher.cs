using Harmony;
using UnityEngine;

namespace ACMF.ModHelper.ModPrefabs.Vehicles.Patches
{
    [HarmonyPatch(typeof(VehicleLightManager))]
    [HarmonyPatch("Awake")]
    internal class VehicleLightManagerAwakePatcher
    {
        [HarmonyPrefix]
        public static bool Prefix(VehicleLightManager __instance)
        {
            if (__instance.specialWarningLights == null)
                __instance.specialWarningLights = new Light[0];

            return true;
        }
    }
}
