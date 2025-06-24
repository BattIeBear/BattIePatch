using UnityEngine;
using Verse;

namespace BattIePatch_EnhancedBioregeneration
{
    public class BattIePatchEnhancedBioregenerationSettingsWindow : Mod
    {
        // A reference to our settings.
        BattIePatchEnhancedBioregenerationSettings settings;

        // A mandatory constructor which resolves the reference to our settings.
        public BattIePatchEnhancedBioregenerationSettingsWindow(ModContentPack content) : base(content)
        {
            this.settings = GetSettings<BattIePatchEnhancedBioregenerationSettings>();
        }

        // The (optional) GUI part to set your settings.
        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            listingStandard.Label("BattIePatch_EnhancedBioregeneration_TopWarning".Translate());

            listingStandard.GapLine();

            listingStandard.Gap();
            listingStandard.Gap();
            listingStandard.Gap();
            listingStandard.Label("BattIePatch_EnhancedBioregeneration_Lable1".Translate());
            listingStandard.CheckboxLabeled("BattIePatch_EnhancedBioregeneration_UseExtraTime".Translate(), ref BattIePatchEnhancedBioregenerationSettings.UseExtraTime, "BattIePatch_EnhancedBioregeneration_UseExtraTimeDesc".Translate());
            listingStandard.CheckboxLabeled("BattIePatch_EnhancedBioregeneration_UseExtraMeds".Translate(), ref BattIePatchEnhancedBioregenerationSettings.UseExtraMeds, "BattIePatch_EnhancedBioregeneration_UseExtraMedsDesc".Translate());

            listingStandard.Gap();
            listingStandard.GapLine();
            listingStandard.Gap();

            listingStandard.Label("BattIePatch_EnhancedBioregeneration_Lable2".Translate());
            listingStandard.CheckboxLabeled("BattIePatch_EnhancedBioregeneration_LessenedRegeneration".Translate(), ref BattIePatchEnhancedBioregenerationSettings.LessenedRegeneration, "BattIePatch_EnhancedBioregeneration_LessenedRegenerationDesc".Translate());
            listingStandard.CheckboxLabeled("BattIePatch_EnhancedBioregeneration_HealAllConditions".Translate(), ref BattIePatchEnhancedBioregenerationSettings.HealAllConditions, "BattIePatch_EnhancedBioregeneration_HealAllConditionsDesc".Translate());

            listingStandard.Gap();
            listingStandard.GapLine();
            listingStandard.Gap();

            listingStandard.Label("BattIePatch_EnhancedBioregeneration_Lable3".Translate());
            listingStandard.CheckboxLabeled("BattIePatch_EnhancedBioregeneration_KeepMinorScars".Translate(), ref BattIePatchEnhancedBioregenerationSettings.KeepMinorScars, "BattIePatch_EnhancedBioregeneration_KeepMinorScarsDesc".Translate());
            if (BattIePatchEnhancedBioregenerationSettings.KeepMinorScars == true)
            {
                BattIePatchEnhancedBioregenerationSettings.KeepAllScars = false;
            }
            listingStandard.CheckboxLabeled("BattIePatch_EnhancedBioregeneration_KeepAllScars".Translate(), ref BattIePatchEnhancedBioregenerationSettings.KeepAllScars, "BattIePatch_EnhancedBioregeneration_KeepAllScarsDesc".Translate());
            if (BattIePatchEnhancedBioregenerationSettings.KeepAllScars == true)
            {
                BattIePatchEnhancedBioregenerationSettings.KeepMinorScars = false;
            }
            listingStandard.Gap();

            listingStandard.GapLine();

            listingStandard.Label("BattIePatch_EnhancedBioregeneration_BottomWarning".Translate());

            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        // Override SettingsCategory to show up in the list of settings.
        // Using .Translate() is optional, but does allow for localization.
        public override string SettingsCategory()
        {
            return "BattIePatch_EnhancedBioregeneration_ModName".Translate();
        }
    }
}
