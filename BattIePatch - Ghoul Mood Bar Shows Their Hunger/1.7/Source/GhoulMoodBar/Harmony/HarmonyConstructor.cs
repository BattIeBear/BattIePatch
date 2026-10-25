using HarmonyLib;
using System.Reflection;
using Verse;

namespace BattIePatch_GhoulMoodBarShowsTheirHunger
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