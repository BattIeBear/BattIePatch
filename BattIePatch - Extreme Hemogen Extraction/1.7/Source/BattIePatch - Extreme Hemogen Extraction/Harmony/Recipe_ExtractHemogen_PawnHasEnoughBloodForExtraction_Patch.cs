using HarmonyLib;
using RimWorld;
using Verse;

namespace BattIePatch_ExtremeHemogenExtraction
{
    [HarmonyPatch(typeof(Recipe_ExtractHemogen), "PawnHasEnoughBloodForExtraction")]
    public class Recipe_ExtractHemogen_PawnHasEnoughBloodForExtraction_Patch
    {
        public static bool Postfix(bool ___result, ref Pawn pawn)
        {
            if (___result == false && pawn.guest.IsInteractionEnabled(PrisonerInteractionModeDefOf.battiepatch_ExtremeHemogenFarm))
            {
                Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.BloodLoss);
                if (firstHediffOfDef != null)
                {
                    return firstHediffOfDef.Severity < 1.0f - BattIePatchExtremeHemogenExtractionSettings.amountToExtract;
                }
            }

            return ___result;
        }
    }
}
