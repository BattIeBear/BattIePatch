using System;
using UnityEngine;
using Verse;

namespace BattIePatch_BetterFactionColors
{
    public class BattIePatch_FactionColorPicker : Window
    {
        private Color color;
        private Action<Color> onChanged;
        private string hexInput;
        private float lastR;
        private float lastG;
        private float lastB;
        private string lastHexInput;

        public BattIePatch_FactionColorPicker(Color initial, Action<Color> onChanged)
        {
            this.color = initial;
            this.onChanged = onChanged;

            hexInput = ColorUtility.ToHtmlStringRGB(initial);

            lastR = initial.r;
            lastG = initial.g;
            lastB = initial.b;
            lastHexInput = hexInput;

            doCloseButton = true;
            closeOnClickedOutside = true;
        }

        public override void DoWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);

            listingStandard.Label("Set New Color");

            float red = color.r;
            float green = color.g;
            float blue = color.b;

            // R
            Rect redRow = listingStandard.GetRect(24f);
            Widgets.Label(new Rect(redRow.x, redRow.y, 50f, redRow.height), $"R {Mathf.RoundToInt(red * 255f)}");
            red = Widgets.HorizontalSlider(new Rect(redRow.x + 55f, redRow.y, redRow.width - 55f, redRow.height), red, 0f, 1f);

            // G
            Rect greenRow = listingStandard.GetRect(24f);
            Widgets.Label(new Rect(greenRow.x, greenRow.y, 50f, greenRow.height), $"G {Mathf.RoundToInt(green * 255f)}");
            green = Widgets.HorizontalSlider(new Rect(greenRow.x + 55f, greenRow.y, greenRow.width - 55f, greenRow.height), green, 0f, 1f);

            // B
            Rect blueRow = listingStandard.GetRect(24f);
            Widgets.Label(new Rect(blueRow.x, blueRow.y, 50f, blueRow.height), $"B {Mathf.RoundToInt(blue * 255f)}");
            blue = Widgets.HorizontalSlider(new Rect(blueRow.x + 55f, blueRow.y, blueRow.width - 55f, blueRow.height), blue, 0f, 1f);

            //(Hex input HERE)
            Rect hexRow = listingStandard.GetRect(24f);
            Widgets.Label(new Rect(hexRow.x, hexRow.y, 40f, hexRow.height), "BattIePatch_BetterFactionColors_Hexcode".Translate());
            hexInput = Widgets.TextField(new Rect(hexRow.x + 45f, hexRow.y, hexRow.width - 45f, hexRow.height), hexInput);

            bool sliderChanged = !(Mathf.Approximately(red, lastR) && Mathf.Approximately(green, lastG) && Mathf.Approximately(blue, lastB));

            if (sliderChanged)
            {
                color = new Color(red, green, blue, 1f);

                lastR = red;
                lastG = green;
                lastB = blue;

                hexInput = ColorUtility.ToHtmlStringRGB(color);
                lastHexInput = hexInput;
            }
            else if (hexInput != lastHexInput)
            {
                string clean = hexInput.Trim();

                if (clean.StartsWith("#"))
                {
                    clean = clean.Substring(1);
                }

                // Only accept full hex (6 chars) not counting spaces or hashtags
                if (clean.Length == 6 && ColorUtility.TryParseHtmlString("#" + clean, out Color parsed))
                {
                    color = new Color(parsed.r, parsed.g, parsed.b, 1f);

                    red = lastR = color.r;
                    green = lastG = color.g;
                    blue = lastB = color.b;
                }
                lastHexInput = hexInput;
            }
            Rect preview = listingStandard.GetRect(30f);
            Widgets.DrawBoxSolid(preview, color);

            if (listingStandard.ButtonText("Apply"))
            {
                Apply();
                Close();
            }

            listingStandard.End();
        }

        public override void OnAcceptKeyPressed()
        {
            GUI.FocusControl(null);
            Apply();
        }

        void Apply()
        {
            onChanged?.Invoke(color);
        }
    }
}