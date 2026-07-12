using UnityEngine;
using Verse;

namespace CSRG_ConfigurableShieldRegenerationGenes.Settings;

public static class CSRGModSettingsUI
{
    private static Vector2 scrollPosition;

    public static void SettingsWindowContents(Rect inRect)
    {
        var s = ConfigurableShieldRegenerationGenes.settings;

        float contentHeight = 1300f;
        Rect viewRect = new Rect(0f, 0f, inRect.width - 16f, contentHeight);
        Widgets.BeginScrollView(inRect, ref scrollPosition, viewRect);

        var listing = new Listing_Standard();
        listing.Begin(viewRect);

        listing.GapLine(24f);
        SectionHeader(listing, "CSRG.ShieldGeneLabel".Translate(), major: true);

        CSRGSliderInput.SliderWithInput(listing,
            "CSRG.ShieldMaxEnergy".Translate(), ref s.shieldMaxEnergy,
            10f, 1000f, "CSRG.ShieldMaxEnergyTip".Translate());

        CSRGSliderInput.SliderWithInput(listing,
            "CSRG.ShieldRegenRate".Translate(), ref s.shieldRegenPerTick,
            0.01f, 100f, "CSRG.ShieldRegenRateTip".Translate());

        CSRGSliderInput.SliderWithInput(listing,
            "CSRG.ShieldBreakCooldown".Translate(), ref s.shieldBreakCooldown,
            0f, 3600f, "CSRG.ShieldBreakCooldownTip".Translate());

        CSRGSliderInput.SliderWithInput(listing,
            "CSRG.ShieldEnergyOnReset".Translate(), ref s.shieldEnergyOnReset,
            0f, 1f, "CSRG.ShieldEnergyOnResetTip".Translate());

        CSRGSliderInput.SliderWithInput(listing,
            "CSRG.EnergyLossPerDamage".Translate(), ref s.energyLossPerDamage,
            0.001f, 1f, "CSRG.EnergyLossPerDamageTip".Translate());

        listing.GapLine(24f);
        SectionHeader(listing, "CSRG.RegenerativeGeneLabel".Translate(), major: true);

        SectionHeader(listing, "CSRG.RegenerationSettings".Translate());

        CSRGSliderInput.SliderWithInput(listing,
            "CSRG.RegenHealInterval".Translate(), ref s.regenHealInterval,
            15f, 72000f, "CSRG.RegenHealIntervalTip".Translate());

        CSRGSliderInput.SliderWithInput(listing,
            "CSRG.RegenHealAmount".Translate(), ref s.regenHealAmount,
            1f, 100f, "CSRG.RegenHealAmountTip".Translate());

        CSRGSliderInput.SliderWithInput(listing,
            "CSRG.RegenSeverityPercent".Translate(), ref s.regenRegeneratingSeverityPercent,
            0.1f, 0.95f, "CSRG.RegenSeverityPercentTip".Translate());

        SectionHeader(listing, "CSRG.LongTermRegen".Translate());

        CSRGSliderInput.SliderWithInput(listing,
            "CSRG.ScarInterval".Translate(), ref s.regenScarInterval,
            0.01f, 30f, "CSRG.ScarIntervalTip".Translate());

        CSRGSliderInput.SliderWithInput(listing,
            "CSRG.LimbInterval".Translate(), ref s.regenLimbInterval,
            0.01f, 30f, "CSRG.LimbIntervalTip".Translate());

        SectionHeader(listing, "CSRG.BurstSettings".Translate());

        CSRGSliderInput.SliderWithInput(listing,
            "CSRG.BurstInterval".Translate(), ref s.regenBurstInterval,
            10f, 150f, "CSRG.BurstIntervalTip".Translate());

        CSRGSliderInput.SliderWithInput(listing,
            "CSRG.BurstDuration".Translate(), ref s.regenBurstDuration,
            60f, 7200f, "CSRG.BurstDurationTip".Translate());

        CSRGSliderInput.SliderWithInput(listing,
            "CSRG.BurstCooldown".Translate(), ref s.regenBurstCooldown,
            6000f, 3600000f, "CSRG.BurstCooldownTip".Translate());

        CSRGSliderInput.SliderWithInput(listing,
            "CSRG.BurstPoolMultiplier".Translate(), ref s.regenBurstPoolMultiplier,
            1f, 10f, "CSRG.BurstPoolMultiplierTip".Translate());

        listing.Gap();
        if (listing.ButtonText("CSRG.ResetToDefaults".Translate()))
            s.ResetSettings();

        listing.End();
        Widgets.EndScrollView();
    }

    private static void SectionHeader(Listing_Standard listing, string label, bool major = false)
    {
        Text.Font = GameFont.Medium;
        GUI.color = major ? Color.white : new Color(0.7f, 0.7f, 0.7f);
        listing.Label(label);
        GUI.color = Color.white;
        Text.Font = GameFont.Small;
        listing.Gap();
    }
}
