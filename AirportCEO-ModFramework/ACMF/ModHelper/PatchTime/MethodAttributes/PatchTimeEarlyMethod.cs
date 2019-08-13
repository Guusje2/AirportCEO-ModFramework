using System;

namespace ACMF.ModHelper.PatchTime.MethodAttributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class PatchTimeEarlyMethod : Attribute, IPatchTime
    {
    }
}
