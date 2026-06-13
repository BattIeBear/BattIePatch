using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace BattIePatch_TendAgain
{
    [HarmonyPatch(typeof(Hediff), nameof(Hediff.TendableNow))]
    public class Hediff_TendableNow_Patch
    {
        public static bool Postfix(bool __result, Hediff __instance)
        {
            if (__result)
            {
                return __result;
            }

            if (__instance.IsTended() == false)
            {
                return __result;
            }

            if (__instance.pawn.IsAnimal && BattIePatchTendAgainSettings.tendAgainAnimals == false)
            {
                return __result;
            }

            if(__instance.pawn.Faction.IsPlayer == false)
            {
                Pawn_GuestTracker guestTracker = __instance.pawn.guest;
                if (guestTracker == null)
                {
                    return __result;
                }

                if (guestTracker.HostFaction == null || guestTracker.HostFaction.IsPlayer == false)
                {
                    return __result;
                }
            }

            if (__instance.pawn.GuestStatus == GuestStatus.Guest && BattIePatchTendAgainSettings.tendAgainGuests == false)
            {
                return __result;
            }

            if (__instance.pawn.GuestStatus == GuestStatus.Prisoner && BattIePatchTendAgainSettings.tendAgainPrisoners == false)
            {
                return __result;
            }

            if (__instance.pawn.GuestStatus == GuestStatus.Slave && BattIePatchTendAgainSettings.tendAgainSlaves == false)
            {
                return __result;
            }

            float quality = __instance.TryGetComp<HediffComp_TendDuration>()?.tendQuality ?? -1f;
            
            if (quality < 0f)
            {
                return __result;
            }

            if (__instance is Hediff_Injury)
            {
                if (BattIePatchTendAgainSettings.tendAgainCheck == false)
                {
                    return __result;
                }

                return quality <= Mathf.RoundToInt(BattIePatchTendAgainSettings.tendAgain * 100) / 100f;
            }
            else if (__instance.def == HediffDefOf.ScariaInfection || __instance.def == HediffDefOf.WoundInfection)
            {
                if (BattIePatchTendAgainSettings.tendAgainCheck_Infection == false)
                {
                    return __result;
                }

                return quality <= Mathf.RoundToInt(BattIePatchTendAgainSettings.tendAgain_Infection * 100) / 100f;
            }
            else
            {
                if (BattIePatchTendAgainSettings.tendAgainCheck_Disease == false)
                {
                    return __result;
                }

                return quality <= Mathf.RoundToInt(BattIePatchTendAgainSettings.tendAgain_Disease * 100) / 100f;
            }
        }
    }
}
