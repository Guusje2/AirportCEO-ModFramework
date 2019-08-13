using ACMF.ModHelper.AssetBundles.Impl;
using Harmony;

namespace ACMF.ModHelper.BuildMenu
{
    [HarmonyPatch(typeof(GameController))]
    [HarmonyPatch("Start")]
    public class GameControllerStartPatcher
    {
        [HarmonyPostfix]
        public static void Postfix()
        {
            BuildMenuView.NewGameStarted();
            ToggleBuildMenuButton.ShowButton();
        }
    }
}
