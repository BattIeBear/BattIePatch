using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace BattIePatch_ExtremeHemogenExtraction
{
    [HarmonyPatch(typeof(Recipe_ExtractHemogen), "ApplyOnPawn")]
    public class Recipe_ExtractHemogen_ApplyOnPawn_Patch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var hemogenField = AccessTools.Field(typeof(BattIePatchExtremeHemogenExtractionSettings), nameof(BattIePatchExtremeHemogenExtractionSettings.amountToExtract));
            foreach (var instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Ldc_R4 && instruction.operand is float f && f == 0.45f)
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
