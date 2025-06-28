using Verse;

namespace BattIePatch_TexturedMetalTile
{
    internal class BattIePatchTexturedMetalTileSettings : ModSettings
    {
        public static bool newTextureSteel = true;
        public static bool newTextureGold = false;
        public static bool newTextureSilver = false;
        public static bool newTextureBioferrite = false;


        /// The part that writes our settings to file. Note that saving is by ref.
        public override void ExposeData()
        {
            Scribe_Values.Look(ref newTextureSteel, "newTextureSteel", true);
            Scribe_Values.Look(ref newTextureGold, "newTextureGold", false);
            Scribe_Values.Look(ref newTextureSilver, "newTextureSilver", false);
            Scribe_Values.Look(ref newTextureBioferrite, "newTextureBioferrite", false);
            base.ExposeData();
        }
    }
}