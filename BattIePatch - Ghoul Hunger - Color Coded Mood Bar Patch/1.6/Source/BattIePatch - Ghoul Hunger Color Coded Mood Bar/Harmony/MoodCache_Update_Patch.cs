using ColoredMoodBar13;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace BattIePatch_GhoulHungerColorCodedMoodBarPatch
{
    [HarmonyPatch(typeof(MoodCache), "DoMood")]
    public static class MoodCache_Update_Patch
    {
        public static bool Prefix(ref Pawn colonist, ref Rect rect, ref MoodCache __instance)
        {
            if (colonist.IsGhoul && colonist.needs?.food != null)
            {
                MoodCacheConversion.RedoMoodAsFood(ref colonist, ref rect, ref __instance);
                return false;
            }

            return true;
        }
    }
}
