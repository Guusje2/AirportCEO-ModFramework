using ACMF.ModHelper.PatchTime;
using ACMF.ModLoader;
using ACMF.ModLoader.Utilities;
using Harmony;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace ACMF
{
    public class ACMF
    {
        public static readonly ModVersion Version = new ModVersion(0, 1, 0);
        public static readonly string ACMFFolderLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static readonly string UNIQUE_ID = "AirportCEO.ModFramework";

        internal static ModHelper.Config.ModHelperConfig Config { get; private set; } = null;
        private static bool Initialised = false;

        public static void Entry()
        {
            if (Initialised)
                return;

            Initialised = true;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Logger.Print($"ACMF Directory: {ACMFFolderLocation}");
            Logger.Print($"Loading all DLLS in {ACMFFolderLocation}");

            string[] dllsToLoad = Directory.GetFiles(ACMFFolderLocation, "*.dll", SearchOption.TopDirectoryOnly);
            foreach (string dll in dllsToLoad)
            {
                if (Path.GetFileName(dll).Equals("ACMF.dll"))
                    continue;

                Logger.Print($"Loading DLL: {Path.GetFileName(dll)}");
                Assembly.LoadFrom(dll);
            }

            Logger.Print($"Loading ModHelper...");
            Config = ModHelper.Config.ACMFConfigManager.LoadConfig<ModHelper.Config.ModHelperConfig>(UNIQUE_ID);
            HarmonyInstance harmonyInstance = HarmonyInstance.Create(UNIQUE_ID);
            harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());

            Logger.Print($"Initialising ModLoader");
            ModLoader.ModLoader.Initialise();

            Logger.Print($"Executing PatchTime");
            PatchTimeManager.Initialise();

            stopwatch.Stop();
            Logger.Print($"ACMF took {stopwatch.ElapsedMilliseconds} ms to load.");
        }
    }
}
