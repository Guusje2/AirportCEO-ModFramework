using OdinSerializer;
using System;
using System.Collections.Generic;

namespace ACMF.ModHelper.ModPrefabs.Placeables.PlaceableItems.Serialization
{
    internal class PlaceableItemSerializationWrapper
    {
        [OdinSerialize]
        private readonly List<PlaceableItemSerializable> PlaceableItems = new List<PlaceableItemSerializable>();

        internal void Add(PlaceableItem placeableItem)
        {
            PlaceableItemSerializable placeableItemSerializable = new PlaceableItemSerializable();
            placeableItemSerializable.SetObjectForSerializer(placeableItem);
            PlaceableItems.Add(placeableItemSerializable);
        }

        internal void Add(PlaceableItemSerializable placeableItemSerializable)
        {
            PlaceableItems.Add(placeableItemSerializable);
        }

        internal IEnumerable<PlaceableItemSerializable> GetPlaceableItems()
        {
            return PlaceableItems;
        }
    }
}
