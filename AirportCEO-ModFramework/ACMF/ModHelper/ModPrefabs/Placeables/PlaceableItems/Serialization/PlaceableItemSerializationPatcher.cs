using ACMF.ModHelper.ModPrefabs.Placeables.PlaceableItems.Interfaces;
using Harmony;
using Serializers;
using System;
using System.Collections.Generic;

/**
 * Patching for placeableitems
 * We disallow StartItemSeralizer from running
 * Because it refers to Singleton<BuildingController>.Instance.allItemsArray
 * And that will include some of our custom objects
 * So we go through that list and remove all "custom" modded objects in the list and place them in a new list
 * Which we deal with ourself.
 */
namespace ACMF.ModHelper.ModPrefabs.Placeables.PlaceableItems.Serialization
{
    public static class PlaceableItemSerializationPatcher
    {
        [HarmonyPatch(typeof(ItemSerializer))]
        [HarmonyPatch("StartItemSeralizer")]
        public static class PlaceableItemSerializerPatcher
        {
            [HarmonyPrefix]
            public static bool Prefix(bool _debugPrints, string _savePath)
            {
                ItemSerializer.savePath = _savePath;
                ItemSerializer.debugPrints = true;
                List<PlaceableItem> originalItemList = new List<PlaceableItem>(Singleton<BuildingController>.Instance.allItemsArray.array);
                List<PlaceableItem> itemsUsingDefaultSerializationSystem = new List<PlaceableItem>();
                List<PlaceableItem> itemsUsingCustomSerializationSystem = new List<PlaceableItem>();

                foreach (PlaceableItem pl in originalItemList)
                {
                    if (pl == null)
                        continue;

                    bool isModded = ActivePlaceableItemCreators.HasCreatorFromEnum(pl.itemType);
                    bool usesCustomSerializer = ActivePlaceableItemCreators.GetCreatorFromEnum(pl.itemType) is IACMFPlaceableItemCustomSerializationSystem;

                    if (isModded && usesCustomSerializer)
                        itemsUsingCustomSerializationSystem.Add(pl);
                    else
                        itemsUsingDefaultSerializationSystem.Add(pl);
                }

                DynamicArray<PlaceableItem> itemsToUseDefaultSerialisationBehaviour = new DynamicArray<PlaceableItem>(256);
                itemsToUseDefaultSerialisationBehaviour.AddRange(itemsUsingDefaultSerializationSystem.ToArray());
                ItemSerializer.SerializeItems(itemsToUseDefaultSerialisationBehaviour);
                PlaceableItemSerializer.SerializePlaceableItemsUsingCustomSerializationSystem(ItemSerializer.savePath, itemsUsingCustomSerializationSystem);
                return false;
            }
        }

        [HarmonyPatch(typeof(ItemSerializer))]
        [HarmonyPatch("StartItemDeserializer")]
        public static class PlaceableItemDeserializerPatcher
        {
            [HarmonyPostfix]
            public static void Postfix(bool _debugPrints, string _savePath)
            {
                PlaceableItemSerializer.DeserializePlaceableItemsUsingCustomSerializationSystem(_savePath);
            }
        }
    }
}
