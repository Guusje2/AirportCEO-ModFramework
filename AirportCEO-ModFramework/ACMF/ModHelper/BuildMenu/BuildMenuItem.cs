using System;
using UnityEngine;

namespace ACMF.ModHelper.BuildMenu
{
    public class BuildMenuItem
    {
        internal Sprite Sprite { get; private set; }
        internal string ItemName { get; private set; }
        internal Action OnClick { get; private set; }
        internal bool HideMenuAfterClick { get; private set; }

        public BuildMenuItem(Sprite sprite, string itemName, Action onClick = null, bool hideMenuAfterClick = true)
        {
            Sprite = sprite;
            ItemName = itemName;
            OnClick = onClick;
            HideMenuAfterClick = hideMenuAfterClick;
        }
    }
}
