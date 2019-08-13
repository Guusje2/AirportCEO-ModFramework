using ACMF.ModLoader;
using ACMF.ModLoader.Attributes;
using Harmony;
using System.Reflection;
using UnityEngine;

namespace SampleModBasicMod
{
    [ACMFMod(id: "ACMF.SampleMod.BasicMod", name: "Sample-Mod-Basic-Mod", modVersion: "12.3.5", requiredACMLVersion: "0.0.0")]
    public class EntryPoint
    {
        public static Config Config { get; private set; } = null;

        [ACMFModEntryPoint]
        public static void Entry(Mod mod)
        {
            System.Console.WriteLine($"[{mod.ModInfo.ID}] {mod.ModInfo.Name} executing.");
            Config = ACMF.ModHelper.Config.ACMFConfigManager.LoadConfig<Config>(mod);
            HarmonyInstance harmony = HarmonyInstance.Create(mod.ModInfo.ID);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }

    [HarmonyPatch(typeof(GameController))]
    [HarmonyPatch("Update")]
    public class GameControllerPatcher
    {
        [HarmonyPostfix]
        public static void Postfix()
        {
            if (EntryPoint.Config.ALLOW_GAME_CONTROLLER_PATCH && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.B))
            {
                ACMF.ModHelper.DialogPopup.DialogManager.QueueMessagePanel("Working Postfix Onto GameController::Update");
                ACMF.ModHelper.Utilities.Logger.ShowNotification("Working Postfix Onto GameController::Update");
            }
        }
    }

    public class Config : ACMF.ModHelper.Config.ACMFConfig
    {
        public bool ALLOW_GAME_CONTROLLER_PATCH = true;
    }
}
