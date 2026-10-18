using System.Reflection;
using Verse;
using HarmonyLib;

namespace BattIePatch_EnhancedBioregeneration
{
    //Setting the Harmony instance
    [StaticConstructorOnStartup]
    public class Main
    {
        static Main()
        {
            var harmony = new Harmony("com.BattIePatch_EnhancedBioregeneration");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
