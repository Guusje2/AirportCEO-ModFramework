using Harmony;
using System.Collections.Generic;

namespace ACMF.ModHelper.ModPrefabs.Floors.Patches
{
    [HarmonyPatch(typeof(PlaceableFloor))]
    [HarmonyPatch("GetVariations")]
    public class PlaceableFloorGetVariationsPatcher
    {
        [HarmonyPostfix]
        public static void Postfix(ref Variation[] __result)
        {
            List<Variation> resultsList = new List<Variation>(__result);
            __result = resultsList.ToArray();
        }
    }
}
