using ACMF.ModHelper.AssetBundles.Impl;
using UnityEngine;
using UnityEngine.UI;

namespace ACMF.ModHelper.BuildMenu
{
    internal static class ToggleBuildMenuButton
    {
        private static readonly string TOGGLE_BUILD_MENU_BUTTON_PREFAB_NAME = "BuildMenuToggleButtonCanvas";
        private static GameObject ToggleBuildMenuGO;

        internal static void ShowButton()
        {
            if (ToggleBuildMenuGO != null)
                return;

            ToggleBuildMenuGO = Object.Instantiate(ACMFAssets.Instance.AttemptLoadGameObject(TOGGLE_BUILD_MENU_BUTTON_PREFAB_NAME));
            ToggleBuildMenuGO.transform.Find("BuildMenuToggleButton").GetComponent<Button>().onClick.AddListener(() => {
                if (BuildMenuView.Instance != null)
                    BuildMenuView.Instance.Toggle();
            });
        }
    }
}
