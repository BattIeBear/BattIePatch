using RimWorld;
using Verse;

namespace BattIePatch_GhoulMoodBarFix
{
    public static class FoodToMood
    {
        public static MoodThreshold Convert(Pawn pawn)
        {
            if (pawn.needs.food.CurCategory == HungerCategory.UrgentlyHungry || pawn.needs.food.CurCategory == HungerCategory.Starving)
            {
                return MoodThreshold.Extreme;
            }
            else if (pawn.needs.food.CurLevelPercentage <= .25f)
            {
                return MoodThreshold.Major;
            }
            else if (pawn.needs.food.CurLevelPercentage <= .5f)
            {
                return MoodThreshold.Minor;
            }

            return MoodThreshold.None;
        }
    }
}