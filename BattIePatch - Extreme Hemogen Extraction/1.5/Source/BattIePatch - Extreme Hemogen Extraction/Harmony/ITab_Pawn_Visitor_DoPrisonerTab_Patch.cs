using HarmonyLib;
using RimWorld;
using System.Linq;
using System.Reflection;
using Verse;

namespace BattIePatch_ExtremeHemogenExtraction
{
    [HarmonyPatch]
    public static class ITab_Pawn_Visitor_DoPrisonerTab_Patch
    {
        static MethodBase TargetMethod()
        {
            var closureType = typeof(ITab_Pawn_Visitor).GetNestedTypes(BindingFlags.NonPublic).FirstOrDefault(t => t.Name.Contains("DisplayClass8_0"));

            return closureType?.GetMethod("<DoPrisonerTab>b__3", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        static bool Prefix(object __instance, PrisonerInteractionModeDef m, ref bool __result)
        {
            if (m != PrisonerInteractionModeDefOf.battiepatch_ExtremeHemogenFarm)
            {
                return true;
            }

            var tab = Traverse.Create(__instance).Field("<>4__this").GetValue();
            if (tab == null)
            {
                Log.Error("Could not extract ITab_Pawn_Visitor from closure instance.");
                return true;
            }

            var pawn = Traverse.Create(tab).Property("SelPawn").GetValue() as Pawn;
            if (pawn == null)
            {
                Log.Error("Could not extract Pawn from ITab_Pawn_Visitor.");
                return true;
            }

            __result = CustomCanUsePrisonerInteractionMode(pawn, m);
            return false;
        }

        public static bool CustomCanUsePrisonerInteractionMode(Pawn pawn, PrisonerInteractionModeDef mode)
        {
            if (ModsConfig.IdeologyActive)
            {
                if (BattIePatchExtremeHemogenExtractionSettings.RequiresBloodfeedingMeme)
                {
                    if (Faction.OfPlayer.ideos.PrimaryIdeo != null)
                    {
                        if (!Faction.OfPlayer.ideos.PrimaryIdeo.memes.Any(meme => meme.defName == "Bloodfeeding"))
                        {
                            pawn.guest.ToggleNonExclusiveInteraction(PrisonerInteractionModeDefOf.battiepatch_ExtremeHemogenFarm, false);
                            return false;
                        }
                    }
                }
            }

            if (mode.hideOnHemogenicPawns && ModsConfig.BiotechActive && pawn.genes != null && pawn.genes.HasActiveGene(GeneDefOf.Hemogenic))
            {
                pawn.guest.ToggleNonExclusiveInteraction(PrisonerInteractionModeDefOf.battiepatch_ExtremeHemogenFarm, false);
                return false;
            }

            return true;
        }
    }
}