using UnityEngine;
using Verse;

namespace BattIePatch_ExtremeHemogenExtraction
{
    internal class BattIePatchExtremeHemogenExtractionSettingsWindow : Mod
    {
        /// A reference to our settings.
        BattIePatchExtremeHemogenExtractionSettings settings;

        /// A mandatory constructor which resolves the reference to our settings.
        public BattIePatchExtremeHemogenExtractionSettingsWindow(ModContentPack content) : base(content)
        {
            this.settings = GetSettings<BattIePatchExtremeHemogenExtractionSettings>();
        }

        /// The (optional) GUI part to set your settings.
        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);

            listingStandard.Label("BattIePatch_ExtremeHemogenExtraction_TopWarning".Translate());

            listingStandard.GapLine();

            listingStandard.Gap();
            listingStandard.Gap();
            listingStandard.Gap();

            listingStandard.Label("BattIePatch_ExtremeHemogenExtraction_Label1".Translate());
            listingStandard.CheckboxLabeled("BattIePatch_ExtremeHemogenExtraction_RequiresBloodfeedingMeme".Translate(), ref BattIePatchExtremeHemogenExtractionSettings.RequiresBloodfeedingMeme, "".Translate());

            listingStandard.Gap();
            listingStandard.GapLine();
            listingStandard.Gap();

            listingStandard.Label("BattIePatch_ExtremeHemogenExtraction_Label2".Translate()); 
            listingStandard.CheckboxLabeled("BattIePatch_ExtremeHemogenExtraction_UseHemogenImage".Translate(), ref BattIePatchExtremeHemogenExtractionSettings.UseHemogenImage, "BattIePatch_ExtremeHemogenExtraction_UseHemogenImageDesc".Translate());
            listingStandard.CheckboxLabeled("BattIePatch_ExtremeHemogenExtraction_TransfuseOneNotStack".Translate(), ref BattIePatchExtremeHemogenExtractionSettings.TransfuseOneNotStack, "BattIePatch_ExtremeHemogenExtraction_TransfuseOneNotStackDesc".Translate());

            listingStandard.Gap();

            listingStandard.GapLine();

            listingStandard.Label("BattIePatch_ExtremeHemogenExtraction_BottomWarning".Translate());

            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        /// Override SettingsCategory to show up in the list of settings.
        /// Using .Translate() is optional, but does allow for localization.
        public override string SettingsCategory()
        {
            return "BattIePatch_ExtremeHemogenExtraction_ModName".Translate();
        }
    }
}