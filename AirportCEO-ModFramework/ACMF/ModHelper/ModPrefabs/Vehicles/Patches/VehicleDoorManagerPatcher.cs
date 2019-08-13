using Harmony;

namespace ACMF.ModHelper.ModPrefabs.Vehicles.Patches
{
    [HarmonyPatch(typeof(VehicleDoorManager))]
    [HarmonyPatch("Awake")]
    internal class VehicleDoorManagerAwakePatcher
    {
        [HarmonyPrefix]
        public static bool Prefix(VehicleDoorManager __instance)
        {
            if (__instance.frontDoorPoints == null || __instance.rearDoorPoints == null)
            {
                return false;
            }

            return true;
        }
    }
}
