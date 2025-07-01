using HarmonyLib;
using RimWorld;
using Verse;

namespace BattIePatch_SilenceIsGolden
{
    [HarmonyPatch(typeof(IncidentWorker_UnnaturalCorpseArrival), "ValidatePawn")]
    class IncidentWorker_UnnaturalCorpseArrival_ValidatePawn_Patch
    {
        public static bool Postfix(bool ___result, ref Pawn pawn)
        {
            if (___result && BattIePatchSilenceIsGoldenSettings.UnnaturalCorpseImmunity)
            {
                if (pawn.GetStatValue(StatDefOf.PsychicSensitivity) <= 0)
                {
                    return false;
                }
            }

            return ___result;
        }
    }
}
