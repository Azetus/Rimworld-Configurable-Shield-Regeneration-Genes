using UnityEngine;
using Verse;

namespace CSRG_ConfigurableShieldRegenerationGenes.Settings;

public static class CSRGModSettingsUI
{
    private static Vector2 scrollPosition;

    public static void SettingsWindowContents(Rect inRect)
    {
        var s = ConfigurableShieldRegenerationGenes.settings;

        float contentHeight = 1000f;
        Rect viewRect = new Rect(0f, 0f, inRect.width - 16f, contentHeight);
        Widgets.BeginScrollView(inRect, ref scrollPosition, viewRect);

        var listing = new Listing_Standard();
        listing.Begin(viewRect);

        listing.Label("CSRG.ShieldSettings".Translate());
        listing.Gap();
        listing.GapLine(12f);

        CSRGSliderInput.SliderWithInput(listing,
            "CSRG.ShieldMaxEnergy".Translate(), ref s.shieldMaxEnergy,
            10f, 1000f, "CSRG.ShieldMaxEnergyTip".Translate());

        CSRGSliderInput.SliderWithInput(listing,
            "CSRG.ShieldRegenRate".Translate() + " /tick", ref s.shieldRegenPerTick,
            0.01f, 100f, "CSRG.ShieldRegenRateTip".Translate());

        CSRGSliderInput.SliderWithInput(listing,
            "CSRG.ShieldBreakCooldown".Translate() + " ticks", ref s.shieldBreakCooldown,
            0f, 3600f, "CSRG.ShieldBreakCooldownTip".Translate());

        CSRGSliderInput.SliderWithInput(listing,
            "CSRG.ShieldEnergyOnReset".Translate(), ref s.shieldEnergyOnReset,
            0f, 1f, "CSRG.ShieldEnergyOnResetTip".Translate());

        CSRGSliderInput.SliderWithInput(listing,
            "CSRG.EnergyLossPerDamage".Translate(), ref s.energyLossPerDamage,
            0.001f, 1f, "CSRG.EnergyLossPerDamageTip".Translate());

        listing.Gap();
        listing.GapLine(12f);
        listing.Label("CSRG.RegenerationSettings".Translate());
        listing.Gap();
        listing.GapLine(12f);

        CSRGSliderInput.SliderWithInput(listing,
            "CSRG.RegenHealInterval".Translate() + " ticks", ref s.regenHealInterval,
            15f, 72000f, "CSRG.RegenHealIntervalTip".Translate());

        CSRGSliderInput.SliderWithInput(listing,
            "CSRG.RegenHealAmount".Translate(), ref s.regenHealAmount,
            1f, 100f, "CSRG.RegenHealAmountTip".Translate());

        CSRGSliderInput.SliderWithInput(listing,
            "CSRG.RegenSeverityPercent".Translate(), ref s.regenRegeneratingSeverityPercent,
            0.1f, 0.95f, "CSRG.RegenSeverityPercentTip".Translate());

        listing.Gap();
        listing.GapLine(12f);
        listing.Label("CSRG.BurstSettings".Translate());
        listing.Gap();
        listing.GapLine(12f);

        CSRGSliderInput.SliderWithInput(listing,
            "CSRG.BurstInterval".Translate() + " ticks", ref s.regenBurstInterval,
            10f, 150f, "CSRG.BurstIntervalTip".Translate());

        CSRGSliderInput.SliderWithInput(listing,
            "CSRG.BurstDuration".Translate() + " ticks", ref s.regenBurstDuration,
            60f, 7200f, "CSRG.BurstDurationTip".Translate());

        CSRGSliderInput.SliderWithInput(listing,
            "CSRG.BurstCooldown".Translate() + " ticks", ref s.regenBurstCooldown,
            6000f, 3600000f, "CSRG.BurstCooldownTip".Translate());

        CSRGSliderInput.SliderWithInput(listing,
            "CSRG.BurstPoolMultiplier".Translate(), ref s.regenBurstPoolMultiplier,
            1f, 10f, "CSRG.BurstPoolMultiplierTip".Translate());

        listing.Gap();
        listing.GapLine(12f);
        listing.Label("CSRG.LongTermRegen".Translate());
        listing.Gap();
        listing.GapLine(12f);

        CSRGSliderInput.SliderWithInput(listing,
            "CSRG.ScarInterval".Translate() + " ticks", ref s.regenScarInterval,
            60000f, 3600000f, "CSRG.ScarIntervalTip".Translate());

        CSRGSliderInput.SliderWithInput(listing,
            "CSRG.LimbInterval".Translate() + " ticks", ref s.regenLimbInterval,
            60000f, 3600000f, "CSRG.LimbIntervalTip".Translate());

        listing.Gap();
        if (listing.ButtonText("CSRG.ResetToDefaults".Translate()))
            s.ResetSettings();

        listing.End();
        Widgets.EndScrollView();
    }
}
