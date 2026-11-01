using Verse;

namespace BattIePatch_ExtremeHemogenExtraction
{
    internal class BattIePatchExtremeHemogenExtractionSettings : ModSettings
    {
        /// Choices for Perfect Body gene graphics
        public static bool RequiresBloodfeedingMeme = true;
        public static bool UseHemogenImage = true;
        public static bool TransfuseOneNotStack = false;
        public static float amountToExtract = 0.35f;
        public static float amountToTransfuse = 0.3f;


        /// The part that writes our settings to file. Note that saving is by ref.
        public override void ExposeData()
        {
            Scribe_Values.Look(ref RequiresBloodfeedingMeme, "RequiresBloodfeedingMeme");
            Scribe_Values.Look(ref UseHemogenImage, "UseHemogenImage");
            Scribe_Values.Look(ref TransfuseOneNotStack, "TransfuseOneNotStack");
            base.ExposeData();
        }
    }
}