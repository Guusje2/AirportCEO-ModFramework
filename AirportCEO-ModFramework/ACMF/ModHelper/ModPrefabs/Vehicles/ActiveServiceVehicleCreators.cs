using ACMF.ModHelper.ModPrefabs.Vehicles.Interfaces;
using System;
using System.Collections.Generic;

namespace ACMF.ModHelper.ModPrefabs.Vehicles
{
    public static class ActiveServiceVehicleCreators
    {
        private static readonly Dictionary<Type, IServiceVehicleCreator> VehicleCreators = new Dictionary<Type, IServiceVehicleCreator>();

        public static ServiceVehicleCreator<T> GetCreator<T>() where T : ServiceVehicleController
        {
            if (HasCreatorFromType(typeof(T)) && VehicleCreators[typeof(T)] is ServiceVehicleCreator<T> result)
                return result;

            Utilities.Logger.Error($"ServiceVehicleCreator attempted to get creator from controller for {typeof(T)} which does not exist.");
            return null;
        }

        internal static void Add<T>(IServiceVehicleCreator o) where T : ServiceVehicleController
        {
            VehicleCreators.Add(typeof(T), o);
        }

        internal static IServiceVehicleCreator GetCreatorFromType(Type type)
        {
            return HasCreatorFromType(type) ? VehicleCreators[type] : null;
        }

        internal static bool HasCreatorFromType(Type type)
        {
            return VehicleCreators.ContainsKey(type);
        }
    }
}
