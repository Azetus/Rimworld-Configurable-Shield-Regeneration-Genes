using Verse;

namespace CSRG_ConfigurableShieldRegenerationGenes.Settings;

public class CSRGModSettings : ModSettings
{
    public float shieldMaxEnergy = 100f;
    public float shieldRegenPerTick = 0.1f;
    public float shieldBreakCooldown = 300f;
    public float shieldEnergyOnReset = 0.2f;
    public float energyLossPerDamage = 1.0f;
    public float regenHealInterval = 1200f;
    public float regenRegrowInterval = 900000f;
    public float regenHealAmount = 1.0f;
    public bool regenCanHealScars = true;
    public bool regenCanHealLimbs = true;

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref shieldMaxEnergy, nameof(shieldMaxEnergy), 100f);
        Scribe_Values.Look(ref shieldRegenPerTick, nameof(shieldRegenPerTick), 0.1f);
        Scribe_Values.Look(ref shieldBreakCooldown, nameof(shieldBreakCooldown), 300f);
        Scribe_Values.Look(ref shieldEnergyOnReset, nameof(shieldEnergyOnReset), 0.2f);
        Scribe_Values.Look(ref energyLossPerDamage, nameof(energyLossPerDamage), 1.0f);
        Scribe_Values.Look(ref regenHealInterval, nameof(regenHealInterval), 1200f);
        Scribe_Values.Look(ref regenRegrowInterval, nameof(regenRegrowInterval), 900000f);
        Scribe_Values.Look(ref regenHealAmount, nameof(regenHealAmount), 1.0f);
        Scribe_Values.Look(ref regenCanHealScars, nameof(regenCanHealScars), true);
        Scribe_Values.Look(ref regenCanHealLimbs, nameof(regenCanHealLimbs), true);
    }

    public void ResetSettings()
    {
        shieldMaxEnergy = 100f;
        shieldRegenPerTick = 0.1f;
        shieldBreakCooldown = 300f;
        shieldEnergyOnReset = 0.2f;
        energyLossPerDamage = 1.0f;
        regenHealInterval = 1200f;
        regenRegrowInterval = 900000f;
        regenHealAmount = 1.0f;
        regenCanHealScars = true;
        regenCanHealLimbs = true;
    }
}