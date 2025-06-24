using HarmonyLib;
using RimWorld;
using Verse;

namespace XenotypeBodyPuristFix
{
    [HarmonyPatch(typeof(GeneUtility), "AddedAndImplantedPartsWithXenogenesCount")]
    public static class AddedAndImplantedPartsWithXenogenesCount_Patch
    {
        [HarmonyPrefix]
        public static bool AddedAndImplantedPartsCount(Pawn pawn, ref int __result)
        {
            int num = pawn.health.hediffSet.CountAddedAndImplantedParts();
            if (ModsConfig.BiotechActive && pawn.genes.UniqueXenotype)
            {
                if (XenotypeUtils.customXenotypesLoaded.Contains(pawn.genes.XenotypeLabel) == false)
                {
                    num++;
                }
            }
            __result = num;
            return false;
        }
    }
}
 