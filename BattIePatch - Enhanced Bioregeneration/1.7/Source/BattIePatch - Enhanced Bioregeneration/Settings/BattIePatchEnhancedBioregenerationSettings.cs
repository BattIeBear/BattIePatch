using Verse;

namespace BattIePatch_EnhancedBioregeneration
{
    public class BattIePatchEnhancedBioregenerationSettings : ModSettings
    {
        // Only heals 1 body part at a time
        public static bool LessenedRegeneration = false;
        // Heals all minor conditions in one cycle
        public static bool HealAllConditions = false;
        // Only keeps minor, external scars
        public static bool KeepMinorScars = true;
        // Keeps all scars, regardless of severity or location
        public static bool KeepAllScars = false;
        // Default time is increased form 25 to 30
        public static bool UseExtraTime = true;
        // Medicine count is increased from 2 to 4
        public static bool UseExtraMeds = true;

        // The part that writes our settings to file. Note that saving is by ref.
        public override void ExposeData()
        {
            Scribe_Values.Look(ref LessenedRegeneration, "LessenedRegeneration");
            Scribe_Values.Look(ref HealAllConditions, "HealAllConditions");
            Scribe_Values.Look(ref KeepMinorScars, "KeepMinorScars");
            Scribe_Values.Look(ref KeepAllScars, "KeepAllScars");
            Scribe_Values.Look(ref UseExtraTime, "UseExtraTime");
            Scribe_Values.Look(ref UseExtraMeds, "UseExtraMeds");
            base.ExposeData();
        }
    }
}