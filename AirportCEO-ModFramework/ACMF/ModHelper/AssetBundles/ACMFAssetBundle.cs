using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace ACMF.ModHelper.AssetBundles
{
    public abstract class ACMFAssetBundle
    {
        public AssetBundle AssetBundle { get; private set; }
        public Dictionary<string, GameObject> GameObjects { get; private set; }

        protected abstract string AssetBundleName { get; }
        protected abstract string AssetBundleLocation { get; }
        protected abstract bool InSameDirectoryAsDLL { get; }
        protected abstract bool ShouldLogContents { get; }

        private readonly string finalLocation;

        public ACMFAssetBundle()
        {
            GameObjects = new Dictionary<string, GameObject>();
            finalLocation = GetAssetBundleLocation(Assembly.GetCallingAssembly().Location);
            if (DoesAssetBundleExist(finalLocation) == false)
            {
                Utilities.Logger.Error($"{GetType().FullName} Attempted to load a non-existent asset bundle at {finalLocation}");
                return;
            }

            LoadAssetBundle(finalLocation);

            if (ShouldLogContents)
                LogContents();
        }

        private bool DoesAssetBundleExist(string assetBundleLocation)
        {
            return File.Exists(assetBundleLocation);
        }

        private void LoadAssetBundle(string assetBundleLocation)
        {
            try
            {
                AssetBundle = AssetBundle.LoadFromFile(assetBundleLocation);
                foreach(GameObject gameObject in AssetBundle.LoadAllAssets<GameObject>())
                {
                    GameObjects.Add(gameObject.name, gameObject);
                }
            }
            catch (Exception e)
            {
                Utilities.Logger.Error($"Failed to load {AssetBundleName} : {e.ToString()}");
            }
        }

        private string GetAssetBundleLocation(string dllLocation)
        {
            if (InSameDirectoryAsDLL)
            {
                string assemblyDirectory = Path.GetDirectoryName(dllLocation);
                return Path.Combine(assemblyDirectory, AssetBundleName);
            }
            else
            {
                return Path.Combine(AssetBundleLocation, AssetBundleName);
            }
        }

        public GameObject AttemptLoadGameObject(string name)
        {
            bool contains = GameObjects.ContainsKey(name);
            if (contains)
                return GameObjects[name];
            else
            {
                Utilities.Logger.Error($"Asset Bundle ({finalLocation}) attempted to load non existent item: {name}");
                return null;
            }
        }

        private void LogContents()
        {
            Utilities.Logger.Print($"Printing asset bundle ({finalLocation}) contents:");
            foreach (KeyValuePair<string, GameObject> kvp in GameObjects)
                Utilities.Logger.Print($"GameObject {kvp.Key} -> {kvp.Value}");
        }
    }
}
