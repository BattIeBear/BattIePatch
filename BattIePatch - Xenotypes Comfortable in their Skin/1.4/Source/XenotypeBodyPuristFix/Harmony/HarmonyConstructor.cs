using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace XenotypeBodyPuristFix
{
    //Setting the Harmony instance
    [StaticConstructorOnStartup]
    public class Main
    {
        static Main()
        {
            var harmony = new Harmony("com.XenotypeBodyPuristFix");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}