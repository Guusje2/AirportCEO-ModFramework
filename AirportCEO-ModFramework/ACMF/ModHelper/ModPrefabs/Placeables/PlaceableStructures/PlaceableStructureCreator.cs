using ACMF.ModHelper.BuildMenu;
using ACMF.ModHelper.EnumPatcher;
using ACMF.ModHelper.ModPrefabs.Placeables.PlaceableStructures.Interfaces;
using ACMF.ModHelper.PatchTime.MethodAttributes;
using ACMF.ModHelper.Utilities.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;

/**
* TO:DO Alot of code duplication with PlaceableItemCreator. -> Needs to become common parent called PlaceableObjectCreator
* */
namespace ACMF.ModHelper.ModPrefabs.Placeables.PlaceableStructures
{
    public abstract class PlaceableStructureCreator<T> : IACMFPlaceableStructure, IPlaceableStructureCreator, IAppearInBuildMenu where T : PlaceableStructureCreator<T>
    {
        public Enums.StructureType StructureTypeEnum { get; private set; }
        public abstract string StructureTypeEnumName { get; }
        public abstract GameObject Prefab { get; }

        public abstract string[] ColorableSprites { get; }
        public abstract bool ICanBeBuiltBelowGround { get; }
        public abstract bool ICannotBeBuiltBelowTerminal { get; }
        public abstract string IObjectNameKey { get; }
        public abstract string IObjectName { get; }
        public abstract string IObjectDescription { get; }
        public abstract Enums.ThreeStepScale IObjectSize { get; }
        public abstract Enums.ObjectType IObjectType { get; }
        public abstract float IObjectCost { get; }
        public abstract float IOperationsCost { get; }
        public abstract float IConditionReductionRate { get; }
        public abstract float ICleanlinessReductionRate { get; }
        public abstract Enums.PlacementType IPlacementType { get; }
        public abstract Vector2 IObjectGridSize { get; }
        public abstract float ISnapSize { get; }
        public abstract Vector2 ISnapOffset { get; }
        public abstract bool IAffectsPassengerGrid { get; }
        public abstract bool IAffectsAreas { get; }
        public abstract bool IAffectsRoadNodes { get; }
        public abstract bool IShouldNotConstruct { get; }
        public abstract int IConstructionEnergyRequired { get; }
        public abstract int INbrOfContractorsPossible { get; }
        public abstract bool IHasOverlaySprite { get; }
        public abstract bool IIsClickable { get; }
        public abstract bool IIsNotLeftClickable { get; }
        public abstract bool IIsColorable { get; }
        public abstract Enums.QualityType IObjectQuality { get; }

        public abstract bool ShouldAppearInBuildMenu { get; }
        public abstract Sprite ItemBuildSprite { get; }

        protected abstract void PatchedStructureTypeEnum(Enums.StructureType structureType);
        protected abstract void PostSetupPrefabDuringPatchtime(GameObject prefab);
        protected abstract void PostCreateNewInstance(GameObject gameObject);

        [PatchTimeMethod]
        public void Patch()
        {
            StructureTypeEnum = EnumCache<Enums.StructureType>.Instance.Patch(StructureTypeEnumName);
            PatchedStructureTypeEnum(StructureTypeEnum);
            SetupPrefabDuringPatchtime(Prefab);
            ActivePlaceableStructureCreators.Add<T>(this, StructureTypeEnum);

            if (ShouldAppearInBuildMenu)
            {
                BuildMenuData.AddBuildItem(new BuildMenuItem(
                    sprite: ItemBuildSprite,
                    itemName: $"{IObjectName} (${IObjectCost})",
                    onClick: () => BuildingController.Instance.SpawnStructure(StructureTypeEnum)
                ));
            }

            Utilities.Logger.Print($"Registered PlaceableStructureCreator of Controller {typeof(T).Name}");
        }

        protected virtual void SetupPrefabDuringPatchtime(GameObject prefab)
        {
            AddPlaceableComponent<PlaceableStructure>(prefab);
            BoundaryHandler bh = AddBoundaryHandler(prefab);
            prefab.GetComponent<PlaceableObject>().boundary = bh;

            if (IHasOverlaySprite)
            {
                prefab.transform.Find("Overlay").gameObject.AddComponent<OverlayHandler>();
                prefab.transform.Find("Overlay/ConstructionOverlay/ConstructionOverlayCanvas").gameObject.AddComponent<FixRotation>();
            }

            PostSetupPrefabDuringPatchtime(prefab);
        }

        public GameObject CreateNewInstance()
        {
            GameObject go = UnityEngine.Object.Instantiate(Prefab);
            PostCreateNewInstance(go);
            return go;
        }

        protected virtual A AddPlaceableComponent<A>(GameObject prefab) where A : PlaceableStructure
        {
            A structureComponent = prefab.GetOrAddComponent<A>();

            structureComponent.structureType = StructureTypeEnum;
            structureComponent.canBuildBelowGround = ICanBeBuiltBelowGround;
            structureComponent.cannotBuiltBelowTerminal = ICannotBeBuiltBelowTerminal;

            structureComponent.staticObjectNameKey = IObjectNameKey;
            structureComponent.objectName = IObjectName;
            structureComponent.objectDescription = IObjectDescription;
            structureComponent.objectSize = IObjectSize;
            structureComponent.objectType = IObjectType;
            structureComponent.objectCost = IObjectCost;
            structureComponent.operationsCost = IOperationsCost;
            structureComponent.conditionReductionRate = IConditionReductionRate;
            structureComponent.cleanlinessReductionRate = ICleanlinessReductionRate;
            structureComponent.placementType = IPlacementType;
            structureComponent.objectGridSize = IObjectGridSize;
            structureComponent.snapSize = ISnapSize;
            structureComponent.snapOffset = ISnapOffset;
            structureComponent.affectsPassengerGrid = IAffectsPassengerGrid;
            structureComponent.affectsAreas = IAffectsAreas;
            structureComponent.affectsRoadNodes = IAffectsRoadNodes;
            structureComponent.shouldNotConstruct = IShouldNotConstruct;
            structureComponent.constructionEnergyRequired = IConstructionEnergyRequired;
            structureComponent.nbrOfContractorsPossible = INbrOfContractorsPossible;
            structureComponent.hasOverlaySprite = IHasOverlaySprite;
            structureComponent.isClickable = IIsClickable;
            structureComponent.isNotLeftClickable = IIsNotLeftClickable;
            structureComponent.isColorable = IIsColorable;
            structureComponent.objectQuality = IObjectQuality;

            List<SpriteRenderer> colorableSprites = new List<SpriteRenderer>();
            foreach (string colorableSprite in ColorableSprites)
            {
                SpriteRenderer sr = prefab.transform?.Find(colorableSprite)?.gameObject?.GetComponent<SpriteRenderer>();
                if (sr != null)
                    colorableSprites.Add(sr);
            }
            structureComponent.colorableSprites = colorableSprites.ToArray();

            return structureComponent;
        }

        protected virtual BoundaryHandler AddBoundaryHandler(GameObject prefab)
        {
            Transform boundary = prefab.transform.Find("Boundary");
            Transform startPos = boundary.Find("StartPos");
            Transform endPos = boundary.Find("EndPos");
            BoundaryHandler bh = boundary.gameObject.AddComponent<BoundaryHandler>();
            bh.penalty = 0;
            bh.border = new Boundary(BoundaryHandler.BoundaryType.Border, startPos, endPos, boundary.gameObject);

            return bh;
        }
    }
}
