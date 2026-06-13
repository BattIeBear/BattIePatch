using Verse;

namespace BattIePatch_TendAgain
{
    public class BattIePatchTendAgainSettings : ModSettings
    {
        ///Drawbacks
        public static bool tendAgainCheck = true;
        public static float tendAgain = .10f;
        public static bool tendAgainCheck_Disease = true;
        public static float tendAgain_Disease = .10f;
        public static bool tendAgainCheck_Infection = true;
        public static float tendAgain_Infection = .10f;
        public static bool tendAgainGuests = true;
        public static bool tendAgainPrisoners = true;
        public static bool tendAgainSlaves = true;
        public static bool tendAgainAnimals = true;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref tendAgainCheck, "tendAgainCheck", true);
            Scribe_Values.Look(ref tendAgain, "tendAgain", .10f);
            Scribe_Values.Look(ref tendAgainCheck_Disease, "tendAgainCheck_Disease", true);
            Scribe_Values.Look(ref tendAgain_Disease, "tendAgain_Disease", .10f);
            Scribe_Values.Look(ref tendAgainCheck_Infection, "tendAgainCheck_Infection", true);
            Scribe_Values.Look(ref tendAgain_Infection, "tendAgain_Infection", .10f);
            Scribe_Values.Look(ref tendAgainGuests, "tendAgainGuests", true);
            Scribe_Values.Look(ref tendAgainPrisoners, "tendAgainPrisoners", true);
            Scribe_Values.Look(ref tendAgainSlaves, "tendAgainSlaves", true);
            Scribe_Values.Look(ref tendAgainAnimals, "tendAgainAnimals", true);
            base.ExposeData();
        }
    }
}