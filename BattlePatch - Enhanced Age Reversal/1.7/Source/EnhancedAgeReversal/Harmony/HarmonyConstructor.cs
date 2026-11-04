using HarmonyLib;
using System.Reflection;
using Verse;

namespace BattlePatch_EnhancedAgeReversal
{
    //Setting the Harmony instance
    [StaticConstructorOnStartup]
    public class Main
    {
        static Main()
        {
            var harmony = new Harmony("com.BattlePatch_EnhancedAgeReversal");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}