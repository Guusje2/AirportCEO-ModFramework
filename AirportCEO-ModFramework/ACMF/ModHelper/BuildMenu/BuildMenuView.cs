using ACMF.ModHelper.AssetBundles.Impl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ACMF.ModHelper.BuildMenu
{
    internal class BuildMenuView
    {
        private readonly string BUILD_MENU_PREFAB_NAME = "BuildMenuCanvas";
        private readonly string BUILD_MENU_ITEM_VIEW_PREFAB_NAME = "BuildMenuItem";
        internal static BuildMenuView Instance { get; private set; }

        private readonly GameObject BuildMenuGO;
        private readonly GameObject Area;
        private readonly GameObject ContentHolder;
        private readonly GameObject ItemPrefab;

        private BuildMenuView()
        {
            BuildMenuGO = Object.Instantiate(ACMFAssets.Instance.AttemptLoadGameObject(BUILD_MENU_PREFAB_NAME));
            Area = BuildMenuGO.transform.Find("Area").gameObject;
            Hide();

            Area.AddComponent<MoveableWindow>();
            Area.transform.Find("Background/Header/CloseButton").gameObject.GetComponent<Button>().onClick.AddListener(() => Hide());
            ContentHolder = Area.transform.Find("Background/BodyListView/ListViewport/Content").gameObject;

            ItemPrefab = ACMFAssets.Instance.AttemptLoadGameObject(BUILD_MENU_ITEM_VIEW_PREFAB_NAME);
            CreateItems();
        }

        private void CreateItems()
        {
            Utilities.Logger.Print($"BuildMenu adding {BuildMenuData.GetItemCount()} items");
            foreach (BuildMenuItem buildMenuItem in BuildMenuData.GetItems())
            {
                GameObject itemView = Object.Instantiate(ItemPrefab);
                itemView.transform.Find("Image").GetComponent<Image>().sprite = buildMenuItem.Sprite;
                itemView.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = buildMenuItem.ItemName;
                itemView.transform.SetParent(ContentHolder.transform, false);
                itemView.GetComponent<Button>().onClick.AddListener(() => DoItemViewClick(buildMenuItem));
            }
        }

        internal static void NewGameStarted() => Instance = new BuildMenuView();

        internal void Show() => BuildMenuGO.SetActive(true);

        internal void Hide() => BuildMenuGO.SetActive(false);

        internal void Toggle()
        {
            if (BuildMenuGO.activeSelf)
                Hide();
            else
                Show();
        }

        private void DoItemViewClick(BuildMenuItem buildMenuItem)
        {
            buildMenuItem.OnClick?.Invoke();
            if (buildMenuItem.HideMenuAfterClick)
                Hide();
        }
    }
}
