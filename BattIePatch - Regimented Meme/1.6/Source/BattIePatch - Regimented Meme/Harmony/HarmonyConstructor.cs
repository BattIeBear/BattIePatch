using HarmonyLib;
using System.Reflection;
using Verse;

namespace BattIePatch_RegimentedMeme
{
    [StaticConstructorOnStartup]
    public class Main
    {
        static Main()
        {
            var harmony = new Harmony("com.BattIePatch_RegimentedMeme");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
