using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace BattIePatch_BetterFactionColors
{
    public class BattIePatch_FactionColorEntry : IExposable
    {
        public string factionDefName;

        public List<Color> colors = new List<Color>(4) {Color.clear, Color.clear, Color.clear, Color.clear };

        public void ExposeData()
        {
            Scribe_Values.Look(ref factionDefName, "factionDefName");

            Scribe_Collections.Look(ref colors, "colors", LookMode.Value);
            
            if (colors == null || colors.Count != 4)
            {
                colors = new List<Color>() {Color.clear, Color.clear, Color.clear, Color.clear};
            }
        }
    }
}
