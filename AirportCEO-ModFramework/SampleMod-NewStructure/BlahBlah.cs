using ACMF.ModHelper.ModPrefabs.Placeables.PlaceableStructures;
using UnityEngine;

namespace SampleModNewStructure
{
    public class BlahBlah : PlaceableStructureCreator<BlahBlah>
    {
        public static Enums.StructureType BlahBlahStructureTypeEnum => ActivePlaceableStructureCreators.GetCreator<BlahBlah>().StructureTypeEnum;

        public override string StructureTypeEnumName => "SampleStructure";
        public override GameObject Prefab { get; } = Assets.Instance.AttemptLoadGameObject("BasicWeatherStation");

        public override string IObjectNameKey => "SampleStructure";
        public override string IObjectName => "SampleStructure";
        public override string IObjectDescription => "A Cool Structure";
        public override Enums.ThreeStepScale IObjectSize => Enums.ThreeStepScale.Large;
        public override Enums.ObjectType IObjectType => Enums.ObjectType.Item;
        public override float IObjectCost => 100f;
        public override float IOperationsCost => 1f;
        public override float IConditionReductionRate => 0f;
        public override float ICleanlinessReductionRate => 0f;
        public override Enums.PlacementType IPlacementType => Enums.PlacementType.NonDragable;
        public override Vector2 IObjectGridSize => new Vector2(6.0f, 6.0f);
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
        public override bool ICanBeBuiltBelowGround => true;
        public override bool ICannotBeBuiltBelowTerminal => false;

        public override bool ShouldAppearInBuildMenu => true;
        public override Sprite ItemBuildSprite => Prefab.transform.Find("Sprite/Base").GetComponent<SpriteRenderer>().sprite;

        protected override void PatchedStructureTypeEnum(Enums.StructureType structureType) {}
        protected override void PostCreateNewInstance(GameObject gameObject) {}

        protected override void PostSetupPrefabDuringPatchtime(GameObject prefab)
        {
            prefab.transform.Find("Sprite/Base").GetComponent<SpriteRenderer>().sortingLayerName = "AboveObjects";
            prefab.transform.Find("Sprite/Animators/WindDirection").GetComponent<SpriteRenderer>().sortingLayerName = "AboveObjects";
            prefab.transform.Find("Sprite/Animators/Aneometer").GetComponent<SpriteRenderer>().sortingLayerName = "AboveObjects";
            prefab.transform.Find("Overlay/OverlaySprite").GetComponent<SpriteRenderer>().sortingLayerName = "SpriteOverlay";
            prefab.transform.Find("Overlay/ConstructionOverlay/ConstructionOverlaySprites/WireframeSprite").GetComponent<SpriteRenderer>().sortingLayerName = "SpriteOverlay";
            prefab.transform.Find("Lights/Toplight").GetComponent<SpriteRenderer>().sortingLayerName = "AboveObjects";
            prefab.transform.Find("Lights/Toplight (1)").GetComponent<SpriteRenderer>().sortingLayerName = "AboveObjects";

            Shader spriteDifuse = Shader.Find("Sprites/Diffuse");
            foreach (SpriteRenderer sr in prefab.GetComponentsInChildren<SpriteRenderer>())
            {
                sr.material.shader = spriteDifuse;
            }

            Shader spriteDefault = Shader.Find("Sprites/Default");
            prefab.transform.Find("Lights/Toplight").GetComponent<SpriteRenderer>().material.shader = spriteDefault;
            prefab.transform.Find("Lights/Toplight (1)").GetComponent<SpriteRenderer>().material.shader = spriteDefault;
        }
    }
}
