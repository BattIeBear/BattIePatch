using UnityEngine;
using Verse;

namespace BattlePatch_EnhancedAgeReversal
{
    internal class BattlePatch_EnhancedAgeReversal_SettingsWindow : Mod
    {
        /// A reference to our settings.
        BattlePatch_EnhancedAgeReversal_Settings settings;

        /// A mandatory constructor which resolves the reference to our settings.
        public BattlePatch_EnhancedAgeReversal_SettingsWindow(ModContentPack content) : base(content)
        {
            this.settings = GetSettings<BattlePatch_EnhancedAgeReversal_Settings>();
        }

        /// The (optional) GUI part to set your settings.
        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);

            listingStandard.Label("BattlePatch_EnhancedAgeReversal_TopWarning".Translate());

            listingStandard.GapLine();

            listingStandard.Gap();
            listingStandard.Gap();
            listingStandard.Gap();


            BattlePatch_EnhancedAgeReversal_Settings.AgeReversalYears = Mathf.RoundToInt(listingStandard.SliderLabeled("BattlePatch_EnhancedAgeReversal_RangeSlider".Translate() + (BattlePatch_EnhancedAgeReversal_Settings.AgeReversalYears).ToString(),
                BattlePatch_EnhancedAgeReversal_Settings.AgeReversalYears, 1, 10, tooltip: "BattlePatch_EnhancedAgeReversal_RangeSliderDesc".Translate()
            ));

            listingStandard.Gap();

            listingStandard.GapLine();

            listingStandard.Label("BattlePatch_EnhancedAgeReversal_BottomWarning".Translate());

            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        /// Override SettingsCategory to show up in the list of settings.
        /// Using .Translate() is optional, but does allow for localization.
        public override string SettingsCategory()
        {
            return "BattlePatch_EnhancedAgeReversal_ModName".Translate();
        }
    }
}
