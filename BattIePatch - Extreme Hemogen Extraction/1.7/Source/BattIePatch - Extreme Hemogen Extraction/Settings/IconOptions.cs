using System.Xml;
using Verse;

namespace BattIePatch_ExtremeHemogenExtraction
{
    public class IconOptions : PatchOperation
    {
        private PatchOperation UseImage;

        protected override bool ApplyWorker(XmlDocument xml)
        {
            if (BattIePatchExtremeHemogenExtractionSettings.UseHemogenImage)
            {
                if (UseImage != null)
                {
                    return UseImage.Apply(xml);
                }
            }

            return true;
        }
    }
}
