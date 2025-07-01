using Verse;

namespace BattIePatch_SilenceIsGolden
{
    class BattIePatchSilenceIsGoldenSettings : ModSettings
    {
        /// allows for extra hair and skin colors
        public static bool GoldenCubeImmunity = true;
        public static bool UnnaturalCorpseImmunity = true;
        public static bool RevenantHypnosisImmunity = true;



        /// The part that writes our settings to file. Note that saving is by ref.
        public override void ExposeData()
        {
            Scribe_Values.Look(ref GoldenCubeImmunity, "GoldenCubeImmunity");
            Scribe_Values.Look(ref UnnaturalCorpseImmunity, "UnnaturalCorpseImmunity");
            Scribe_Values.Look(ref RevenantHypnosisImmunity, "RevenantHypnosisImmunity");
            base.ExposeData();
        }
    }
}
