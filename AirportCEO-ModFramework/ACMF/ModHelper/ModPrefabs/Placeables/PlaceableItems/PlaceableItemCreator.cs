using ACMF.ModHelper.BuildMenu;
using ACMF.ModHelper.EnumPatcher;
using ACMF.ModHelper.ModPrefabs.Placeables.PlaceableItems.Interfaces;
using ACMF.ModHelper.PatchTime.MethodAttributes;
using ACMF.ModHelper.Utilities.Extensions;
using System.Collections.Generic;
using UnityEngine;

/**
 * TO:DO Alot of code duplication with PlaceableStructureCreator. -> Needs to become common parent called PlaceableObjectCreator
 */
namespace ACMF.ModHelper.ModPrefabs.Placeables.PlaceableItems
{
    public abstract class PlaceableItemCreator<T> : IACMFPlaceableItem, IPlaceableItemCreator, IAppearInBuildMenu where T : PlaceableItemCreator<T>
    {
        public Enums.ItemType ItemTypeEnum { get; private set; }
        public abstract string ItemTypeEnumName { get; }
        public abstract GameObject Prefab { get; }

        public abstract string[] ColorableSprites { get; }
        public abstract Enums.ObjectType IObjectType { get; }
        public abstract string IObjectNameKey { get; }
        public abstract string IObjectName { get; }
        public abstract string IObjectDescription { get; }
        public abstract Enums.ThreeStepScale IObjectSize { get; }
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
        public abstract Enums.ItemPlacementArea IItemPlacementArea { get; }
        public abstract Enums.GenericZoneType[] IAllowedGenericZones { get; }
        public abstract Enums.SpecificZoneType[] IAllowedSpecificZones { get; }
        public abstract Enums.RoomType[] IAllowedRooms { get; }
        public abstract bool IMustBeWithinGenericZone { get; }
        public abstract bool IMustBeWithinSpecificZone { get; }
        public abstract bool IMustBeWithinRoom { get; }

        public abstract Sprite ItemBuildSprite { get; }
        public abstract bool ShouldAppearInBuildMenu { get; }

        protected abstract void PatchedItemTypeEnum(Enums.ItemType itemType);
        protected abstract void PostSetupPrefabDuringPatchtime(GameObject prefab);
        protected abstract void PostCreateNewInstance(GameObject gameObject);

        [PatchTimeMethod]
        public void Patch()
        {
            ItemTypeEnum = EnumCache<Enums.ItemType>.Instance.Patch(ItemTypeEnumName);
            PatchedItemTypeEnum(ItemTypeEnum);
            SetupPrefabDuringPatchtime(Prefab);
            ActivePlaceableItemCreators.Add<T>(this, ItemTypeEnum);

            if (ShouldAppearInBuildMenu)
            {
                BuildMenuData.AddBuildItem(new BuildMenuItem(
                    sprite: ItemBuildSprite,
                    itemName: $"{IObjectName} (${IObjectCost})",
                    onClick: () => BuildingController.Instance.SpawnItem(ItemTypeEnum)
                ));
            }

            Utilities.Logger.Print($"Registered PlaceableItemCreator of Controller {typeof(T).Name}");
        }

        protected virtual void SetupPrefabDuringPatchtime(GameObject prefab)
        {
            AddPlaceableComponent<PlaceableItem>(prefab);
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
            GameObject go = Object.Instantiate(Prefab);
            PostCreateNewInstance(go);
            return go;
        }

        protected virtual A AddPlaceableComponent<A>(GameObject prefab) where A : PlaceableItem
        {
            A itemModel = prefab.GetOrAddComponent<A>();
            itemModel.itemType = ItemTypeEnum;
            itemModel.itemPlacementArea = IItemPlacementArea;
            itemModel.allowedGenericZones = IAllowedGenericZones;
            itemModel.allowedSpecificZones = IAllowedSpecificZones;
            itemModel.allowedRooms = IAllowedRooms;
            itemModel.mustBeWithinGenericZone = IMustBeWithinGenericZone;
            itemModel.mustBeWithinSpecificZone = IMustBeWithinSpecificZone;
            itemModel.mustBeWithinRoom = IMustBeWithinRoom;

            itemModel.staticObjectNameKey = IObjectNameKey;
            itemModel.objectName = IObjectName;
            itemModel.objectDescription = IObjectDescription;
            itemModel.objectSize = IObjectSize;
            itemModel.objectType = IObjectType;
            itemModel.objectCost = IObjectCost;
            itemModel.operationsCost = IOperationsCost;
            itemModel.conditionReductionRate = IConditionReductionRate;
            itemModel.cleanlinessReductionRate = ICleanlinessReductionRate;
            itemModel.placementType = IPlacementType;
            itemModel.objectGridSize = IObjectGridSize;
            itemModel.snapSize = ISnapSize;
            itemModel.snapOffset = ISnapOffset;
            itemModel.affectsPassengerGrid = IAffectsPassengerGrid;
            itemModel.affectsAreas = IAffectsAreas;
            itemModel.affectsRoadNodes = IAffectsRoadNodes;
            itemModel.shouldNotConstruct = IShouldNotConstruct;
            itemModel.constructionEnergyRequired = IConstructionEnergyRequired;
            itemModel.nbrOfContractorsPossible = INbrOfContractorsPossible;
            itemModel.hasOverlaySprite = IHasOverlaySprite;
            itemModel.isClickable = IIsClickable;
            itemModel.isNotLeftClickable = IIsNotLeftClickable;
            itemModel.isColorable = IIsColorable;
            itemModel.objectQuality = IObjectQuality;

            List<SpriteRenderer> colorableSprites = new List<SpriteRenderer>();
            foreach(string colorableSprite in ColorableSprites)
            {
                SpriteRenderer sr = prefab.transform?.Find(colorableSprite)?.gameObject?.GetComponent<SpriteRenderer>();
                if (sr != null)
                    colorableSprites.Add(sr);
            }
            itemModel.colorableSprites = colorableSprites.ToArray();

            return itemModel;
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
