using HarmonyLib;
using System.Reflection;
using Verse;

namespace BattIePatch_ExtremeHemogenExtraction
{
    [StaticConstructorOnStartup]
    public class Main
    {
        static Main()
        {
            var harmony = new Harmony("com.BattIePatch_ExtremeHemogenExtraction");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
