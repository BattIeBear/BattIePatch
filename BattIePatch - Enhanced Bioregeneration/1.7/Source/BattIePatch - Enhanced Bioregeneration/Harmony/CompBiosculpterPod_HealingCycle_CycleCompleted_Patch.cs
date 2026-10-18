using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using Verse;
namespace BattIePatch_EnhancedBioregeneration
{
    [HarmonyPatch(typeof(CompBiosculpterPod_HealingCycle), nameof(CompBiosculpterPod_HealingCycle.CycleCompleted))]
    public static class CompBiosculpterPod_HealingCycle_CycleCompleted_Patch
    {
        public static bool Prefix(ref CompBiosculpterPod_HealingCycle __instance, ref Pawn pawn, List<string> ___tmpWillHealHediffs, List<string> ___tmpCanHealHediffs, List<Hediff> ___tmpHediffs)
        {
            if (__instance.Regenerate == false)
            {
                return true;
            }
            if (BattIePatchEnhancedBioregenerationSettings.LessenedRegeneration || BattIePatchEnhancedBioregenerationSettings.HealAllConditions || BattIePatchEnhancedBioregenerationSettings.KeepMinorScars || BattIePatchEnhancedBioregenerationSettings.KeepAllScars)
            {
                HealingCycle_Utils.CycleCompleted(__instance, pawn, ___tmpHediffs);
                return false;
            }

            return true;
        }
    }
}