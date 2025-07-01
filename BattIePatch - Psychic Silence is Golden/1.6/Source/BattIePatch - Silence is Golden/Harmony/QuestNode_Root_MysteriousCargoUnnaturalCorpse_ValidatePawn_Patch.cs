using HarmonyLib;
using RimWorld;
using RimWorld.QuestGen;
using Verse;

namespace BattIePatch_SilenceIsGolden
{
    [HarmonyPatch(typeof(QuestNode_Root_MysteriousCargoUnnaturalCorpse), "ValidatePawn")]
    class QuestNode_Root_MysteriousCargoUnnaturalCorpse_ValidatePawn_Patch
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
