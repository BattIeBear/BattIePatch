using HarmonyLib;
using System.Reflection;
using Verse;

namespace BattIePatch_TendAgain
{
    [StaticConstructorOnStartup]
    public class Main
    {
        static Main()
        {
            var harmony = new Harmony("com.BattIePatch_TendAgain");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
