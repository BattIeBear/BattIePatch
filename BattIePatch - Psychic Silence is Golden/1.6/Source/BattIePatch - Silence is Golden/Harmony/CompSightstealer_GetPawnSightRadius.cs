using HarmonyLib;
using RimWorld;
using Verse;

namespace BattIePatch_SilenceIsGolden
{
    [HarmonyPatch(typeof(CompSightstealer), "GetPawnSightRadius")]
    public class CompSightstealer_GetPawnSightRadius
    {
        public static void Postfix(ref float __result, Pawn pawn)
        {
            if(BattIePatchSilenceIsGoldenSettings.SightstealerImmunity == false)
            {
                return;
            }
            if (pawn.GetStatValue(StatDefOf.PsychicSensitivity) <= 0f)
            {
                __result *= 2f;
            }
        }
    }
}