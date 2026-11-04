using Verse;

namespace BattlePatch_EnhancedAgeReversal
{
    internal class BattlePatch_EnhancedAgeReversal_Settings : ModSettings
    {

        public static int AgeReversalYears = 2; // Default range for detonation

        /// The part that writes our settings to file. Note that saving is by ref.
        public override void ExposeData()
        {
            Scribe_Values.Look(ref AgeReversalYears, "AgeReversalYears", 2);
            base.ExposeData();
        }
    }
}
