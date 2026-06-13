using UnityEngine;
using Verse;

namespace BattIePatch_TendAgain
{
    public class BattIePatchTendAgainSettingsWindow : Mod
    {
        /// A reference to our settings.
        BattIePatchTendAgainSettings settings;

        /// A mandatory constructor which resolves the reference to our settings.
        public BattIePatchTendAgainSettingsWindow(ModContentPack content) : base(content)
        {
            this.settings = GetSettings<BattIePatchTendAgainSettings>();
        }

        /// The (optional) GUI part to set your settings.
        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            listingStandard.Label("BattIePatch_TendAgain_TopWarning".Translate());

            listingStandard.GapLine();

            listingStandard.Label("BattIePatch_TendAgain_Label1".Translate());
            listingStandard.CheckboxLabeled("BattIePatch_TendAgain_TendAgainCheck".Translate(), ref BattIePatchTendAgainSettings.tendAgainCheck, tooltip: "BattIePatch_TendAgain_TendAgainCheckDesc".Translate());
            BattIePatchTendAgainSettings.tendAgain = listingStandard.SliderLabeled("BattIePatch_TendAgain_TendAgain".Translate() + Mathf.RoundToInt(BattIePatchTendAgainSettings.tendAgain * 100f) + "%", BattIePatchTendAgainSettings.tendAgain, 0f, 1f, tooltip: "BattIePatch_TendAgain_TendAgainDesc".Translate());

            listingStandard.CheckboxLabeled("BattIePatch_TendAgain_TendAgainCheckDisease".Translate(), ref BattIePatchTendAgainSettings.tendAgainCheck_Disease, tooltip: "BattIePatch_TendAgain_TendAgainCheckDiseaseDesc".Translate());
            BattIePatchTendAgainSettings.tendAgain_Disease = listingStandard.SliderLabeled("BattIePatch_TendAgain_TendAgainDisease".Translate() + Mathf.RoundToInt(BattIePatchTendAgainSettings.tendAgain_Disease * 100f) + "%", BattIePatchTendAgainSettings.tendAgain_Disease, 0f, 1f, tooltip: "BattIePatch_TendAgain_TendAgainDiseaseDesc".Translate());

            listingStandard.CheckboxLabeled("BattIePatch_TendAgain_TendAgainCheckInfection".Translate(), ref BattIePatchTendAgainSettings.tendAgainCheck_Infection, tooltip: "BattIePatch_TendAgain_TendAgainCheckInfectionDesc".Translate());
            BattIePatchTendAgainSettings.tendAgain_Infection = listingStandard.SliderLabeled("BattIePatch_TendAgain_TendAgainInfection".Translate() + Mathf.RoundToInt(BattIePatchTendAgainSettings.tendAgain_Infection * 100f) + "%", BattIePatchTendAgainSettings.tendAgain_Infection , 0f, 1f, tooltip: "BattIePatch_TendAgain_TendAgainInfectionDesc".Translate());

            listingStandard.GapLine();

            listingStandard.Label("BattIePatch_TendAgain_Label2".Translate());
            listingStandard.CheckboxLabeled("BattIePatch_TendAgain_TendAgainGuests".Translate(), ref BattIePatchTendAgainSettings.tendAgainGuests, tooltip: "BattIePatch_TendAgain_TendAgainGuestsDesc".Translate());
            listingStandard.CheckboxLabeled("BattIePatch_TendAgain_TendAgainPrisoners".Translate(), ref BattIePatchTendAgainSettings.tendAgainPrisoners, tooltip: "BattIePatch_TendAgain_TendAgainPrisonersDesc".Translate());
            listingStandard.CheckboxLabeled("BattIePatch_TendAgain_TendAgainSlaves".Translate(), ref BattIePatchTendAgainSettings.tendAgainSlaves, tooltip: "BattIePatch_TendAgain_TendAgainSlavesDesc".Translate());
            listingStandard.CheckboxLabeled("BattIePatch_TendAgain_TendAgainAnimals".Translate(), ref BattIePatchTendAgainSettings.tendAgainAnimals, tooltip: "BattIePatch_TendAgain_TendAgainAnimalsDesc".Translate());

            listingStandard.GapLine();

            listingStandard.Label("BattIePatch_TendAgain_BottomWarning".Translate());
            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        /// Override SettingsCategory to show up in the list of settings.
        /// Using .Translate() is optional, but does allow for localization.
        public override string SettingsCategory()
        {
            return "BattIePatch_TendAgain_ModName".Translate();
        }
    }
}