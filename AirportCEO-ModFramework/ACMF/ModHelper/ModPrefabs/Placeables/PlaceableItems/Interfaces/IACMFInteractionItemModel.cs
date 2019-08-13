using System.Collections.Generic;

namespace ACMF.ModHelper.ModPrefabs.Placeables.PlaceableItems.Interfaces
{
    public interface IACMFInteractionItemModel : IACMFPlaceableItem
    {
        Enums.InteractionItemGroup IInteractionItemGroup { get; }
        List<InteractionOccupancyData> IInteractionPoints { get; }
    }
}
