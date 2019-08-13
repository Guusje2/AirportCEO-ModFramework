namespace ACMF.ModHelper.ModPrefabs.Placeables.PlaceableItems.Interfaces
{
    public interface IACMFPlaceableItemCustomSerializationSystem
    {
        PlaceableItemSerializable SerializeItem(PlaceableItem placeableItem);
        bool DeserializeItem(PlaceableItemSerializable placeableItemSerializable);
    }
}
