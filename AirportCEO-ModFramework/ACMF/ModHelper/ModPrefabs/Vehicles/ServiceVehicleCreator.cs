using ACMF.ModHelper.ModPrefabs.Vehicles.Interfaces;
using ACMF.ModHelper.PatchTime;
using ACMF.ModHelper.PatchTime.MethodAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace ACMF.ModHelper.ModPrefabs.Vehicles
{
    public abstract class ServiceVehicleCreator<T> : IServiceVehicleCreator where T : ServiceVehicleController
    {
        protected abstract GameObject VehicleGameObject { get; }
        protected abstract string[] FrontDoorTransforms { get; }
        protected abstract string[] RearDoorTransforms { get; }
        protected abstract string[] CargoDoorTransforms { get; }
        protected float ShadowDistance { get; } = 0.175f;
        protected abstract string[] ColorableSpriteRenderers { get; }

        [PatchTimeMethod]
        public void Patch()
        {
            Utilities.Logger.Print($"Registered ServiceVehicleCreator of Controller {typeof(T).Name}");
            ActiveServiceVehicleCreators.Add<T>(this);
        }

        public virtual GameObject CreateNewInstance()
        {
            if (VehicleGameObject == null)
            {
                Utilities.Logger.Error($"{GetType().Name} Tried to Instantiate a null VehicleGameObject");
                return null;
            }

            GameObject vehicle = Object.Instantiate(VehicleGameObject);
            vehicle.transform.SetParent(FolderController.Instance.GetSceneRootTransform(), false);
            AddVehicleDoorManager(vehicle);
            AddVehicleLightManager(vehicle);
            AddBoundaryHandler(vehicle);
            AddShadowHandler(vehicle);
            AddVehicleAudio(vehicle);
            AddServiceVehicleController(vehicle);

            PostCreateInstance(vehicle);
            return vehicle;
        }

        protected virtual void AddVehicleDoorManager(GameObject vehicle)
        {
            VehicleDoorManager vdm = vehicle.transform.Find("Doors").gameObject.AddComponent<VehicleDoorManager>();
            vdm.frontDoorPoints = new List<Transform>();
            vdm.rearDoorPoints = new List<Transform>();
            vdm.cargoDoorPoints = new List<Transform>();
            vdm.transformsToHide = new List<Transform>();

            foreach (string s in FrontDoorTransforms)
                vdm.frontDoorPoints.Add(vehicle.transform.Find(s));

            foreach (string s in RearDoorTransforms)
                vdm.rearDoorPoints.Add(vehicle.transform.Find(s));

            foreach (string s in CargoDoorTransforms)
                vdm.cargoDoorPoints.Add(vehicle.transform.Find(s));

            // We call Awake() again as we patch into Awake and dont allow it to run if frontDoorPoints or rearDoorPoints is null.
            // This will only affect the VDM we just added above as all other VDM are added in the unity editor so will always be
            // an array of at least size 0.
            vdm.Awake();
        }

        protected virtual void AddVehicleLightManager(GameObject vehicle)
        {
            VehicleLightManager vlm = vehicle.transform.Find("Lights").gameObject.AddComponent<VehicleLightManager>();
        }

        protected virtual void AddBoundaryHandler(GameObject vehicle)
        {
            Transform boundary = vehicle.transform.Find("Boundary");
            Transform startPos = boundary.Find("StartPos");
            Transform endPos = boundary.Find("EndPos");

            BoundaryHandler bh = boundary.gameObject.AddComponent<BoundaryHandler>();
            bh.penalty = 0;
            bh.border = new Boundary(BoundaryHandler.BoundaryType.Border, startPos, endPos, boundary.gameObject);
        }

        protected virtual void AddShadowHandler(GameObject vehicle)
        {
            ShadowHandler shadowHandler = vehicle.transform.Find("Sprite/Shadow").gameObject.AddComponent<ShadowHandler>();
            shadowHandler.shadowDistance = ShadowDistance;
        }

        protected virtual void AddVehicleAudio(GameObject vehicle)
        {
            VehicleAudioManager vehicleAudio = vehicle.transform.Find("Audio").gameObject.AddComponent<VehicleAudioManager>();
        }

        protected virtual void AddServiceVehicleController(GameObject vehicle)
        {
            T serviceVehicleController = vehicle.AddComponent<T>();

            SpriteRenderer[] colorableSpriteRenderers = new SpriteRenderer[ColorableSpriteRenderers.Length];
            for (int i = 0; i < ColorableSpriteRenderers.Length; i++)
                colorableSpriteRenderers[i] = vehicle.transform.Find(ColorableSpriteRenderers[i]).gameObject.GetComponent<SpriteRenderer>();
            serviceVehicleController.colorableParts = colorableSpriteRenderers;

            serviceVehicleController.cargoDoors = new Transform[0];
            serviceVehicleController.doorManager = vehicle.GetComponentInChildren<VehicleDoorManager>();
            serviceVehicleController.lightManager = vehicle.GetComponentInChildren<VehicleLightManager>();
            serviceVehicleController.audioManager = vehicle.GetComponentInChildren<VehicleAudioManager>();
            serviceVehicleController.exhaust = vehicle.GetComponentInChildren<ParticleSystem>();
            serviceVehicleController.shadows = vehicle.GetComponentsInChildren<ShadowHandler>();
            serviceVehicleController.boundary = vehicle.GetComponentInChildren<BoundaryHandler>();
            serviceVehicleController.thoughtsReferenceList = new List<Thought>();
            serviceVehicleController.currentShipment = new Shipment("", Vector3.zero, Enums.DeliveryContainerType.Unspecified, Enums.DeliveryContentType.Unspecified, 0, "");
            serviceVehicleController.gameObject.SetActive(false);
            serviceVehicleController.transform.position = Vector3.zero;
        }

        protected abstract void PostCreateInstance(GameObject vehicle);

        public virtual GameObject CreateForDeserialization(VehicleModel vehicleModel)
        {
            GameObject vehicle = CreateNewInstance();
            T controller = vehicle.GetComponent<T>();
            controller.Initialize();
            controller.RestoreVehicleFromSerialization(vehicleModel);
            TrafficController.Instance.AddVehicleToList(controller);
            controller.Launch();

            PostCreateForDeserialization(vehicle);
            return vehicle;
        }

        protected abstract void PostCreateForDeserialization(GameObject vehicle);
    }
}