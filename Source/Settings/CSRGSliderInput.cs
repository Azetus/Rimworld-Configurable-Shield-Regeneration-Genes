using UnityEngine;
using Verse;

namespace CSRG_ConfigurableShieldRegenerationGenes.Settings;

public static class CSRGSliderInput
{
    public static void SliderWithInput(
        Listing_Standard listing,
        string label,
        ref float value,
        float min,
        float max,
        string? tooltip = null)
    {
        var labelRect = listing.GetRect(Text.LineHeight);
        Widgets.Label(labelRect, label);
        if (tooltip != null)
            TooltipHandler.TipRegion(labelRect, tooltip);

        var rowRect = listing.GetRect(24f);
        var inputWidth = 70f;

        var sliderRect = new Rect(rowRect.x, rowRect.y,
            rowRect.width - inputWidth - 4f, rowRect.height);
        var inputRect = new Rect(sliderRect.xMax + 4f, rowRect.y,
            inputWidth, rowRect.height);

        value = Widgets.HorizontalSlider(sliderRect, value, min, max,
            true, value.ToString("0.00"));

        string buffer = value.ToString("0.00");
        Widgets.TextFieldNumeric(inputRect, ref value, ref buffer, min, max);

        listing.Gap(listing.verticalSpacing);
    }
}
