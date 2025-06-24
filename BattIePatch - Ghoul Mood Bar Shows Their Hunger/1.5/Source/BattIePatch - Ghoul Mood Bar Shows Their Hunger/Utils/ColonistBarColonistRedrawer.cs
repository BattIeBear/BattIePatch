using RimWorld.Planet;
using UnityEngine;
using Verse;
using RimWorld;
using System.Collections.Generic;

namespace BattIePatch_GhoulMoodBarFix
{
    struct IconDrawCall
    {
        public Texture2D texture;

        public string tooltip;

        public Color? color;

        public IconDrawCall(Texture2D texture, string tooltip = null, Color? color = null)
        {
            this.texture = texture;
            this.tooltip = tooltip;
            this.color = color;
        }
    }
    public static class ColonistBarColonistRedrawer
    {
        private static ColonistBar ColonistBar => Find.ColonistBar;
        private static Dictionary<string, string> pawnLabelsCache = new Dictionary<string, string>();
        private static readonly Texture2D MoodBGTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.4f, 0.47f, 0.53f, 0.44f));
        private static readonly float BaseIconAreaWidth = ColonistBarColonistDrawer.PawnTextureSize.x;
        private static readonly float BaseIconMaxSize = 20f;
        private static readonly Texture2D MoodAtlas = ContentFinder<Texture2D>.Get("UI/Widgets/SubtleGradient");
        private static readonly Texture2D DeadColonistTex = ContentFinder<Texture2D>.Get("UI/Misc/DeadColonist");
        private static readonly Texture2D MoodGradient = ContentFinder<Texture2D>.Get("UI/Widgets/MoodGradient");
        private static readonly Texture2D Icon_FormingCaravan = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/FormingCaravan");
        private static readonly Texture2D Icon_MentalStateNonAggro = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/MentalStateNonAggro");
        private static readonly Texture2D Icon_MentalStateAggro = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/MentalStateAggro");
        private static readonly Texture2D Icon_MedicalRest = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/MedicalRest");
        private static readonly Texture2D Icon_Sleeping = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/Sleeping");
        private static readonly Texture2D Icon_Fleeing = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/Fleeing");
        private static readonly Texture2D Icon_Attacking = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/Attacking");
        private static readonly Texture2D Icon_Idle = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/Idle");
        private static readonly Texture2D Icon_Burning = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/Burning");
        private static readonly Texture2D Icon_Inspired = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/Inspired");
        private static List<IconDrawCall> tmpIconsToDraw = new List<IconDrawCall>();

        public static void RedrawColonist(ColonistBarColonistDrawer instance, Rect rect, Pawn colonist, Map pawnMap, bool highlight, bool reordering)
        {
            float alpha = ColonistBar.GetEntryRectAlpha(rect);
            bool num = Prefs.VisibleMood && colonist.needs?.food != null && colonist.mindState.mentalBreaker.CanDoRandomMentalBreaks && !colonist.Dead && !colonist.Downed;
            MoodThreshold moodThreshold = FoodToMood.Convert(colonist);
            Color color = moodThreshold.GetColor();
            color.a *= alpha;
            ApplyEntryInAnotherMapAlphaFactor(pawnMap, ref alpha);
            if (reordering)
            {
                alpha *= 0.5f;
            }

            Color color3 = (GUI.color = new Color(1f, 1f, 1f, alpha));
            if (num && alpha >= 1f)
            {
                float num2 = moodThreshold.EdgeExpansion();
                if (num2 > 0f)
                {
                    GUI.color = color;
                    Widgets.DrawAtlas(rect.ExpandedBy(num2), MoodAtlas);
                    GUI.color = color3;
                }
            }

            GUI.DrawTexture(rect, (Texture)ColonistBar.BGTex);
            if (colonist.needs != null && colonist.needs.food != null)
            {
                Rect rect2 = rect.ContractedBy(2f);
                float num3 = rect2.height * colonist.needs.food.CurLevelPercentage;
                rect2.yMin = rect2.yMax - num3;
                rect2.height = num3;
                GUI.DrawTexture(rect2, (Texture)MoodBGTex);
            }

            if (num && alpha >= 1f)
            {
                float transparency = ((moodThreshold < MoodThreshold.Major) ? 0.1f : 0.15f);
                Widgets.DrawBoxSolid(rect, moodThreshold.GetColor().ToTransparent(transparency));
            }

            if (highlight)
            {
                int thickness = ((rect.width <= 22f) ? 2 : 3);
                GUI.color = Color.white;
                Widgets.DrawBox(rect, thickness);
                GUI.color = color3;
            }

            Rect rect3 = rect.ContractedBy(-2f * ColonistBar.Scale);
            if ((colonist.Dead ? Find.Selector.SelectedObjects.Contains(colonist.Corpse) : Find.Selector.SelectedObjects.Contains(colonist)) && !WorldRendererUtility.WorldRenderedNow)
            {
                DrawSelectionOverlayOnGUI(colonist, rect3);
            }
            else if (WorldRendererUtility.WorldRenderedNow && colonist.IsCaravanMember() && Find.WorldSelector.IsSelected(colonist.GetCaravan()))
            {
                DrawCaravanSelectionOverlayOnGUI(colonist.GetCaravan(), rect3);
            }

            GUI.DrawTexture(instance.GetPawnTextureRect(rect.position), (Texture)PortraitsCache.Get(colonist, ColonistBarColonistDrawer.PawnTextureSize, Rot4.South, ColonistBarColonistDrawer.PawnTextureCameraOffset, 1.28205f));
            if (num)
            {
                Rect rect4 = rect.ContractedBy(1f);
                Widgets.BeginGroup(rect4);
                Rect rect5 = rect4.AtZero();
                rect5.yMin = rect5.yMax - 35f;
                GUI.color = color;
                GUI.DrawTexture(rect5, (Texture)MoodGradient);
                GUI.color = color3;
                Widgets.EndGroup();
            }

            GUI.color = new Color(1f, 1f, 1f, alpha * 0.8f);
            DrawIcons(rect, colonist);
            GUI.color = color3;
            if (colonist.Dead)
            {
                GUI.DrawTexture(rect, (Texture)DeadColonistTex);
            }

            float num4 = 4f * ColonistBar.Scale;
            Vector2 pos = new Vector2(rect.center.x, rect.yMax - num4);
            GenMapUI.DrawPawnLabel(colonist, pos, alpha, rect.width + ColonistBar.SpaceBetweenColonistsHorizontal - 2f, pawnLabelsCache);
            Text.Font = GameFont.Small;
            GUI.color = Color.white;
        }

        private static void ApplyEntryInAnotherMapAlphaFactor(Map map, ref float alpha)
        {
            if (map == null)
            {
                if (!WorldRendererUtility.WorldRenderedNow)
                {
                    alpha = Mathf.Min(alpha, 0.4f);
                }
            }
            else if (map != Find.CurrentMap || WorldRendererUtility.WorldRenderedNow)
            {
                alpha = Mathf.Min(alpha, 0.4f);
            }
        }

        private static void DrawIcons(Rect rect, Pawn colonist)
        {
            if (colonist.Dead)
            {
                return;
            }

            tmpIconsToDraw.Clear();
            bool flag = false;
            if (colonist.CurJob != null)
            {
                JobDef def = colonist.CurJob.def;
                if (def == JobDefOf.AttackMelee || def == JobDefOf.AttackStatic)
                {
                    flag = true;
                }
                else if (def == JobDefOf.Wait_Combat && colonist.stances.curStance is Stance_Busy stance_Busy && stance_Busy.focusTarg.IsValid)
                {
                    flag = true;
                }
            }

            if (colonist.IsFormingCaravan())
            {
                tmpIconsToDraw.Add(new IconDrawCall(Icon_FormingCaravan, "ActivityIconFormingCaravan".Translate()));
            }

            if (colonist.InAggroMentalState)
            {
                tmpIconsToDraw.Add(new IconDrawCall(Icon_MentalStateAggro, colonist.MentalStateDef.LabelCap));
            }
            else if (colonist.InMentalState)
            {
                tmpIconsToDraw.Add(new IconDrawCall(Icon_MentalStateNonAggro, colonist.MentalStateDef.LabelCap));
            }
            else if (colonist.InBed() && colonist.CurrentBed().Medical)
            {
                tmpIconsToDraw.Add(new IconDrawCall(Icon_MedicalRest, "ActivityIconMedicalRest".Translate()));
            }
            else
            {
                if (colonist.CurJob != null && colonist.jobs.curDriver.asleep)
                {
                    goto IL_01c5;
                }

                if (colonist.GetCaravan() != null)
                {
                    Pawn_NeedsTracker needs = colonist.needs;
                    if (needs != null && needs.rest?.Resting == true)
                    {
                        goto IL_01c5;
                    }
                }

                if (colonist.CurJob != null && colonist.CurJob.def == JobDefOf.FleeAndCower)
                {
                    tmpIconsToDraw.Add(new IconDrawCall(Icon_Fleeing, "ActivityIconFleeing".Translate()));
                }
                else if (flag)
                {
                    tmpIconsToDraw.Add(new IconDrawCall(Icon_Attacking, "ActivityIconAttacking".Translate()));
                }
                else if (colonist.mindState.IsIdle && GenDate.DaysPassed >= 1)
                {
                    tmpIconsToDraw.Add(new IconDrawCall(Icon_Idle, "ActivityIconIdle".Translate()));
                }
            }

            goto IL_02b4;
        IL_01c5:
            tmpIconsToDraw.Add(new IconDrawCall(Icon_Sleeping, "ActivityIconSleeping".Translate()));
            goto IL_02b4;
        IL_02b4:
            if (colonist.IsBurning())
            {
                tmpIconsToDraw.Add(new IconDrawCall(Icon_Burning, "ActivityIconBurning".Translate()));
            }

            if (colonist.Inspired)
            {
                tmpIconsToDraw.Add(new IconDrawCall(Icon_Inspired, colonist.InspirationDef.LabelCap));
            }

            if (colonist.IsSlaveOfColony)
            {
                tmpIconsToDraw.Add(new IconDrawCall(colonist.guest.GetIcon()));
            }
            else
            {
                bool flag2 = false;
                if (colonist.Ideo != null)
                {
                    Ideo ideo = colonist.Ideo;
                    Precept_Role role = ideo.GetRole(colonist);
                    if (role != null)
                    {
                        tmpIconsToDraw.Add(new IconDrawCall(role.Icon, null, ideo.Color));
                        flag2 = true;
                    }
                }

                if (!flag2)
                {
                    Faction faction = null;
                    if (colonist.HasExtraMiniFaction())
                    {
                        faction = colonist.GetExtraMiniFaction();
                    }
                    else if (colonist.HasExtraHomeFaction())
                    {
                        faction = colonist.GetExtraHomeFaction();
                    }

                    if (faction != null)
                    {
                        tmpIconsToDraw.Add(new IconDrawCall(faction.def.FactionIcon, null, faction.Color));
                    }
                }
            }

            float num = Mathf.Min(BaseIconAreaWidth / (float)tmpIconsToDraw.Count, BaseIconMaxSize) * ColonistBar.Scale;
            Vector2 pos = new Vector2(rect.x + 1f, rect.yMax - num - 1f);
            foreach (IconDrawCall item in tmpIconsToDraw)
            {
                GUI.color = item.color ?? Color.white;
                DrawIcon(item.texture, ref pos, num, item.tooltip);
                GUI.color = Color.white;
            }
        }

        private static void DrawIcon(Texture2D icon, ref Vector2 pos, float iconSize, string tooltip = null)
        {
            Rect rect = new Rect(pos.x, pos.y, iconSize, iconSize);
            GUI.DrawTexture(rect, (Texture)icon);
            if (tooltip != null)
            {
                TooltipHandler.TipRegion(rect, tooltip);
            }

            pos.x += iconSize;
        }

        private static void DrawSelectionOverlayOnGUI(Pawn colonist, Rect rect)
        {
            Thing target = colonist;
            if (colonist.Dead)
            {
                target = colonist.Corpse;
            }

            SelectionDrawerUtility.DrawSelectionOverlayOnGUI(target, rect, 0.4f * ColonistBar.Scale, 20f * ColonistBar.Scale);
        }

        private static void DrawCaravanSelectionOverlayOnGUI(Caravan caravan, Rect rect)
        {
            SelectionDrawerUtility.DrawSelectionOverlayOnGUI(caravan, rect, 0.4f * ColonistBar.Scale, 20f * ColonistBar.Scale);
        }
    }
}
