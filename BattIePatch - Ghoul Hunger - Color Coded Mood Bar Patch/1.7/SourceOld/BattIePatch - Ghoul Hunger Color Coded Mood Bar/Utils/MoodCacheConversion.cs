using System.Collections.Generic;
using ColoredMoodBar13;
using RimWorld;
using UnityEngine;
using Verse;

namespace BattIePatch_GhoulHungerColorCodedMoodBarPatch
{
    public static class MoodCacheConversion
    {
        private static int ThresholdUpdateCounterMax = 0;
        private static Dictionary<int, int> PawnIDToCounter = new Dictionary<int, int>();

        public static void RedoMoodAsFood(ref Pawn colonist, ref Rect rect, ref MoodCache __instance)
        {
            if (ThresholdUpdateCounterMax == 0)
            {
                if (ColoredMoodBar13.Main.Settings.RefreshRate > 500)
                {
                    ThresholdUpdateCounterMax = 5;
                }
                else
                {
                    ThresholdUpdateCounterMax = 10;
                }
            }
            int PawnID = ((Thing)colonist).thingIDNumber;
            if (PawnIDToCounter.ContainsKey(PawnID) == false)
            {
                PawnIDToCounter.Add(PawnID, ThresholdUpdateCounterMax+1);
            }
            int ThresholdUpdateCounter = PawnIDToCounter[PawnID];

            if (colonist.needs != null && colonist.needs.food != null)
            {
                
                __instance.HasMood = true;
                __instance.CurrentMoodLevelPercent = ((Need)colonist.needs.food).CurLevelPercentage;
                __instance.Position = GenUI.ContractedBy(rect, 2f);
                float num = ((Rect)(__instance.Position)).height * __instance.CurrentMoodLevelPercent;
                __instance.Position.yMin = ((Rect)(__instance.Position)).yMax - num;
                __instance.Position.height = num;

                //counter
                if (ThresholdUpdateCounter > ThresholdUpdateCounterMax)
                {
                    PawnIDToCounter[PawnID] = 0;
                }
                else
                {
                    PawnIDToCounter[PawnID]++;
                    return;
                }

                if (__instance.CurrentMoodLevelPercent <= .10f)
                {
                    __instance.MoodLevel = MoodLevel.Extreme;
                    __instance.MoodTexture = ColoredMoodBar13.Main.extremeBreakTex;
                    __instance.MoodColor = ColoredMoodBar13.Main.Settings.Extreme;
                }
                else if (__instance.CurrentMoodLevelPercent <= .25f)
                {
                    __instance.MoodLevel = MoodLevel.Major;
                    __instance.MoodTexture = ColoredMoodBar13.Main.majorBreakTex;
                    __instance.MoodColor = ColoredMoodBar13.Main.Settings.Major;
                }
                else if (__instance.CurrentMoodLevelPercent <= .50f)
                {
                    __instance.MoodLevel = MoodLevel.Minor;
                    __instance.MoodTexture = ColoredMoodBar13.Main.minorBreakTex;
                    __instance.MoodColor = ColoredMoodBar13.Main.Settings.Minor;
                }
                else if (__instance.CurrentMoodLevelPercent <= 0.65f)
                {
                    __instance.MoodLevel = MoodLevel.Neutral;
                    __instance.MoodTexture = ColoredMoodBar13.Main.neutralTex;
                    __instance.MoodColor = ColoredMoodBar13.Main.Settings.Neutral;
                }
                else if (__instance.CurrentMoodLevelPercent <= 0.9f)
                {
                    __instance.MoodLevel = MoodLevel.Content;
                    __instance.MoodTexture = ColoredMoodBar13.Main.contentTex;
                    __instance.MoodColor = ColoredMoodBar13.Main.Settings.Content;
                }
                else
                {
                    __instance.MoodLevel = MoodLevel.Happy;
                    __instance.MoodTexture = ColoredMoodBar13.Main.happyTex;
                    __instance.MoodColor = ColoredMoodBar13.Main.Settings.Happy;
                }
            }
            else
            {
                __instance.HasMood = false;
                __instance.MoodLevel = MoodLevel.Neutral;
                __instance.MoodTexture = ColoredMoodBar13.Main.neutralTex;
            }
        }
    }
}