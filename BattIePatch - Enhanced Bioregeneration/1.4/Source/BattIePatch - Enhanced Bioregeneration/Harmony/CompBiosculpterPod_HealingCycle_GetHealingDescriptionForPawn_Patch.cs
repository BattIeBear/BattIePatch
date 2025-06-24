using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using Verse;
namespace BattIePatch_EnhancedBioregeneration
{
    [HarmonyPatch(typeof(CompBiosculpterPod_HealingCycle), nameof(CompBiosculpterPod_HealingCycle.GetHealingDescriptionForPawn))]
    public static class CompBiosculpterPod_HealingCycle_GetHealingDescriptionForPawn_Patch
    {
        public static bool Prefix(ref CompBiosculpterPod_HealingCycle __instance, ref Pawn pawn, ref string __result, List<string> ___tmpWillHealHediffs, List<string> ___tmpCanHealHediffs, List<Hediff> ___tmpHediffs)
        {
            if (__instance.Regenerate == false)
            {
                return true;
            }
            if (BattIePatchEnhancedBioregenerationSettings.LessenedRegeneration || BattIePatchEnhancedBioregenerationSettings.HealAllConditions || BattIePatchEnhancedBioregenerationSettings.KeepMinorScars || BattIePatchEnhancedBioregenerationSettings.KeepAllScars)
            {
                __result = HealingCycle_Utils.GetHealingDescriptionForPawn(__instance, pawn, ___tmpWillHealHediffs, ___tmpCanHealHediffs);
                return false;
            }

            return true;
        }
    }
}