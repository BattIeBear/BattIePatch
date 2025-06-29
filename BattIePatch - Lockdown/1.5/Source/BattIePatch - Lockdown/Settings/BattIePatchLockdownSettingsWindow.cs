using UnityEngine;
using Verse;

namespace BattIePatch_Lockdown
{
    internal class BattIePatchLockdownSettingsWindow : Mod
    {
        /// A reference to our settings.
        BattIePatchLockdownSettings settings;

        /// A mandatory constructor which resolves the reference to our settings.
        public BattIePatchLockdownSettingsWindow(ModContentPack content) : base(content)
        {
            this.settings = GetSettings<BattIePatchLockdownSettings>();
        }

        /// The (optional) GUI part to set your settings.
        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);

            listingStandard.Label("BattIePatch_Lockdown_TopWarning".Translate());

            listingStandard.GapLine();

            listingStandard.Gap();
            listingStandard.Gap();
            listingStandard.Gap();

            listingStandard.Label("BattIePatch_Lockdown_Lable1".Translate());
            listingStandard.CheckboxLabeled("BattIePatch_Lockdown_OverridesHoldOpen".Translate(), ref BattIePatchLockdownSettings.OverridesHoldOpen, "BattIePatch_Lockdown_OverridesHoldOpenDesc".Translate());

            listingStandard.Gap();

            listingStandard.GapLine();

            listingStandard.Label("BattIePatch_Lockdown_BottomWarning".Translate());

            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        /// Override SettingsCategory to show up in the list of settings.
        /// Using .Translate() is optional, but does allow for localization.
        public override string SettingsCategory()
        {
            return "BattIePatch_Lockdown_ModName".Translate();
        }
    }
}