using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace BattIePatch_BetterFactionColors
{
    [StaticConstructorOnStartup]
    public static class BattIePatch_Cache
    {
        public static Dictionary<string, List<Color>> defNameToColors = new Dictionary<string, List<Color>>();
        public static List<ModContentPack> sources = new List<ModContentPack>();

        static BattIePatch_Cache()
        {
            var settings = LoadedModManager.GetMod<BattIePatchBetterFactionColorsSettingsWindow>().GetSettings<BattIePatchBetterFactionColorsSettings>();

            defNameToColors = new Dictionary<string, List<Color>>();

            foreach (BattIePatch_FactionColorEntry entry in settings.factionColorEntries)
            {
                defNameToColors[entry.factionDefName] = entry.colors;
            }

            foreach (FactionDef def in DefDatabase<FactionDef>.AllDefs)
            {
                if (def.hidden || def.isPlayer)
                {
                    continue;
                }

                if (def == null)
                {
                    return;
                }

                if (!sources.Contains(def.modContentPack))
                {
                    sources.Add(def.modContentPack);
                }

                if (!defNameToColors.ContainsKey(def.defName))
                {
                    List<Color> defColors = new List<Color> { Color.white, Color.clear, Color.white, Color.clear };
                    if (def.colorSpectrum != null && def.colorSpectrum.Count > 0)
                    {
                        defColors[0] = def.colorSpectrum[0];
                        if (def.colorSpectrum.Count > 1)
                        {
                            defColors[1] = def.colorSpectrum[1];
                        }
                        else
                        {
                            defColors[1] = Color.clear;
                        }

                        defColors[2] = defColors[0];
                        defColors[3] = defColors[1];
                    }

                    defNameToColors[def.defName] = defColors;
                    settings.factionColorEntries.Add(new BattIePatch_FactionColorEntry {factionDefName = def.defName, colors = defColors});
                }

                ApplyNewFactionColors(def);
            }
        }

        public static void ApplyNewFactionColors(FactionDef def)
        {
            if (def == null)
            {
                return;
            }

            List<Color> defColors = defNameToColors[def.defName];

            if (defColors[3].a == 0)
            {
                def.colorSpectrum = new List<Color> { defColors[2] };
            }
            else
            {
                def.colorSpectrum = new List<Color> { defColors[2], defColors[3] };
            }
        }
    }
}
