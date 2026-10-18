using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace BattIePatch_GhoulMoodBarFix
{
    [HarmonyPatch(typeof(ColonistBarColonistDrawer), nameof(ColonistBarColonistDrawer.DrawColonist))]
    public static class ColonistBarColonistDrawer_DrawColonist_Patch
    {
        public static bool Prefix(ColonistBarColonistDrawer __instance, ref Rect rect, ref Pawn colonist, ref Map pawnMap, ref bool highlight, ref bool reordering)
        {
            if (colonist.IsGhoul && colonist.needs?.food != null)
            {
                ColonistBarColonistRedrawer.RedrawColonist(__instance, rect, colonist, pawnMap, highlight, reordering);
                return false;
            }
            return true;
        }
    }
}