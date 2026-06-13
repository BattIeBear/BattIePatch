using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace BattIePatch_TendAgain
{
    [HarmonyPatch(typeof(HediffComp_TendDuration), nameof(HediffComp_TendDuration.CompTended))]
    public class HediffComp_TendDuration_CompTended_Patch
    {
        public static bool Prefix(HediffComp_TendDuration __instance, ref float quality, ref float maxQuality, ref int batchPosition)
        {
            if (__instance != null)
            {
                if (__instance.parent.pawn.Faction.IsPlayer)
                {
                    if (__instance.parent.IsTended())
                    {
                        if (!__instance.AllowTend)
                        {
                            __instance.tendQuality = Mathf.Clamp(quality + Rand.Range(-0.25f, 0.25f), 0f, maxQuality);
                            float totalTendQuality = Traverse.Create(__instance).Field("totalTendQuality").GetValue<float>();
                            totalTendQuality += __instance.tendQuality;
                            Traverse.Create(__instance).Field("totalTendQuality").SetValue(totalTendQuality);
                            //if (__instance.TProps.TendIsPermanent)
                            //{
                            //    __instance.tendTicksLeft = 1;
                            //}
                            //else
                            //{
                            //    __instance.tendTicksLeft = Mathf.Max(0, __instance.tendTicksLeft) + __instance.TProps.TendTicksFull;
                            //}
                            if (batchPosition == 0 && __instance.Pawn.Spawned)
                            {
                                string text = "TextMote_Tended".Translate(__instance.parent.Label).CapitalizeFirst() + "\n" + "Quality".Translate() + " " + __instance.tendQuality.ToStringPercent();
                                MoteMaker.ThrowText(__instance.Pawn.DrawPos, __instance.Pawn.Map, text, Color.white, 3.65f);
                            }
                            __instance.Pawn.health.Notify_HediffChanged(__instance.parent);
                            return false;
                        }
                    }
                }
            }
            return true;
        }
    }
}