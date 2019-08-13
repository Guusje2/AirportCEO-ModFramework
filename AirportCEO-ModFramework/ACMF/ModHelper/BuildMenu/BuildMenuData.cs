using System;
using System.Collections.Generic;

namespace ACMF.ModHelper.BuildMenu
{
    public static class BuildMenuData
    {
        private static readonly List<BuildMenuItem> BuildItems = new List<BuildMenuItem>();
        
        public static void AddBuildItem(BuildMenuItem buildMenuItem)
        {
            BuildItems.Add(buildMenuItem);
        }

        internal static IEnumerable<BuildMenuItem> GetItems()
        {
            return BuildItems;
        }

        internal static int GetItemCount()
        {
            return BuildItems.Count;
        }
    }
}
