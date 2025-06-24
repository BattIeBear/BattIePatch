using HarmonyLib;
using System.Reflection;
using Verse;

namespace XenotypeBodyPuristFix
{
    //Setting the Harmony instance
    [StaticConstructorOnStartup]
    public class Main
    {
        static Main()
        {
            var harmony = new Harmony("com.XenotypeBodyPuristFix");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}