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
    public float regenHealAmount = 2f;
    public float regenRegeneratingSeverityPercent = 0.8f;
    public float regenTissueDecayPerDay = 0.2f;
    public float regenBurstInterval = 15f;
    public float regenBurstDuration = 900f;
    public float regenBurstCooldownDays = 1f;
    public float regenBurstPoolMultiplier = 1f;
    public bool regenBurstEnabled = true;
    public float regenBurstThreshold = 0.85f;
    public float regenScarIntervalDays = 5f;
    public float regenLimbIntervalDays = 7f;

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref shieldMaxEnergy, nameof(shieldMaxEnergy), 100f);
        Scribe_Values.Look(ref shieldRegenPerTick, nameof(shieldRegenPerTick), 0.1f);
        Scribe_Values.Look(ref shieldBreakCooldown, nameof(shieldBreakCooldown), 300f);
        Scribe_Values.Look(ref shieldEnergyOnReset, nameof(shieldEnergyOnReset), 0.2f);
        Scribe_Values.Look(ref energyLossPerDamage, nameof(energyLossPerDamage), 1.0f);
        Scribe_Values.Look(ref regenHealInterval, nameof(regenHealInterval), 1200f);
        Scribe_Values.Look(ref regenHealAmount, nameof(regenHealAmount), 2f);
        Scribe_Values.Look(ref regenRegeneratingSeverityPercent, nameof(regenRegeneratingSeverityPercent), 0.8f);
        Scribe_Values.Look(ref regenTissueDecayPerDay, nameof(regenTissueDecayPerDay), 0.2f);
        Scribe_Values.Look(ref regenBurstInterval, nameof(regenBurstInterval), 15f);
        Scribe_Values.Look(ref regenBurstDuration, nameof(regenBurstDuration), 900f);
        Scribe_Values.Look(ref regenBurstCooldownDays, nameof(regenBurstCooldownDays), 1f);
        Scribe_Values.Look(ref regenBurstPoolMultiplier, nameof(regenBurstPoolMultiplier), 1f);
        Scribe_Values.Look(ref regenBurstEnabled, nameof(regenBurstEnabled), true);
        Scribe_Values.Look(ref regenBurstThreshold, nameof(regenBurstThreshold), 0.85f);
        Scribe_Values.Look(ref regenScarIntervalDays, nameof(regenScarIntervalDays), 5f);
        Scribe_Values.Look(ref regenLimbIntervalDays, nameof(regenLimbIntervalDays), 7f);
    }

    public void ResetSettings()
    {
        shieldMaxEnergy = 100f;
        shieldRegenPerTick = 0.1f;
        shieldBreakCooldown = 300f;
        shieldEnergyOnReset = 0.2f;
        energyLossPerDamage = 1.0f;
        regenHealInterval = 1200f;
        regenHealAmount = 2f;
        regenRegeneratingSeverityPercent = 0.8f;
        regenTissueDecayPerDay = 0.2f;
        regenBurstInterval = 15f;
        regenBurstDuration = 900f;
        regenBurstCooldownDays = 1f;
        regenBurstPoolMultiplier = 1f;
        regenBurstEnabled = true;
        regenBurstThreshold = 0.85f;
        regenScarIntervalDays = 5f;
        regenLimbIntervalDays = 7f;
    }
}
