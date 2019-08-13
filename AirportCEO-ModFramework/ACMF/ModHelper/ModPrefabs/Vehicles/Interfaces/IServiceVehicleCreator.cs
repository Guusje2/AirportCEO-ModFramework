using UnityEngine;

namespace ACMF.ModHelper.ModPrefabs.Vehicles.Interfaces
{
    internal interface IServiceVehicleCreator
    {
        GameObject CreateNewInstance();
        GameObject CreateForDeserialization(VehicleModel vehicleModel);
    }
}
