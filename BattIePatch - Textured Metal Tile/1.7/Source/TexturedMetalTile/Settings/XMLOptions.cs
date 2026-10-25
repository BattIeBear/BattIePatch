using System.Xml;
using Verse;

namespace BattIePatch_TexturedMetalTile
{
    public class UseNewTextureSteel : PatchOperation
    {
        private PatchOperation newSteel;

        protected override bool ApplyWorker(XmlDocument xml)
        {
            if (BattIePatchTexturedMetalTileSettings.newTextureSteel)
            {
                if (newSteel != null)
                {
                    return newSteel.Apply(xml);
                }
            }

            return true;
        }
    }
    public class UseNewTextureGold : PatchOperation
    {
        private PatchOperation newGold;

        protected override bool ApplyWorker(XmlDocument xml)
        {
            if (BattIePatchTexturedMetalTileSettings.newTextureGold)
            {
                if (newGold != null)
                {
                    return newGold.Apply(xml);
                }
            }

            return true;
        }
    }
    public class UseNewTextureSilver : PatchOperation
    {
        private PatchOperation newSilver;

        protected override bool ApplyWorker(XmlDocument xml)
        {
            if (BattIePatchTexturedMetalTileSettings.newTextureSilver)
            {
                if (newSilver != null)
                {
                    return newSilver.Apply(xml);
                }
            }

            return true;
        }
    }
    public class UseNewTextureBioferrite : PatchOperation
    {
        private PatchOperation newBioferrite;

        protected override bool ApplyWorker(XmlDocument xml)
        {
            if (BattIePatchTexturedMetalTileSettings.newTextureBioferrite)
            {
                if (newBioferrite != null)
                {
                    return newBioferrite.Apply(xml);
                }
            }

            return true;
        }
    }
}
