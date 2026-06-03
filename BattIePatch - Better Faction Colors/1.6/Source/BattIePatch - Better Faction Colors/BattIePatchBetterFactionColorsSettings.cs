using System.Collections.Generic;
using Verse;

namespace BattIePatch_BetterFactionColors
{
    public class BattIePatchBetterFactionColorsSettings : ModSettings
    {
        public List<BattIePatch_FactionColorEntry> factionColorEntries = new List<BattIePatch_FactionColorEntry>();
        public override void ExposeData()
        {
            Scribe_Collections.Look(ref factionColorEntries, "factionColors", LookMode.Deep);

            if (factionColorEntries == null)
            {
                factionColorEntries = new List<BattIePatch_FactionColorEntry>();
            }

            base.ExposeData();
        }
    }
}
