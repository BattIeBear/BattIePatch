using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace BattIePatch_ExtremeHemogenExtraction
{
    [HarmonyPatch(typeof(Recipe_BloodTransfusion), "ApplyOnPawn")]
    public class Recipe_BloodTransfusion_ApplyOnPawn_Patch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var hemogenField = AccessTools.Field(typeof(BattIePatchExtremeHemogenExtractionSettings), nameof(BattIePatchExtremeHemogenExtractionSettings.amountToTransfuse));
            foreach (var instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Ldc_R4 && instruction.operand is float f && f == 0.35f)
                {
                    yield return new CodeInstruction(OpCodes.Ldsfld, hemogenField);
                }
                else
                {
                    yield return instruction;
                }
            }
        }
    }
}
