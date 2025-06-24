using System.Xml;
using Verse;

namespace BattIePatch_EnhancedBioregeneration
{
    public class LessenedRegenerationSettings : PatchOperation
    {
        private PatchOperation LessenedRegeneration;
        private PatchOperation DefaultRegeneration;

        protected override bool ApplyWorker(XmlDocument xml)
        {
            if (BattIePatchEnhancedBioregenerationSettings.LessenedRegeneration)
            {
                if (LessenedRegeneration != null)
                {
                    return LessenedRegeneration.Apply(xml);
                }
            }
            else
            {
                if (DefaultRegeneration != null)
                {
                    return DefaultRegeneration.Apply(xml);
                }
            }

            return true;
        }
    }

    public class HealAllConditionsSettings : PatchOperation
    {
        private PatchOperation HealAll;
        private PatchOperation DefaultHeal;

        protected override bool ApplyWorker(XmlDocument xml)
        {
            if (BattIePatchEnhancedBioregenerationSettings.HealAllConditions)
            {
                if (HealAll != null)
                {
                    return HealAll.Apply(xml);
                }
            }
            else
            {
                if (DefaultHeal != null)
                {
                    return DefaultHeal.Apply(xml);
                }
            }

            return true;
        }
    }

    public class TimeDificultySettings : PatchOperation
    {
        private PatchOperation ExtraTime;

        protected override bool ApplyWorker(XmlDocument xml)
        {
            if (BattIePatchEnhancedBioregenerationSettings.UseExtraTime)
            {
                if (ExtraTime != null)
                {
                    return ExtraTime.Apply(xml);
                }
            }

            return true;
        }
    }

    public class MedsDificultySettings : PatchOperation
    {
        private PatchOperation ExtraMeds;

        protected override bool ApplyWorker(XmlDocument xml)
        {
            if (BattIePatchEnhancedBioregenerationSettings.UseExtraMeds)
            {
                if (ExtraMeds != null)
                {
                    return ExtraMeds.Apply(xml);
                }
            }

            return true;
        }
    }
}
