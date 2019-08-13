using UnityEngine;

namespace ACMF.ModHelper.Utilities.Extensions
{
    public static class GameObjectExtensions
    {
        public static void PrintDirectChildren(this GameObject go)
        {
            Logger.Print("--------------");
            Logger.Print($"GameObject {go.name} direct children");
            foreach (Transform child in go.transform)
            {
                Logger.Print($"GameObject {go.name} has direct child {child.name}");
            }
            Logger.Print("--------------");
        }

        public static void PrintDirectChildrenComponents(this GameObject go)
        {
            foreach (Transform child in go.transform)
            {
                Logger.Print("--------------");
                Logger.Print($"GameObject {go.name} has direct child {child.name}");
                foreach (Component c in child.GetComponents<Component>())
                {
                    Logger.Print($"Direct Child {child.name} has component {c.GetType()}");
                }
                Logger.Print("--------------");
            }
        }

        public static void PrintAllChildrenComponents(this GameObject go)
        {
            Logger.Print("--------------");
            Logger.Print($"GameObject: {go.name}");
            foreach (Component c in go.GetComponentsInChildren<Component>())
            {
                Logger.Print($"Child {c.gameObject.name} has component {c.GetType()}");
            }
            Logger.Print("--------------");
        }

        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            T component = go.GetComponent<T>();
            if (component == null)
                component = go.AddComponent<T>();

            return component;
        }
    }
}
