using UnityEngine;
using Verse;

namespace CSRG_ConfigurableShieldRegenerationGenes.Settings;

public static class CSRGModSettingsUI
{
    public static void SettingsWindowContents(Rect inRect)
    {
        var s = ConfigurableShieldRegenerationGenes.settings;
        var listing = new Listing_Standard();
        listing.Begin(inRect);

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
        if (listing.ButtonText("CSRG.ResetToDefaults".Translate()))
            s.ResetSettings();

        listing.End();
    }
}