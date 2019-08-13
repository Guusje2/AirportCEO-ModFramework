using ACMF.ModHelper.ModPrefabs.Placeables.PlaceableItems;
using ACMF.ModHelper.ModPrefabs.Placeables.PlaceableStructures.Interfaces;
using System;
using System.Collections.Generic;

namespace ACMF.ModHelper.ModPrefabs.Placeables.PlaceableStructures
{
    public static class ActivePlaceableStructureCreators
    {
        private static readonly Dictionary<Type, IPlaceableStructureCreator> PlaceableStructureCreators = new Dictionary<Type, IPlaceableStructureCreator>();
        private static readonly Dictionary<Enums.StructureType, IPlaceableStructureCreator> ActiveEnums = new Dictionary<Enums.StructureType, IPlaceableStructureCreator>();

        public static PlaceableStructureCreator<T> GetCreator<T>() where T : PlaceableStructureCreator<T>
        {
            if (HasCreatorFromType(typeof(T)) && PlaceableStructureCreators[typeof(T)] is PlaceableStructureCreator<T> result)
                return result;

            Utilities.Logger.Error($"ActivePlaceableStructureCreators attempted to get creator from controller for {typeof(T)} which does not exist.");
            return null;
        }

        internal static void Add<T>(IPlaceableStructureCreator o, Enums.StructureType structureType) where T : IPlaceableStructureCreator
        {
            PlaceableStructureCreators.Add(typeof(T), o);
            ActiveEnums.Add(structureType, o);
        }

        internal static IPlaceableStructureCreator GetCreatorFromType(Type type)
        {
            return HasCreatorFromType(type) ? PlaceableStructureCreators[type] : null;
        }

        internal static bool HasCreatorFromType(Type type)
        {
            return PlaceableStructureCreators.ContainsKey(type);
        }

        internal static bool HasCreatorFromEnum(Enums.StructureType structureType)
        {
            return ActiveEnums.ContainsKey(structureType);
        }

        internal static IPlaceableStructureCreator GetCreatorFromEnum(Enums.StructureType structureType)
        {
            return HasCreatorFromEnum(structureType) ? ActiveEnums[structureType] : null;
        }
    }
}
