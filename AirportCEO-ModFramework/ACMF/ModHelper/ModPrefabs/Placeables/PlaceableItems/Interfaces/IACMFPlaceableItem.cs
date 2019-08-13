using ACMF.ModHelper.ModPrefabs.Placeables.PlaceableObjects.Interfaces;

namespace ACMF.ModHelper.ModPrefabs.Placeables.PlaceableItems.Interfaces
{
    public interface IACMFPlaceableItem : IACMFPlaceableObject
    {
        Enums.ItemType ItemTypeEnum { get; }

        Enums.ItemPlacementArea IItemPlacementArea { get; }
        Enums.GenericZoneType[] IAllowedGenericZones { get; }
        Enums.SpecificZoneType[] IAllowedSpecificZones { get; }
        Enums.RoomType[] IAllowedRooms { get; }
        bool IMustBeWithinGenericZone { get; }
        bool IMustBeWithinRoom { get; }
        bool IMustBeWithinSpecificZone { get; }
    }
}
