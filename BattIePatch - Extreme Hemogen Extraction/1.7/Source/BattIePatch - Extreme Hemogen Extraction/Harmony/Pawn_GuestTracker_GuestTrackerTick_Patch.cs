using HarmonyLib;
using RimWorld;
using Verse;

namespace BattIePatch_ExtremeHemogenExtraction
{
    [HarmonyPatch(typeof(Pawn_GuestTracker), nameof(Pawn_GuestTracker.GuestTrackerTickInterval))]
    public class Pawn_GuestTracker_GuestTrackerTick_Patch
    {
        public static void Postfix(Pawn_GuestTracker __instance, ref Pawn ___pawn, ref int delta)
        {

            if (ModsConfig.BiotechActive && ___pawn.IsHashIntervalTick(1800, delta) && SanguophageUtility.CanSafelyBeQueuedForHemogenExtraction(___pawn) && __instance.guestStatusInt == GuestStatus.Prisoner && __instance.IsInteractionEnabled(RimWorld.PrisonerInteractionModeDefOf.HemogenFarm) && __instance.IsInteractionEnabled(PrisonerInteractionModeDefOf.battiepatch_ExtremeHemogenFarm))
            {
                HealthCardUtility.CreateSurgeryBill(___pawn, RecipeDefOf.ExtractHemogenPack, null, null, sendMessages: false);
            }
        }
    }
}
