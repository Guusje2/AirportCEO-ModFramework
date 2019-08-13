using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using ACMF.ModHelper.PatchTime.MethodAttributes;

namespace ACMF.ModHelper.PatchTime
{
    internal static class PatchTimeManager
    {
        internal static void Initialise()
        {
            List<Assembly> assembliesToCheck = FindAssembliesToCheck();
            List<Tuple<Type, MethodInfo>> patchEarlyMethods = FindPatchTimeMethods<PatchTimeEarlyMethod>(assembliesToCheck);
            List<Tuple<Type, MethodInfo>> patchRegularMethods = FindPatchTimeMethods<PatchTimeMethod>(assembliesToCheck);
            List<Tuple<Type, MethodInfo>> patchLateMethods = FindPatchTimeMethods<PatchTimeLateMethod>(assembliesToCheck);

            Utilities.Logger.Print($"Found {patchEarlyMethods.Count()} Early Patch Time Methods.");
            Utilities.Logger.Print($"Found {patchRegularMethods.Count()} Regular Patch Time Methods.");
            Utilities.Logger.Print($"Found {patchLateMethods.Count()} Late Patch Time Methods.");

            ExecutePatchTimes(patchEarlyMethods);
            ExecutePatchTimes(patchRegularMethods);
            ExecutePatchTimes(patchLateMethods);
        }

        private static List<Assembly> FindAssembliesToCheck()
        {
            List<Assembly> assembliesToCheck = new List<Assembly>();
            assembliesToCheck.AddRange(ModLoader.ModLoader.ModsLoaded.Select(t => t.Value.Assembly));
            assembliesToCheck.Add(Assembly.GetExecutingAssembly());

            return assembliesToCheck;
        }

        private static List<Tuple<Type, MethodInfo>> FindPatchTimeMethods<T>(List<Assembly> assembliesToCheck) where T : IPatchTime
        {
            return assembliesToCheck.SelectMany(t => t.GetTypes())
                    .SelectMany(t => t.GetMethods())
                    .Where(m => m.GetCustomAttributes(typeof(T), true).Length > 0 && IsValidMethod(m))
                    .Select(m => new Tuple<Type, MethodInfo>(m.ReflectedType, m))
                    .ToList();
        }

        private static void ExecutePatchTimes(List<Tuple<Type, MethodInfo>> methods)
        {
            foreach (Tuple<Type, MethodInfo> tuple in methods)
            {
                try
                {
                    if (tuple.Item2.IsStatic)
                        tuple.Item2.Invoke(null, null);
                    else
                        tuple.Item2.Invoke(Activator.CreateInstance(tuple.Item1), null);
                }
                catch (Exception e)
                {
                    Utilities.Logger.Error($"{tuple.Item1} {tuple.Item2} || Failed To Patch: {e.ToString()}");
                }
            }
        }

        private static bool IsValidMethod(MethodInfo methodInfo)
        {
            return (methodInfo.ReflectedType.IsClass && !methodInfo.ReflectedType.IsAbstract) || methodInfo.IsStatic;
        }
    }
}
