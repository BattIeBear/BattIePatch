using HarmonyLib;
using System.Reflection;
using Verse;

namespace BattIePatch_IEDRemoteDetonation
{
    [StaticConstructorOnStartup]
    public class Main
    {
        static Main()
        {
            var harmony = new Harmony("com.BattIePatch_IEDRemoteDetonation");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
