using HarmonyLib;
using RimWorld;
using System.Collections.Generic;

namespace BattlePatch_EnhancedAgeReversal
{
    [HarmonyPatch(typeof(CompBiosculpterPod_AgeReversalCycle), nameof(CompBiosculpterPod_AgeReversalCycle.CycleCompleted))]
    public static class CompBiosculpterPod_AgeReversalCycle_CycleCompletedPatch
    {
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            codes[0].operand = (float)codes[0].operand * BattlePatch_EnhancedAgeReversal_Settings.AgeReversalYears;
            codes.RemoveRange(45, 28);

            return codes;
        }
    }
}