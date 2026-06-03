using RimWorld;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Verse;

namespace BattIePatch_BetterFactionColors
{
    public class BattIePatchBetterFactionColorsSettingsWindow : Mod
    {
        /// A reference to our settings.
        BattIePatchBetterFactionColorsSettings settings;

        /// A mandatory constructor which resolves the reference to our settings.
        public BattIePatchBetterFactionColorsSettingsWindow(ModContentPack content) : base(content)
        {
            this.settings = GetSettings<BattIePatchBetterFactionColorsSettings>();
        }

        ModContentPack curSource;
        ModContentPack extraSource;
        List<FactionDef> mergedDefs;
        Dictionary<ModContentPack, List<FactionDef>> sourceToDefs = new Dictionary<ModContentPack, List<FactionDef>>();
        Color copyColorPrimary;
        Color copyColorSecondary;
        string factionSearch = "";

        /// The (optional) GUI part to set your settings.
        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            listingStandard.Label("BattIePatch_BetterFactionColors_TopWarning".Translate());

            listingStandard.GapLine();
            Rect sourceRow = listingStandard.GetRect(40f);
            float x = sourceRow.x;
            float y = sourceRow.y;
            float h = sourceRow.height;
            Rect sourceRect = new Rect(x, y, sourceRow.width / 5f, h);
            Rect extraRect = new Rect(x + sourceRow.width / 5f + 10, y, sourceRow.width / 5f - 10, h);
            Rect eraseRect = new Rect(x + sourceRow.width / 5f * 2f + 10, y, y, h);
            Widgets.Label(new Rect(x + sourceRow.width / 2f, y + (y / 2.8f), 50f, h), "Search".Translate());

            factionSearch = Widgets.TextField(new Rect(x + sourceRow.width / 2f + 55, y, sourceRow.width / 2f - 55, h), factionSearch);

            if (curSource == null)
            {
                curSource = BattIePatch_Cache.sources[0];
            }

            if (Widgets.ButtonText(sourceRect, curSource.Name))
            {
                var options = new List<FloatMenuOption>();
                foreach (ModContentPack source in BattIePatch_Cache.sources)
                {
                    if (curSource == null)
                    {
                        curSource = source;
                    }
                    options.Add(new FloatMenuOption(source.Name,
                        () =>
                        {
                            curSource = source;
                            if (mergedDefs != null)
                            {
                                mergedDefs.Clear();
                            }
                            if (extraSource != null && extraSource == curSource)
                            {
                                extraSource = null;
                            }
                        }
                        ));
                }

                Find.WindowStack.Add(new FloatMenu(options));
            }

            if (Widgets.ButtonText(extraRect, extraSource != null ? extraSource.Name : "Select Extra".Translate().ToString()))
            {
                var options = new List<FloatMenuOption>();
                foreach (ModContentPack source in BattIePatch_Cache.sources)
                {
                    options.Add(new FloatMenuOption(source.Name,
                        () =>
                        {
                            extraSource = source;
                            if (mergedDefs != null)
                            {
                                mergedDefs.Clear();
                            }
                            if (extraSource != null && extraSource == curSource)
                            {
                                extraSource = null;
                            }
                        }
                        ));
                }

                Find.WindowStack.Add(new FloatMenu(options));
            }

            if (Widgets.ButtonText(eraseRect, " X"))
            {
                extraSource = null;
                if (mergedDefs != null)
                {
                    mergedDefs.Clear();
                }
            }

            listingStandard.GapLine();

            if (factionSearch != "")
            {
                foreach (FactionDef def in DefDatabase<FactionDef>.AllDefs)
                {
                    if (def.hidden || def.isPlayer)
                    {
                        continue;
                    }
                    if (def.label.Contains(factionSearch))
                    {
                        Rect FactionRow = listingStandard.GetRect(40f);
                        CreatGradientRow(def, FactionRow);
                    }
                }
            }
            else
            {
                if (extraSource == null)
                {
                    if (sourceToDefs.Count == 0 || sourceToDefs.ContainsKey(curSource) == false)
                    {
                        foreach (FactionDef def in DefDatabase<FactionDef>.AllDefs)
                        {
                            if (def.hidden || def.isPlayer)
                            {
                                continue;
                            }
                            if (def.modContentPack == curSource)
                            {
                                if (sourceToDefs.ContainsKey(curSource) == false)
                                {
                                    sourceToDefs[curSource] = new List<FactionDef>();
                                }
                                sourceToDefs[curSource].Add(def);
                            }
                        }
                    }

                    foreach (FactionDef def in sourceToDefs[curSource])
                    {
                        if (def.hidden || def.isPlayer)
                        {
                            continue;
                        }
                        Rect FactionRow = listingStandard.GetRect(40f);
                        CreatGradientRow(def, FactionRow);
                    }
                }
                else
                {
                    if (sourceToDefs.Count == 0 || sourceToDefs.ContainsKey(curSource) == false)
                    {
                        foreach (FactionDef def in DefDatabase<FactionDef>.AllDefs)
                        {
                            if (def.hidden || def.isPlayer)
                            {
                                continue;
                            }
                            if (def.modContentPack == curSource)
                            {
                                if (sourceToDefs.ContainsKey(curSource) == false)
                                {
                                    sourceToDefs[curSource] = new List<FactionDef>();
                                }
                                sourceToDefs[curSource].Add(def);
                            }
                        }
                    }

                    if (sourceToDefs.ContainsKey(extraSource) == false)
                    {
                        foreach (FactionDef def in DefDatabase<FactionDef>.AllDefs)
                        {
                            if (def.hidden || def.isPlayer)
                            {
                                continue;
                            }
                            if (def.modContentPack == extraSource)
                            {
                                if (sourceToDefs.ContainsKey(extraSource) == false)
                                {
                                    sourceToDefs[extraSource] = new List<FactionDef>();
                                }
                                sourceToDefs[extraSource].Add(def);
                            }
                        }
                    }
                    if (mergedDefs == null || mergedDefs.Count == 0)
                    {
                        mergedDefs = new List<FactionDef>();
                        mergedDefs.AddRange(sourceToDefs[curSource]);
                        mergedDefs.AddRange(sourceToDefs[extraSource]);
                    }
                    foreach (FactionDef def in mergedDefs)
                    {
                        if (def.hidden || def.isPlayer)
                        {
                            continue;
                        }
                        Rect FactionRow = listingStandard.GetRect(40f);
                        CreatGradientRow(def, FactionRow);
                    }
                }
            }
            listingStandard.GapLine();

            listingStandard.Label("BattIePatch_BetterFactionColors_BottomWarning".Translate());
            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        /// Override SettingsCategory to show up in the list of settings.
        /// Using .Translate() is optional, but does allow for localization.
        public override string SettingsCategory()
        {
            return "BattIePatch_BetterFactionColors_ModName".Translate();
        }

        private void CreatGradientRow(FactionDef def, Rect row)
        {
            if (def == null)
            {
                return;
            }
            // color settings row

            List<Color> defColors = BattIePatch_Cache.defNameToColors[def.defName];

            float x = row.x;
            float y = row.y;
            float h = row.height;

            Rect labelRect = new Rect(x, y, row.size.x / 2f, h);
            Widgets.Label(labelRect, def.label);

            x += row.size.x / 2f;

            Rect primaryRect = new Rect(x, y, 40f, h);

            Widgets.DrawBoxSolid(primaryRect, defColors[2]);

            if (Widgets.ButtonInvisible(primaryRect))
            {
                Color current = defColors[2];

                Find.WindowStack.Add(new BattIePatch_FactionColorPicker(current, c =>
                {
                    defColors[2] = c;
                    BattIePatch_Cache.ApplyNewFactionColors(def);
                }));
            }

            x += 45f;

            Rect gradRect = new Rect(x, y, 120f, h);

            if (defColors[3].a != 0)
            {
                for (int i = 0; i < gradRect.width; i++)
                {
                    float t = i / gradRect.width;

                    Color c = Color.Lerp(
                        defColors[2],
                        defColors[3],
                        t);

                    Widgets.DrawBoxSolid(
                        new Rect(gradRect.x + i, gradRect.y, 1f, gradRect.height),
                        c);
                }
            }
            else
            {
                Widgets.DrawBoxSolid(gradRect, defColors[2]);
            }

            x += 125f;

            Rect secondaryRect = new Rect(x, y, 40f, h);

            Color secondaryColor = defColors[3];

            if (secondaryColor.a == 0)
            {
                secondaryColor.a = 1;
            }
            Widgets.DrawBoxSolid(secondaryRect, secondaryColor);

            Widgets.DrawHighlightIfMouseover(secondaryRect);

            if (Widgets.ButtonInvisible(secondaryRect))
            {
                Find.WindowStack.Add(new BattIePatch_FactionColorPicker(secondaryColor, c =>
                {
                    defColors[3] = c;
                    BattIePatch_Cache.ApplyNewFactionColors(def);
                }));
            }

            x += 45f;

            Rect clearRect = new Rect(x, y, 50f, h);
            if (defColors[3].a == 0)
            {
                if (Widgets.ButtonText(clearRect, "Grad".Translate()))
                {
                    Color c = defColors[3];
                    c.a = 1;
                    defColors[3] = c;
                    BattIePatch_Cache.ApplyNewFactionColors(def);
                }
            }
            else
            {
                if (Widgets.ButtonText(clearRect, "Mono".Translate()))
                {
                    Color c = defColors[3];
                    c.a = 0;
                    defColors[3] = c;
                    BattIePatch_Cache.ApplyNewFactionColors(def);
                }
            }

            x += 55f;

            Rect resetRect = new Rect(x, y, 50f, h);

            if (Widgets.ButtonText(resetRect, "Reset".Translate()))
            {
                defColors[2] = defColors[0];
                defColors[3] = defColors[1];
                BattIePatch_Cache.ApplyNewFactionColors(def);
            }

            x += 55f;

            Rect copyRect = new Rect(x, y, 50f, h);

            if (Widgets.ButtonText(copyRect, "Copy".Translate()))
            {
                copyColorPrimary = defColors[2];
                copyColorSecondary = defColors[3];
            }

            x += 55f;

            Rect pasteRect = new Rect(x, y, 50f, h);

            if (Widgets.ButtonText(pasteRect, "Paste".Translate()))
            {
                if (copyColorPrimary == null || copyColorSecondary == null)
                {
                    return;
                }

                defColors[2] = copyColorPrimary;
                defColors[3] = copyColorSecondary;
            }
        }
    }
}