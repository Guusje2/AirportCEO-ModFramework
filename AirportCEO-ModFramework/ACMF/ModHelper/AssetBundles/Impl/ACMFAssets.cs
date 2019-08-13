namespace ACMF.ModHelper.AssetBundles.Impl
{
    internal class ACMFAssets : ACMFAssetBundle
    {
        internal static ACMFAssets Instance = new ACMFAssets();

        protected override string AssetBundleName => "acmf";
        protected override string AssetBundleLocation => throw new System.NotImplementedException();
        protected override bool InSameDirectoryAsDLL => true;
        protected override bool ShouldLogContents => false;
    }
}
