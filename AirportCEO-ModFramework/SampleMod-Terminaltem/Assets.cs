using ACMF.ModHelper.AssetBundles;

namespace SampleModTerminaltem
{
    public class Assets : ACMFAssetBundle
    {
        public static Assets Instance = new Assets();

        protected override string AssetBundleName => "terminalitem";
        protected override string AssetBundleLocation => throw new System.NotImplementedException();
        protected override bool InSameDirectoryAsDLL => true;
        protected override bool ShouldLogContents => false;

        private Assets() { }
    }
}
