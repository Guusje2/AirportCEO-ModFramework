using ACMF.ModHelper.EnumPatcher;
using ACMF.ModHelper.PatchTime;
using ACMF.ModHelper.PatchTime.MethodAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace ACMF.ModHelper.ModPrefabs.Vehicles
{
    public abstract class ACMFServiceVehicleModel : ServiceVehicleModel
    {
        public abstract Enums.VehicleType VehicleTypeEnum { get; }
        public abstract string VehicleName { get; }
        public abstract Vector2[] VehicleHitboxes { get; }
        public abstract Enums.FuelType FuelType { get; }
        public abstract float TopSpeed { get; }
        public abstract float OperatingCost { get; }
        public abstract bool RandomizeColor { get; }
        public abstract float MaxQuantityLoadedGoods { get; }
        public abstract List<Enums.DeliveryContainerType> AllowedDeliveryContainers { get; }
        public abstract float AcceptedTargetDistance { get; }
        public abstract float AcceptedTargetDistanceTurn { get; }
        public abstract int NodesToOccupy { get; }
        public abstract bool CanBeAssignedToStaticJobTaskObject { get; }
        public abstract float ConditionReductionRate { get; }

        public abstract void InitializeServiceVehicle();
        public abstract void ResetServiceVehicle();

        public abstract string VehicleTypeEnumName { get; }
        public abstract void PatchedVehicleTypeEnum(Enums.VehicleType vehicleType);

        public new void Initialize()
        {
            base.Initialize();
            vehicleName = VehicleName;
            vehicleHitboxes = VehicleHitboxes;
            fuelType = FuelType;
            topSpeed = TopSpeed;
            operatingCost = OperatingCost;
            randomizeColor = RandomizeColor;
            maxQuantityLoadedGoods = MaxQuantityLoadedGoods;
            allowedDeliveryContainers = AllowedDeliveryContainers;
            acceptedTargetDistance = AcceptedTargetDistance;
            acceptedTargetDistanceTurn = AcceptedTargetDistanceTurn;
            nodesToOccupy = NodesToOccupy;
            canBeAssignedToStaticJobTaskObject = CanBeAssignedToStaticJobTaskObject;
            conditionReductionRate = ConditionReductionRate;
            vehicleType = VehicleTypeEnum;
            InitializeServiceVehicle();
        }

        public override void Reset()
        {
            base.Reset();
            ResetServiceVehicle();
        }

        [PatchTimeMethod]
        public void Patch()
        {
            Enums.VehicleType newlyPatchedVehicleType = EnumCache<Enums.VehicleType>.Instance.Patch(VehicleTypeEnumName);
            Utilities.Logger.Print($"Added VehicleType {newlyPatchedVehicleType}");
            PatchedVehicleTypeEnum(newlyPatchedVehicleType);
        }
    }
}
