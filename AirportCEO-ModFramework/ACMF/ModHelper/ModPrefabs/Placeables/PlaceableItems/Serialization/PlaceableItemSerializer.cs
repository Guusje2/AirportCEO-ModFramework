using ACMF.ModHelper.ModPrefabs.Placeables.PlaceableItems.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace ACMF.ModHelper.ModPrefabs.Placeables.PlaceableItems.Serialization
{
    internal static class PlaceableItemSerializer
    {
        private static readonly string MODDED_PLACEABLE_ITEM_SAVE_FILE_NAME = "/ModdedPlaceableItemData.json";

        internal static void SerializePlaceableItemsUsingCustomSerializationSystem(string savePath, List<PlaceableItem> itemsUsingCustomSerializationSystem)
        {
            PlaceableItemSerializationWrapper placeableItemSerializationWrapper = new PlaceableItemSerializationWrapper();
            foreach (PlaceableItem item in itemsUsingCustomSerializationSystem)
            {
                if (ActivePlaceableItemCreators.HasCreatorFromEnum(item.itemType) == false)
                    continue;

                Utilities.Logger.Print($"Modded Placeable Item Serializing {item}...");
                IACMFPlaceableItemCustomSerializationSystem serializationSystem = ActivePlaceableItemCreators.GetCreatorFromEnum(item.itemType) as IACMFPlaceableItemCustomSerializationSystem;

                PlaceableItemSerializable placeableItemSerializable = serializationSystem.SerializeItem(item);
                if (placeableItemSerializable == null) {
                    Utilities.Logger.Error($"{item} SerializeItem returned null.");
                    ShowErrorDialog();
                }
                else
                    placeableItemSerializationWrapper.Add(placeableItemSerializable);
            }

            bool result = Utilities.JsonSerialization.Serialize(placeableItemSerializationWrapper, savePath + MODDED_PLACEABLE_ITEM_SAVE_FILE_NAME);
            if (result == false)
                ShowErrorDialog();

            Utilities.Logger.Print($"Modded Placeable Items Serialization successful.");
        }

        internal static void DeserializePlaceableItemsUsingCustomSerializationSystem(string savePath)
        {
            if (File.Exists(savePath + MODDED_PLACEABLE_ITEM_SAVE_FILE_NAME) == false)
                return;

            bool deserializeResult = Utilities.JsonSerialization.Deserialize(out PlaceableItemSerializationWrapper placeableItemSerializationWrapper, savePath + MODDED_PLACEABLE_ITEM_SAVE_FILE_NAME);
            if (deserializeResult == false)
                DialogPopup.DialogManager.QueueMessagePanel($"A error occured while trying to deserialize modded vehicles. Please check the output log to see why.");

            foreach(PlaceableItemSerializable pis in placeableItemSerializationWrapper.GetPlaceableItems())
            {
                if (ActivePlaceableItemCreators.HasCreatorFromEnum(pis.itemType) == false)
                    continue;

                IACMFPlaceableItemCustomSerializationSystem serializationSystem = ActivePlaceableItemCreators.GetCreatorFromEnum(pis.itemType) as IACMFPlaceableItemCustomSerializationSystem;
                bool result = serializationSystem.DeserializeItem(pis);
                if (result == false)
                {
                    Utilities.Logger.Error($"{pis} || {serializationSystem} DeserializeItem returned false.");
                    ShowErrorDialog();
                }
            }

            Utilities.Logger.Print($"Modded Placeable Items Deserialization successful.");
        }

        private static void ShowErrorDialog() =>
            DialogPopup.DialogManager.QueueMessagePanel($"A error occured while trying to serialize placeable items. Please check the output log to see why.");
    }
}
