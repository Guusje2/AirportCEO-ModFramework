using ACMF.ModHelper.ModPrefabs.Placeables.PlaceableObjects.Interfaces;

namespace ACMF.ModHelper.ModPrefabs.Placeables.PlaceableStructures.Interfaces
{
    public interface IACMFPlaceableStructure : IACMFPlaceableObject
    {
        Enums.StructureType StructureTypeEnum { get; }

        bool ICanBeBuiltBelowGround { get; }
        bool ICannotBeBuiltBelowTerminal { get; }
    }
}
