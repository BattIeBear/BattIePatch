using HarmonyLib;
using RimWorld;
using System.Collections.Generic;

namespace BattlePatch_EnhancedAgeReversal
{
    [HarmonyPatch(typeof(CompBiosculpterPod_AgeReversalCycle), nameof(CompBiosculpterPod_AgeReversalCycle.Description))]
    public static class CompBiosculpterPod_AgeReversalCycle_DescriptionPatch
    {
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            for (int i = 0; i < codes.Count; i++)
            {
                if (i == 0)
                {
                    codes[i].operand = (int)codes[i].operand * BattlePatch_EnhancedAgeReversal_Settings.AgeReversalYears;
                }
            }
            return codes;
        }
    }
}