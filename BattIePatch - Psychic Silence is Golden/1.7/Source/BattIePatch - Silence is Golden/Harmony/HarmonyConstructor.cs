using HarmonyLib;
using System.Reflection;
using Verse;

namespace BattIePatch_SilenceIsGolden
{
    [StaticConstructorOnStartup]
    public class Main
    {
        static Main()
        {
            var harmony = new Harmony("com.BattIePatch_SilenceIsGolden");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
