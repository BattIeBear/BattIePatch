using UnityEngine;
using Verse;

namespace BattIePatch_TexturedMetalTile
{
    internal class BattIePatchTexturedMetalTileSettingsWindow : Mod
    {
        /// A reference to our settings.
        BattIePatchTexturedMetalTileSettings settings;

        /// A mandatory constructor which resolves the reference to our settings.
        public BattIePatchTexturedMetalTileSettingsWindow(ModContentPack content) : base(content)
        {
            this.settings = GetSettings<BattIePatchTexturedMetalTileSettings>();
        }

        /// The (optional) GUI part to set your settings.
        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);

            listingStandard.Label("BattIePatch_TexturedMetalTile_TopWarning".Translate());

            listingStandard.GapLine();

            listingStandard.Gap();
            listingStandard.Gap();
            listingStandard.Gap();

            listingStandard.CheckboxLabeled("BattIePatch_TexturedMetalTile_NewTextureSteel".Translate(), ref BattIePatchTexturedMetalTileSettings.newTextureSteel, "BattIePatch_TexturedMetalTile_NewTextureSteelDesc".Translate());
            listingStandard.CheckboxLabeled("BattIePatch_TexturedMetalTile_NewTextureGold".Translate(), ref BattIePatchTexturedMetalTileSettings.newTextureGold, "BattIePatch_TexturedMetalTile_NewTextureGoldDesc".Translate());
            listingStandard.CheckboxLabeled("BattIePatch_TexturedMetalTile_NewTextureSilver".Translate(), ref BattIePatchTexturedMetalTileSettings.newTextureSilver, "BattIePatch_TexturedMetalTile_NewTextureSilverDesc".Translate());
            listingStandard.CheckboxLabeled("BattIePatch_TexturedMetalTile_NewTextureBioferrite".Translate(), ref BattIePatchTexturedMetalTileSettings.newTextureBioferrite, "BattIePatch_TexturedMetalTile_NewTextureBioferriteDesc".Translate());

            listingStandard.Gap();
            listingStandard.GapLine();

            listingStandard.Label("BattIePatch_TexturedMetalTile_BottomWarning".Translate());

            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        /// Override SettingsCategory to show up in the list of settings.
        /// Using .Translate() is optional, but does allow for localization.
        public override string SettingsCategory()
        {
            return "BattIePatch_TexturedMetalTile_ModName".Translate();
        }
    }
}