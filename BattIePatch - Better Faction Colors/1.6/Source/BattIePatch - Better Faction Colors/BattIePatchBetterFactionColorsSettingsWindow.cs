using RimWorld;
using System.Collections.Generic;
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
        private Vector2 scrollPosition = Vector2.zero;
        bool showAll = false;
        ModContentPack curSource;
        ModContentPack extraSource;
        List<FactionDef> defsToDraw = new List<FactionDef>();
        Dictionary<ModContentPack, List<FactionDef>> sourceToDefs = new Dictionary<ModContentPack, List<FactionDef>>();
        Color copyColorPrimary;
        Color copyColorSecondary;
        string factionSearch = "";
        string storedSearch = "";
        float midwayStartX = 0f;
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
            Rect allRect = new Rect(x, y, 40f, h);
            float labelWidth = (sourceRow.width / 4f) - 55f;
            if (midwayStartX == 0f)
            {
                midwayStartX = (sourceRow.width / 2) - 5f;
            }
            float spaceBetween = (midwayStartX - ((labelWidth * 2) + 80)) / 3f;
            if (Widgets.ButtonText(allRect, "BattIePatch_BetterFactionColors_All".Translate()))
            {
                showAll = !showAll;
                defsToDraw.Clear();
            }

            if (curSource == null)
            {
                curSource = BattIePatch_Cache.sources[0];
            }

            x += 40f + spaceBetween;
            Rect sourceRect = new Rect(x, y, labelWidth, h);
            // First Button
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
                            showAll = false;
                            defsToDraw.Clear();
                            if (extraSource != null && extraSource == curSource)
                            {
                                extraSource = null;
                            }
                        }
                        ));
                }

                Find.WindowStack.Add(new FloatMenu(options));
            }

            x += labelWidth + spaceBetween;
            Rect extraRect = new Rect(x, y, labelWidth, h);
            // Second Button
            if (Widgets.ButtonText(extraRect, extraSource != null ? extraSource.Name : "BattIePatch_BetterFactionColors_SelectExtra".Translate().ToString()))
            {
                var options = new List<FloatMenuOption>();
                foreach (ModContentPack source in BattIePatch_Cache.sources)
                {
                    options.Add(new FloatMenuOption(source.Name,
                        () =>
                        {
                            extraSource = source;
                            showAll = false;
                            defsToDraw.Clear();
                            if (extraSource != null && extraSource == curSource)
                            {
                                extraSource = null;
                            }
                        }
                        ));
                }

                Find.WindowStack.Add(new FloatMenu(options));
            }
            x += labelWidth + spaceBetween;
            Rect eraseRect = new Rect(x, y, 40f, h);
            // Third Button
            if (Widgets.ButtonText(eraseRect, "BattIePatch_BetterFactionColors_X".Translate()))
            {
                extraSource = null;
                defsToDraw.Clear();
                showAll = false;
                GUI.FocusControl(null);
                factionSearch = "";
                storedSearch = "";
            }
            x = midwayStartX + 10;
            //Search bar
            Widgets.Label(new Rect(x, y + (y / 2.8f), 50f, h), "BattIePatch_BetterFactionColors_Search".Translate());
            x += 55f;
            factionSearch = Widgets.TextField(new Rect(x, y, sourceRow.width / 2f - 60, h), factionSearch);

            listingStandard.GapLine();
            
            if (factionSearch != storedSearch)
            {
                showAll = false;
                defsToDraw.Clear();
                storedSearch = "";
            }

            if (showAll && defsToDraw.Count == 0)
            {
                foreach (FactionDef def in DefDatabase<FactionDef>.AllDefs)
                {
                    if (def.hidden || def.isPlayer)
                    {
                        continue;
                    }

                    defsToDraw.AddUnique(def);
                }
            }
            else if (defsToDraw != null && defsToDraw.Count > 0)
            {
                Rect scrollOutRect = listingStandard.GetRect(inRect.height - listingStandard.CurHeight - 60f);
                float contentHeight = Mathf.Max(defsToDraw.Count * 40f, scrollOutRect.height);

                Rect scrollViewRect = new Rect(0f, 0f, scrollOutRect.width - 16f, contentHeight);

                Widgets.BeginScrollView(scrollOutRect, ref scrollPosition, scrollViewRect);

                Listing_Standard scrollListing = new Listing_Standard();
                scrollListing.Begin(scrollViewRect);
                foreach (FactionDef def in defsToDraw)
                {
                    Rect factionRow = scrollListing.GetRect(40f);
                    CreateGradientRow(def, factionRow);
                }
                scrollListing.End();
                Widgets.EndScrollView();
            }
            else
            {
                defsToDraw.Clear();
                showAll = false;
                if (factionSearch != "")
                {
                    storedSearch = factionSearch;
                    foreach (FactionDef def in DefDatabase<FactionDef>.AllDefs)
                    {
                        if (def.hidden || def.isPlayer)
                        {
                            continue;
                        }
                        if (def.label.ToLower().Contains(factionSearch.ToLower()))
                        {
                            defsToDraw.AddUnique(def);
                        }
                    }
                }
                else
                {
                    if (sourceToDefs.Count == 0 || sourceToDefs.ContainsKey(curSource) == false)
                    {
                        sourceToDefs[curSource] = new List<FactionDef>();
                        foreach (FactionDef def in DefDatabase<FactionDef>.AllDefs)
                        {
                            if (def.hidden || def.isPlayer)
                            {
                                continue;
                            }
                            if (def.modContentPack == curSource)
                            {
                                sourceToDefs[curSource].Add(def);
                            }
                        }

                        //if (sourceToDefs[curSource].Count == 0)
                        //{
                        //    curSource = null;
                        //    sourceToDefs.Remove(curSource);
                        //    BattIePatch_Cache.sources.Remove(curSource);
                        //    return;
                        //}
                    }

                    foreach (FactionDef def in sourceToDefs[curSource])
                    {
                        if (def.hidden || def.isPlayer)
                        {
                            continue;
                        }
                        defsToDraw.AddUnique(def);
                    }

                    if(extraSource != null)
                    {
                        if (sourceToDefs.ContainsKey(extraSource) == false)
                        {
                            sourceToDefs[extraSource] = new List<FactionDef>();
                            foreach (FactionDef def in DefDatabase<FactionDef>.AllDefs)
                            {
                                if (def.hidden || def.isPlayer)
                                {
                                    continue;
                                }
                                if (def.modContentPack == extraSource)
                                {
                                    sourceToDefs[extraSource].Add(def);
                                }
                            }

                            //if (sourceToDefs[extraSource].Count == 0)
                            //{
                            //    extraSource = null;
                            //    sourceToDefs.Remove(extraSource);
                            //    BattIePatch_Cache.sources.Remove(extraSource);
                            //    return;
                            //}
                        }

                        foreach (FactionDef def in sourceToDefs[extraSource])
                        {
                            defsToDraw.AddUnique(def);
                        }
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

        private void CreateGradientRow(FactionDef def, Rect row)
        {
            if (def == null)
            {
                return;
            }
            // color settings row
            List<Color> defColors = new List<Color>();
            if (BattIePatch_Cache.defNameToColors.ContainsKey(def.defName))
            {
                defColors = BattIePatch_Cache.defNameToColors[def.defName];
            }
            else
            {
                Log.Error("Def " + def.defName + " from " + def.modContentPack.Name + " was not found in BattIePatch_Cache.defNameToColors. This should not happen. Please report this to the mod author.");
                return;
            }
             

            float x = row.x;
            float y = row.y;
            float h = row.height;

            Rect labelRect = new Rect(x, y, midwayStartX - 40f, h);
            Widgets.Label(labelRect, def.label);

            x += midwayStartX - 40f;
            float rowSpace = row.width - (midwayStartX - 40f);

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

            x += primaryRect.width + 5f;

            Rect gradRect = new Rect(x, y, rowSpace - 303f, h);

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

            x += gradRect.width + 5f;

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

            x += secondaryRect.width + 4f;

            Rect clearRect = new Rect(x, y, 50f, h);
            if (defColors[3].a == 0)
            {
                if (Widgets.ButtonText(clearRect, "BattIePatch_BetterFactionColors_Ease".Translate()))
                {
                    Color c = defColors[3];
                    c.a = 1;
                    defColors[3] = c;
                    BattIePatch_Cache.ApplyNewFactionColors(def);
                }
            }
            else
            {
                if (Widgets.ButtonText(clearRect, "BattIePatch_BetterFactionColors_Single".Translate()))
                {
                    Color c = defColors[3];
                    c.a = 0;
                    defColors[3] = c;
                    BattIePatch_Cache.ApplyNewFactionColors(def);
                }
            }

            x += clearRect.width + 3f;

            Rect resetRect = new Rect(x, y, 50f, h);

            if (Widgets.ButtonText(resetRect, "BattIePatch_BetterFactionColors_Reset".Translate()))
            {
                defColors[2] = defColors[0];
                defColors[3] = defColors[1];
                BattIePatch_Cache.ApplyNewFactionColors(def);
            }

            x += resetRect.width + 3f;

            Rect copyRect = new Rect(x, y, 50f, h);

            if (Widgets.ButtonText(copyRect, "BattIePatch_BetterFactionColors_Copy".Translate()))
            {
                copyColorPrimary = defColors[2];
                copyColorSecondary = defColors[3];
            }

            x += copyRect.width + 3f;

            Rect pasteRect = new Rect(x, y, 50f, h);

            if (Widgets.ButtonText(pasteRect, "BattIePatch_BetterFactionColors_Paste".Translate()))
            {
                if (copyColorPrimary.a == 0 && copyColorSecondary.a == 0)
                {
                    return;
                }

                defColors[2] = copyColorPrimary;
                defColors[3] = copyColorSecondary;
                BattIePatch_Cache.ApplyNewFactionColors(def);
            }
        }
    }
}