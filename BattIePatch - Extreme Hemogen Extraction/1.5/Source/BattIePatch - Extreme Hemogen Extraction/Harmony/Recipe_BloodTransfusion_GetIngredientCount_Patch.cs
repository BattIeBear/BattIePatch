using HarmonyLib;
using RimWorld;
using System.Linq;
using UnityEngine;
using Verse;

namespace BattIePatch_ExtremeHemogenExtraction
{

    [HarmonyPatch(typeof(Recipe_BloodTransfusion), nameof(Recipe_BloodTransfusion.GetIngredientCount))]
    public class Recipe_BloodTransfusion_GetIngredientCount_Patch
    {
        public static bool Prefix(ref float __result, Recipe_BloodTransfusion __instance, ref IngredientCount ing, ref Bill bill)
        {
            if (BattIePatchExtremeHemogenExtractionSettings.TransfuseOneNotStack)
            {
                __result = 1.0f;
                return false;
            }

            if (!(bill.billStack?.billGiver is Pawn pawn))
            {
                __result = __instance.GetIngredientCount(ing, bill);
                return false;
            }

            Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.BloodLoss);
            if (firstHediffOfDef == null)
            {
                __result = __instance.GetIngredientCount(ing, bill);
                return false;
            }

            __result =  Mathf.Min(bill.Map.listerThings.ThingsOfDef(ThingDefOf.HemogenPack).Sum((Thing x) => x.stackCount), firstHediffOfDef.Severity / BattIePatchExtremeHemogenExtractionSettings.amountToTransfuse);
            return false;
        }
    }
}
