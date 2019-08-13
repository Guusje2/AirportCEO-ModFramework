using ACMF.ModHelper.ModPrefabs.Placeables.PlaceableItems.Interfaces;
using Harmony;
using System;
using UnityEngine;

namespace ACMF.ModHelper.ModPrefabs.Placeables.PlaceableItems.Patches
{
    [HarmonyPatch(typeof(BuildingController))]
    [HarmonyPatch("GetItemGameObject")]
    [HarmonyPatch(new Type[] { typeof(Enums.ItemType) })]
    public class BuildingControllerGetItemGameObjectPatcher
    {
        [HarmonyPrefix]
        public static bool Prefix(ref GameObject __result, Enums.ItemType itemType)
        {
            IPlaceableItemCreator placeableItemCreator = ActivePlaceableItemCreators.GetCreatorFromEnum(itemType);
            if (placeableItemCreator != null)
            {
                __result = placeableItemCreator.Prefab;
                return false;
            }

            return true;
        }
    }
}
