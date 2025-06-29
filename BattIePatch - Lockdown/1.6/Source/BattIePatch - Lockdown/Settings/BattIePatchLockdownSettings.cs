using Verse;

namespace BattIePatch_Lockdown
{
    internal class BattIePatchLockdownSettings : ModSettings
    {
        /// Choices for Perfect Body gene graphics
        public static bool OverridesHoldOpen = true;


        /// The part that writes our settings to file. Note that saving is by ref.
        public override void ExposeData()
        {
            Scribe_Values.Look(ref OverridesHoldOpen, "OverridesHoldOpen");
            base.ExposeData();
        }
    }
}