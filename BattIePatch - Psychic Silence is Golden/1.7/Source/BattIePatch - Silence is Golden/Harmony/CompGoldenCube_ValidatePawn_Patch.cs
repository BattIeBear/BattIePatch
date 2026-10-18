using HarmonyLib;
using RimWorld;
using Verse;

namespace BattIePatch_SilenceIsGolden
{
    [HarmonyPatch(typeof(CompGoldenCube), "ValidatePawn")]
    class CompGoldenCube_ValidatePawn_Patch
    {
        public static bool Postfix(bool ___result, ref Pawn pawn)
        {
            if (___result && BattIePatchSilenceIsGoldenSettings.GoldenCubeImmunity)
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
