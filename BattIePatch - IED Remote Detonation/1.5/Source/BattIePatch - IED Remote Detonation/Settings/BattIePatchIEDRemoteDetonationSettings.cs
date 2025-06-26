using Verse;

namespace BattIePatch_IEDRemoteDetonation
{
    internal class BattIePatchIEDRemoteDetonationSettings : ModSettings
    {
        /// Choices for Perfect Body gene graphics
        public static bool DraftedDetonation = true;
        public static bool MoveToSafeDistance = true; // Whether to move to a safe distance before detonation
        public static float DraftedDetonationMaxRange = 2f; // Default range for detonation

        /// The part that writes our settings to file. Note that saving is by ref.
        public override void ExposeData()
        {
            Scribe_Values.Look(ref DraftedDetonation, "DraftedDetonation", true);
            Scribe_Values.Look(ref MoveToSafeDistance, "MoveToSafeDistance", true);
            Scribe_Values.Look(ref DraftedDetonationMaxRange, "DraftedDetonationRange", 2f);
            base.ExposeData();
        }
    }
}