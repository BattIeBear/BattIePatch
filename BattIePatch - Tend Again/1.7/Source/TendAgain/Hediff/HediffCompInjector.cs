using Verse;

namespace BattIePatch_TendAgain
{
    [StaticConstructorOnStartup]
    public static class HediffCompInjector
    {
        static HediffCompInjector()
        {
            foreach (HediffDef def in DefDatabase<HediffDef>.AllDefs)
            {
                if (!def.tendable)
                {
                    continue;
                }

                if (def.comps == null)
                {
                    def.comps = new List<HediffCompProperties>();
                }

                List<HediffCompProperties> comps = def.comps;

                if (comps.OfType<HediffCompProperties_Disappears>().Any())
                {
                    continue;
                }

                HediffCompProperties_TendDuration tendDuration = comps.OfType<HediffCompProperties_TendDuration>().FirstOrDefault();
                if (tendDuration != null)
                {
                    if (tendDuration.disappearsAtTotalTendQuality > -1)
                    {
                        continue;
                    }
                }
                else
                {
                    continue;
                }

                if (def.chronic && !BattIePatchTendAgainSettings.tendAgainCheck_ChronicDisease)
                {
                    continue;
                }

                if (!comps.OfType<HediffCompProperties_TendTracker>().Any())
                {
                    def.comps.Add(new HediffCompProperties_TendTracker());
                }
            }
        }
    }
}
