using ACMF.ModHelper.ModPrefabs.Placeables.PlaceableStructures.Interfaces;
using Harmony;
using System;
using UnityEngine;

namespace ACMF.ModHelper.ModPrefabs.Placeables.PlaceableStructures.Patches
{
    [HarmonyPatch(typeof(BuildingController))]
    [HarmonyPatch("GetStructureGameObject")]
    [HarmonyPatch(new Type[] { typeof(Enums.StructureType) })]
    public class BuildingControllerGetStructureGameObjectPatcher
    {
        [HarmonyPrefix]
        public static bool Prefix(ref GameObject __result, Enums.StructureType structureType)
        {
            IPlaceableStructureCreator placeableStructureCreator = ActivePlaceableStructureCreators.GetCreatorFromEnum(structureType);
            if (placeableStructureCreator != null)
            {
                __result = placeableStructureCreator.Prefab;
                return false;
            }

            return true;
        }
    }
}
