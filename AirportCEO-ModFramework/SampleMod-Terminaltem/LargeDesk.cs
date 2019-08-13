using ACMF.ModHelper.ModPrefabs.Placeables.PlaceableItems;
using ACMF.ModHelper.ModPrefabs.Placeables.PlaceableItems.Interfaces;
using UnityEngine;

namespace SampleModTerminaltem
{
    public class LargeDesk : PlaceableItemCreator<LargeDesk>, IACMFPlaceableItemCustomSerializationSystem
    {
        public static Enums.ItemType LargeDeskItemTypeEnum => ActivePlaceableItemCreators.GetCreator<LargeDesk>().ItemTypeEnum;

        public override string ItemTypeEnumName => "LargeOfficeDesk";
        public override GameObject Prefab { get; } = Assets.Instance.AttemptLoadGameObject("LargeOfficeDesk");
        public override string IObjectNameKey => "Large Deluxe Desk";
        public override string IObjectName => "Large Deluxe Desk";
        public override string IObjectDescription => "A desk fit for a CEO.";
        public override Enums.ThreeStepScale IObjectSize => Enums.ThreeStepScale.Medium;
        public override Enums.ObjectType IObjectType => Enums.ObjectType.Item;
        public override float IObjectCost => 100f;
        public override float IOperationsCost => 1f;
        public override float IConditionReductionRate => 0f;
        public override float ICleanlinessReductionRate => 0f;
        public override Enums.PlacementType IPlacementType => Enums.PlacementType.NonDragable;
        public override Vector2 IObjectGridSize => new Vector2(3.0f, 2.0f);
        public override float ISnapSize => 1f;
        public override Vector2 ISnapOffset => Vector2.zero;
        public override bool IAffectsPassengerGrid => true;
        public override bool IAffectsAreas => false;
        public override bool IAffectsRoadNodes => false;
        public override bool IShouldNotConstruct => false;
        public override int IConstructionEnergyRequired => 150;
        public override int INbrOfContractorsPossible => 1;
        public override bool IHasOverlaySprite => true;
        public override bool IIsClickable => false;
        public override bool IIsNotLeftClickable => false;
        public override bool IIsColorable => false;
        public override string[] ColorableSprites => new string[0];
        public override Enums.QualityType IObjectQuality => Enums.QualityType.High;
        public override Enums.ItemPlacementArea IItemPlacementArea => Enums.ItemPlacementArea.Inside;
        public override Enums.GenericZoneType[] IAllowedGenericZones => new Enums.GenericZoneType[] { };
        public override Enums.SpecificZoneType[] IAllowedSpecificZones => new Enums.SpecificZoneType[] { Enums.SpecificZoneType.StaffZone };
        public override Enums.RoomType[] IAllowedRooms => new Enums.RoomType[] { Enums.RoomType.StaffRoom };
        public override bool IMustBeWithinGenericZone => false;
        public override bool IMustBeWithinSpecificZone => true;
        public override bool IMustBeWithinRoom => true;

        public override bool ShouldAppearInBuildMenu => true;
        public override Sprite ItemBuildSprite => Prefab.transform.Find("Sprite/Desk").GetComponent<SpriteRenderer>().sprite;

        /**
         * If you are just doing boring serialization/deserialization like the following
         * remove the interface IACMFPlaceableItemCustomSerializationSystem and these following methods
         * and allow the system serialize/deserialize for you.
         * This is the default serialization/deseralization behaviour.
         */
        public PlaceableItemSerializable SerializeItem(PlaceableItem placeableItem)
        {
            PlaceableItemSerializable placeableItemSerializable = new PlaceableItemSerializable();
            placeableItemSerializable.SetObjectForSerializer(placeableItem);
            return placeableItemSerializable;
        }

        /**
         * Read the above comment for SerializeItem()
         */
        public bool DeserializeItem(PlaceableItemSerializable placeableItemSerializable)
        {
            PlaceableItem component = 
                Object.Instantiate(Prefab, placeableItemSerializable.position, placeableItemSerializable.rotation, FolderController.Instance.itemsFolder).GetComponent<PlaceableItem>();
            component.SetObjectFromSerializer(placeableItemSerializable);
            component.ChangeToPlaced(false);

            return true;
        }

        protected override void PatchedItemTypeEnum(Enums.ItemType itemType) { }
        protected override void PostCreateNewInstance(GameObject gameObject) { }

        protected override void PostSetupPrefabDuringPatchtime(GameObject prefab)
        {
            ShadowHandler shadowHandler = prefab.transform.Find("Sprite/Shadow").gameObject.AddComponent<ShadowHandler>();
            shadowHandler.shadowDistance = 0.3f;
            shadowHandler.referenceTransform = prefab.transform;

            ShadowHandler shadowHandler2 = prefab.transform.Find("Sprite/OfficeChair/Shadow").gameObject.AddComponent<ShadowHandler>();
            shadowHandler2.shadowDistance = 0.1f;
            shadowHandler2.referenceTransform = prefab.transform.Find("Sprite/OfficeChair");

            prefab.transform.Find("Sprite/Desk").GetComponent<SpriteRenderer>().sortingLayerName = "BelowObjects";
            prefab.transform.Find("Sprite/Shadow").GetComponent<SpriteRenderer>().sortingLayerName = "BelowObjects";
            prefab.transform.Find("Sprite/OfficeChair").GetComponent<SpriteRenderer>().sortingLayerName = "SeatingObjects";
            prefab.transform.Find("Sprite/OfficeChair/Shadow").GetComponent<SpriteRenderer>().sortingLayerName = "SeatingObjects";
            prefab.transform.Find("Sprite/DeskItems/Computer").GetComponent<SpriteRenderer>().sortingLayerName = "BelowObjects";
            prefab.transform.Find("Sprite/DeskItems/Computer (2)").GetComponent<SpriteRenderer>().sortingLayerName = "BelowObjects";
            prefab.transform.Find("Sprite/DeskItems/Computer (3)").GetComponent<SpriteRenderer>().sortingLayerName = "BelowObjects";
            prefab.transform.Find("Sprite/DeskItems/Keyboard").GetComponent<SpriteRenderer>().sortingLayerName = "BelowObjects";
            prefab.transform.Find("Sprite/DeskItems/Phone").GetComponent<SpriteRenderer>().sortingLayerName = "BelowObjects";
            prefab.transform.Find("Sprite/DeskItems/Lamp").GetComponent<SpriteRenderer>().sortingLayerName = "BelowObjects";
            prefab.transform.Find("Sprite/DeskItems/Pens/Pen").GetComponent<SpriteRenderer>().sortingLayerName = "BelowObjects";
            prefab.transform.Find("Sprite/DeskItems/Pens/Pen (3)").GetComponent<SpriteRenderer>().sortingLayerName = "BelowObjects";
            prefab.transform.Find("Sprite/DeskItems/Pens/Pen (4)").GetComponent<SpriteRenderer>().sortingLayerName = "BelowObjects";
            prefab.transform.Find("Sprite/DeskItems/Paper/Paper").GetComponent<SpriteRenderer>().sortingLayerName = "BelowObjects";
            prefab.transform.Find("Sprite/DeskItems/Paper/Paper (2)").GetComponent<SpriteRenderer>().sortingLayerName = "BelowObjects";
            prefab.transform.Find("Sprite/DeskItems/Paper/Paper (4)").GetComponent<SpriteRenderer>().sortingLayerName = "BelowObjects";
            prefab.transform.Find("Sprite/DeskItems/Paper/Paper (5)").GetComponent<SpriteRenderer>().sortingLayerName = "BelowObjects";
            prefab.transform.Find("Overlay/OverlaySprite").GetComponent<SpriteRenderer>().sortingLayerName = "SpriteOverlay";
            prefab.transform.Find("Overlay/ConstructionOverlay/ConstructionOverlaySprites/WireframeSprite").GetComponent<SpriteRenderer>().sortingLayerName = "SpriteOverlay";

            Shader spriteDefault = Shader.Find("Sprites/Default");
            foreach (SpriteRenderer sr in prefab.GetComponentsInChildren<SpriteRenderer>())
            {
                sr.material.shader = spriteDefault;
            }
        }
    }
}
