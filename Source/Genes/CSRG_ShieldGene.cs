using CSRG_ConfigurableShieldRegenerationGenes.Gizmos;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace CSRG_ConfigurableShieldRegenerationGenes.Genes;

public class CSRG_ShieldGene : Gene
{
    private float energy;
    private int ticksToReset = -1;
    private int lastKeepDisplayTick = -9999;
    private Vector3 impactAngleVect;
    private int lastAbsorbDamageTick = -9999;

    private const float MinDrawSize = 1.2f;
    private const float MaxDrawSize = 1.55f;

    public float MaxEnergy => ConfigurableShieldRegenerationGenes.settings.shieldMaxEnergy;
    private float RegenPerTick => ConfigurableShieldRegenerationGenes.settings.shieldRegenPerTick;
    private int BreakCooldown => (int)ConfigurableShieldRegenerationGenes.settings.shieldBreakCooldown;
    private float ResetEnergyRatio => ConfigurableShieldRegenerationGenes.settings.shieldEnergyOnReset;
    private float LossMultiplier => ConfigurableShieldRegenerationGenes.settings.energyLossPerDamage;

    public float Energy => energy;

    public ShieldState ShieldState
    {
        get
        {
            if (ticksToReset <= 0) return ShieldState.Active;
            return ShieldState.Resetting;
        }
    }

    public override void PostAdd()
    {
        base.PostAdd();
        energy = MaxEnergy * ResetEnergyRatio;
    }

    public override void Tick()
    {
        if (ShieldState == ShieldState.Resetting)
        {
            ticksToReset--;
            if (ticksToReset <= 0)
                Reset();
        }
        else if (ShieldState == ShieldState.Active)
        {
            energy += RegenPerTick;
            if (energy > MaxEnergy)
                energy = MaxEnergy;
        }
    }

    public void ProcessDamage(DamageInfo dinfo, ref bool absorbed)
    {
        absorbed = false;
        if (ShieldState != ShieldState.Active) return;

        energy -= dinfo.Amount * LossMultiplier;
        if (energy < 0f)
            Break();
        else
            AbsorbedDamage(dinfo);
        absorbed = true;
    }

    private void Break()
    {
        if (pawn != null && pawn.Spawned)
        {
            float scale = Mathf.Lerp(MinDrawSize, MaxDrawSize, Mathf.Max(0f, energy));
            EffecterDefOf.Shield_Break.SpawnAttached(pawn, pawn.MapHeld, scale);
            FleckMaker.Static(pawn.TrueCenter(), pawn.Map, FleckDefOf.ExplosionFlash, 12f);
            for (int i = 0; i < 6; i++)
                FleckMaker.ThrowDustPuff(
                    pawn.TrueCenter() + Vector3Utility.HorizontalVectorFromAngle(Rand.Range(0, 360)) * Rand.Range(0.3f, 0.6f),
                    pawn.Map, Rand.Range(0.8f, 1.2f));
        }
        energy = 0f;
        ticksToReset = BreakCooldown;
    }

    public override void Reset()
    {
        base.Reset();
        ticksToReset = -1;
        energy = MaxEnergy * ResetEnergyRatio;
    }

    private void AbsorbedDamage(DamageInfo dinfo)
    {
        if (pawn == null || !pawn.Spawned) return;
        SoundDefOf.EnergyShield_AbsorbDamage.PlayOneShot(new TargetInfo(pawn.Position, pawn.Map));
        impactAngleVect = Vector3Utility.HorizontalVectorFromAngle(dinfo.Angle);
        Vector3 loc = pawn.TrueCenter() + impactAngleVect.RotatedBy(180f) * 0.5f;
        float num = Mathf.Min(10f, 2f + dinfo.Amount / 10f);
        FleckMaker.Static(loc, pawn.Map, FleckDefOf.ExplosionFlash, num);
        int num2 = (int)num;
        for (int i = 0; i < num2; i++)
            FleckMaker.ThrowDustPuff(loc, pawn.Map, Rand.Range(0.8f, 1.2f));
        lastAbsorbDamageTick = Find.TickManager.TicksGame;
        KeepDisplaying();
    }

    public void KeepDisplaying()
    {
        lastKeepDisplayTick = Find.TickManager.TicksGame;
    }

    public override IEnumerable<Gizmo> GetGizmos()
    {
        if (!Active) yield break;
        if (Find.Selector.SingleSelectedThing != pawn) yield break;
        if (pawn.Faction != Faction.OfPlayer && !pawn.RaceProps.IsMechanoid) yield break;
        yield return new Gizmo_GeneShieldStatus(this);
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref energy, "shieldEnergy", 0f);
        Scribe_Values.Look(ref ticksToReset, "ticksToReset", -1);
        Scribe_Values.Look(ref lastKeepDisplayTick, "lastKeepDisplayTick", 0);
    }
}
