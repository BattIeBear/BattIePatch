using HarmonyLib;
using RimWorld;
using Verse;

namespace BattIePatch_TendAgain
{
    [HarmonyPatch(typeof(HediffComp_TendDuration), nameof(HediffComp_TendDuration.CompTipStringExtra), MethodType.Getter)]
    public class HediffComp_TendDuration_CompTipStringExtra_Patch
    {
        static void Postfix(ref string __result, HediffComp_TendDuration __instance)
        {
            if (__result == null)
            {
                return;
            }
            if (__result == "")
            {
                return;
            }
            if (__instance.IsTended)
            {
                if (!__instance.Pawn.Dead && __instance.parent.TendableNow())
                {
                    if (!__instance.Pawn.Dead && !__instance.TProps.TendIsPermanent)
                    {
                        int num = __instance.tendTicksLeft - __instance.TProps.TendTicksOverlap;
                        if (num < 0)
                        {
                            return;
                        }
                        else if ("NextTendIn".CanTranslate())
                        {
                            __result = __result.Replace("NextTendIn".Translate(num.ToStringTicksToPeriod()), "BattIePatch_TendAgain_Append".Translate());
                        }
                        else
                        {
                            __result = __result.Replace("NextTreatmentIn".Translate(num.ToStringTicksToPeriod()), "BattIePatch_TreatAgain_Append".Translate());
                        }
                    }
                }
            }
        }
    }
}