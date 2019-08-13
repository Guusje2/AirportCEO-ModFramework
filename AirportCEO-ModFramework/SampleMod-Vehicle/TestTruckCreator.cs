using System;
using ACMF.ModHelper.ModPrefabs.Vehicles;
using UnityEngine;

namespace SampleModVehicle
{
    public class TestTruckCreator : ServiceVehicleCreator<TestTruckController>
    {
        protected override GameObject VehicleGameObject { get; } = Assets.Instance.AttemptLoadGameObject("TestTruck");
        protected override string[] FrontDoorTransforms => new string[2] { "Doors/FrontDoor1", "Doors/FrontDoor2" };
        protected override string[] RearDoorTransforms => new string[0];
        protected override string[] CargoDoorTransforms => new string[0];
        protected override string[] ColorableSpriteRenderers => new string[1] { "Sprite/Chassie" };

        protected override void PostCreateInstance(GameObject vehicle) { }
        protected override void PostCreateForDeserialization(GameObject vehicle) { }

        public static GameObject SpawnRegular()
        {
            GameObject vehicle = ActiveServiceVehicleCreators.GetCreator<TestTruckController>().CreateNewInstance();
            TestTruckController controller = vehicle.GetComponent<TestTruckController>();
            controller.Initialize();
            controller.ServiceVehicleModel.isOwnedByAirport = true;
            TrafficController.Instance.AddVehicleToSpawnQueue(controller, false);

            return vehicle;
        }
    }
}