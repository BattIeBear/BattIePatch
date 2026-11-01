using HarmonyLib;
using RimWorld;
using System.Linq;
using Verse;

namespace BattIePatch_ExtremeHemogenExtraction
{
    [HarmonyPatch(typeof(SanguophageUtility), nameof(SanguophageUtility.CanSafelyBeQueuedForHemogenExtraction))]
    public class SanguophageUtility_CanSafelyBeQueuedForHemogenExtraction_Patch
    {
        public static bool Postfix(bool ___result, ref Pawn pawn)
        {
            if (___result == false && pawn.guest.IsInteractionEnabled(PrisonerInteractionModeDefOf.battiepatch_ExtremeHemogenFarm))
            {
                if (ModsConfig.BiotechActive && pawn.Spawned && pawn.BillStack != null && !pawn.BillStack.Bills.Any((Bill x) => x.recipe == RecipeDefOf.ExtractHemogenPack) && RecipeDefOf.ExtractHemogenPack.Worker.AvailableOnNow(pawn))
                {
                    Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.BloodLoss);
                    if (firstHediffOfDef != null)
                    {
                        Log.Message(firstHediffOfDef.Severity < 0.700f);
                        return firstHediffOfDef.Severity < 0.700f;
                    }
                }
            }

            return ___result;
        }
    }
}
