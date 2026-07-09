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

        s.shieldMaxEnergy = listing.SliderLabeled(
            "CSRG.ShieldMaxEnergy".Translate() + ": " + s.shieldMaxEnergy.ToString("F0"),
            s.shieldMaxEnergy, 10f, 1000f, 0.5f,
            "CSRG.ShieldMaxEnergyTip".Translate());

        s.shieldRegenPerTick = listing.SliderLabeled(
            "CSRG.ShieldRegenRate".Translate() + ": " + s.shieldRegenPerTick.ToString("F3") + "/tick",
            s.shieldRegenPerTick, 0.01f, 10f, 0.5f,
            "CSRG.ShieldRegenRateTip".Translate());

        s.shieldBreakCooldown = listing.SliderLabeled(
            "CSRG.ShieldBreakCooldown".Translate() + ": " + s.shieldBreakCooldown.ToString("F0") + " ticks",
            s.shieldBreakCooldown, 60f, 3600f, 0.5f,
            "CSRG.ShieldBreakCooldownTip".Translate());

        s.shieldEnergyOnReset = listing.SliderLabeled(
            "CSRG.ShieldEnergyOnReset".Translate() + ": " + (s.shieldEnergyOnReset * 100f).ToString("F0") + "%",
            s.shieldEnergyOnReset, 0f, 1f, 0.5f,
            "CSRG.ShieldEnergyOnResetTip".Translate());

        s.energyLossPerDamage = listing.SliderLabeled(
            "CSRG.EnergyLossPerDamage".Translate() + ": " + s.energyLossPerDamage.ToString("F3"),
            s.energyLossPerDamage, 0.001f, 0.5f, 0.5f,
            "CSRG.EnergyLossPerDamageTip".Translate());

        listing.Gap();
        if (listing.ButtonText("CSRG.ResetToDefaults".Translate()))
        {
            s.shieldMaxEnergy = 100f;
            s.shieldRegenPerTick = 0.1f;
            s.shieldBreakCooldown = 300f;
            s.shieldEnergyOnReset = 0.2f;
            s.energyLossPerDamage = 0.033f;
        }

        listing.End();
    }
}