using ACMF.ModHelper.AssetBundles;

namespace SampleModNewStructure
{
    public class Assets : ACMFAssetBundle
    {
        public static Assets Instance = new Assets();

        protected override string AssetBundleName => "samplestructure";
        protected override string AssetBundleLocation => throw new System.NotImplementedException();
        protected override bool InSameDirectoryAsDLL => true;
        protected override bool ShouldLogContents => true;

        private Assets() { }
    }
}
