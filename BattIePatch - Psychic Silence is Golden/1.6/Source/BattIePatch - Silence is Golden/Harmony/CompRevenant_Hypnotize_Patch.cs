using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace BattIePatch_SilenceIsGolden
{
    [HarmonyPatch(typeof(CompRevenant), nameof(CompRevenant.Hypnotize))]
    class CompRevenant_Hypnotize_Patch
    {

        public static bool Prefix(CompRevenant __instance, ref Pawn victim)
        {
            if (!BattIePatchSilenceIsGoldenSettings.RevenantHypnosisImmunity)
            {
                return true;
            }

            Pawn Revenant = (Pawn)(__instance.parent);

            if (victim.GetStatValue(StatDefOf.PsychicSensitivity) <= 0)
            {
                if (!victim.Dead)
                {
                    Revenant.Drawer.renderer.SetAnimation(AnimationDefOf.RevenantSpasm);
                    Revenant.mindState.lastEngageTargetTick = Find.TickManager.TicksGame;
                    Revenant.mindState.enemyTarget = null;
                    __instance.nextHypnosis = Find.TickManager.TicksGame + Mathf.FloorToInt(RevenantUtility.SearchForTargetCooldownRangeDays.RandomInRange * 60000f);
                    if (PawnUtility.ShouldSendNotificationAbout(victim))
                    {
                        Find.LetterStack.ReceiveLetter("BattIePatch_SilenceIsGolden_LetterLabelPawnAlmostHypnotized".Translate(victim.Named("PAWN")), "BattIePatch_SilenceIsGolden_LetterPawnAlmostHypnotized".Translate(victim.Named("PAWN")), LetterDefOf.NeutralEvent, victim);
                    }

                    return false;
                }
            }

            return true;
        }
    }
}