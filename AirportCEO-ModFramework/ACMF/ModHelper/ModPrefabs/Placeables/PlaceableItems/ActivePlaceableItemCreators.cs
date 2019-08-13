using ACMF.ModHelper.ModPrefabs.Placeables.PlaceableItems.Interfaces;
using System;
using System.Collections.Generic;

namespace ACMF.ModHelper.ModPrefabs.Placeables.PlaceableItems
{
    public static class ActivePlaceableItemCreators
    {
        private static readonly Dictionary<Type, IPlaceableItemCreator> PlaceableItemCreators = new Dictionary<Type, IPlaceableItemCreator>();
        private static readonly Dictionary<Enums.ItemType, IPlaceableItemCreator> ActiveEnums = new Dictionary<Enums.ItemType, IPlaceableItemCreator>(); 

        public static PlaceableItemCreator<T> GetCreator<T>() where T : PlaceableItemCreator<T>
        {
            if (HasCreatorFromType(typeof(T)) && PlaceableItemCreators[typeof(T)] is PlaceableItemCreator<T> result)
                return result;

            Utilities.Logger.Error($"ActivePlaceableItemCreators attempted to get creator from controller for {typeof(T)} which does not exist.");
            return null;
        }

        internal static void Add<T>(IPlaceableItemCreator o, Enums.ItemType itemType) where T : IPlaceableItemCreator
        {
            PlaceableItemCreators.Add(typeof(T), o);
            ActiveEnums.Add(itemType, o);
        }

        internal static IPlaceableItemCreator GetCreatorFromType(Type type)
        {
            return HasCreatorFromType(type) ? PlaceableItemCreators[type] : null;
        }

        internal static bool HasCreatorFromType(Type type)
        {
            return PlaceableItemCreators.ContainsKey(type);
        }

        internal static bool HasCreatorFromEnum(Enums.ItemType itemType)
        {
            return ActiveEnums.ContainsKey(itemType);
        }

        internal static IPlaceableItemCreator GetCreatorFromEnum(Enums.ItemType itemType)
        {
            return HasCreatorFromEnum(itemType) ? ActiveEnums[itemType] : null;
        }
    }
}
