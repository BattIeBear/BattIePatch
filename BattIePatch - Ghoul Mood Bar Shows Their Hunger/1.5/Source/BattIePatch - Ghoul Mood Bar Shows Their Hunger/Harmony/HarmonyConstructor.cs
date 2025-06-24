using HarmonyLib;
using RimWorld;
using System.Reflection;
using Verse;

namespace BattIePatch_GhoulMoodBarFix
{
    //Setting the Harmony instance
    [StaticConstructorOnStartup]
    public class Main
    {
        static Main()
        {
            var harmony = new Harmony("com.BattIePatch_GhoulMoodBarFix");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
