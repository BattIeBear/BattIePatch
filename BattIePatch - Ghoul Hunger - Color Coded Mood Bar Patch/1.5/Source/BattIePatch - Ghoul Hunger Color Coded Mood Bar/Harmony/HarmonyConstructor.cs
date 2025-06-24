using HarmonyLib;
using System.Reflection;
using Verse;

namespace BattIePatch_GhoulHungerColorCodedMoodBarPatch
{
    //Setting the Harmony instance
    [StaticConstructorOnStartup]
    public class Main
    {
        static Main()
        {
            var harmony = new Harmony("com.BattIePatch_GhoulHungerColorCodedMoodBarPatch");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}