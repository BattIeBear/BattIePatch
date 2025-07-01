using UnityEngine;
using Verse;

namespace BattIePatch_SilenceIsGolden
{
    class BattIePatchSilenceIsGoldenSettingsWindow : Mod
    {
        /// A reference to our settings.
        BattIePatchSilenceIsGoldenSettings settings;

        /// A mandatory constructor which resolves the reference to our settings.
        public BattIePatchSilenceIsGoldenSettingsWindow(ModContentPack content) : base(content)
        {
            this.settings = GetSettings<BattIePatchSilenceIsGoldenSettings>();
        }

        /// The (optional) GUI part to set your settings.
        public override void DoSettingsWindowContents(Rect inRect)
        {
            Rect lRect = new Rect(inRect.x, inRect.y, (inRect.width / 2f) - 15f, inRect.height);
            Rect rRect = new Rect((inRect.x + inRect.width / 2f) + 15f, inRect.y, (inRect.width / 2f) - 15f, inRect.height);
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            listingStandard.Label("BattIePatch_SilenceIsGolden_TopWarning".Translate());

            listingStandard.GapLine();

            listingStandard.Gap();
            listingStandard.Gap();
            listingStandard.Gap();

            listingStandard.Label("BattIePatch_SilenceIsGolden_Lable1".Translate());
            listingStandard.CheckboxLabeled("BattIePatch_SilenceIsGolden_GoldenCubeImmunity".Translate(), ref BattIePatchSilenceIsGoldenSettings.GoldenCubeImmunity, "BattIePatch_SilenceIsGolden_GoldenCubeImmunityDesc".Translate());
            listingStandard.CheckboxLabeled("BattIePatch_SilenceIsGolden_UnnaturalCorpseImmunity".Translate(), ref BattIePatchSilenceIsGoldenSettings.UnnaturalCorpseImmunity, "BattIePatch_SilenceIsGolden_UnnaturalCorpseImmunityDesc".Translate());
            listingStandard.CheckboxLabeled("BattIePatch_SilenceIsGolden_RevenantHypnosisImmunity".Translate(), ref BattIePatchSilenceIsGoldenSettings.RevenantHypnosisImmunity, "BattIePatch_SilenceIsGolden_RevenantHypnosisImmunityDesc".Translate());

            listingStandard.Gap();

            listingStandard.GapLine();

            listingStandard.Label("BattIePatch_SilenceIsGolden_BottomWarning".Translate());

            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        /// Override SettingsCategory to show up in the list of settings.
        /// Using .Translate() is optional, but does allow for localization.
        public override string SettingsCategory()
        {
            return "BattIePatch_SilenceIsGolden_ModName".Translate();
        }
    }
}