using UnityEngine;
using Verse;

namespace BattIePatch_IEDRemoteDetonation
{
    internal class BattIePatchIEDRemoteDetonationSettingsWindow : Mod
    {
        /// A reference to our settings.
        BattIePatchIEDRemoteDetonationSettings settings;

        /// A mandatory constructor which resolves the reference to our settings.
        public BattIePatchIEDRemoteDetonationSettingsWindow(ModContentPack content) : base(content)
        {
            this.settings = GetSettings<BattIePatchIEDRemoteDetonationSettings>();
        }

        /// The (optional) GUI part to set your settings.
        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);

            listingStandard.Label("BattIePatch_IEDRemoteDetonation_TopWarning".Translate());

            listingStandard.GapLine();

            listingStandard.Gap();
            listingStandard.Gap();
            listingStandard.Gap();

            listingStandard.CheckboxLabeled("BattIePatch_IEDRemoteDetonation_DraftedDetonation".Translate(), ref BattIePatchIEDRemoteDetonationSettings.DraftedDetonation, "BattIePatch_IEDRemoteDetonation_DraftedDetonationDesc".Translate());
            listingStandard.CheckboxLabeled("BattIePatch_IEDRemoteDetonation_SafeDistance".Translate(), ref BattIePatchIEDRemoteDetonationSettings.MoveToSafeDistance, "BattIePatch_IEDRemoteDetonation_SafeDistanceDesc".Translate());
            BattIePatchIEDRemoteDetonationSettings.DraftedDetonationMaxRange = Mathf.Round(listingStandard.SliderLabeled("BattIePatch_IEDRemoteDetonation_RangeSlider".Translate() + (BattIePatchIEDRemoteDetonationSettings.DraftedDetonationMaxRange).ToString(),
                BattIePatchIEDRemoteDetonationSettings.DraftedDetonationMaxRange, 1f, 24f, tooltip: "BattIePatch_IEDRemoteDetonation_RangeSliderDesc".Translate()
            ) * 4f) / 4f;

            listingStandard.Gap();

            listingStandard.GapLine();

            listingStandard.Label("BattIePatch_IEDRemoteDetonation_BottomWarning".Translate());

            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        /// Override SettingsCategory to show up in the list of settings.
        /// Using .Translate() is optional, but does allow for localization.
        public override string SettingsCategory()
        {
            return "BattIePatch_IEDRemoteDetonation_ModName".Translate();
        }
    }
}