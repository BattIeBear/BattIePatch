﻿using HarmonyLib;
using RimWorld;
using Verse;

namespace BattIePatch_SilenceIsGolden
{
    [HarmonyPatch(typeof(RevenantUtility), nameof(RevenantUtility.ValidTarget))]
    class RevenantUtility_ValidTarget_Patch
    {
        public static bool Postfix(bool ___result, ref Pawn pawn)
        {
            if (___result && BattIePatchSilenceIsGoldenSettings.RevenantHypnosisTrueImmunity)
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