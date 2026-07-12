using System.Collections.Generic;
using CSRG_ConfigurableShieldRegenerationGenes.Hediffs;
using RimWorld;
using UnityEngine;
using Verse;

namespace CSRG_ConfigurableShieldRegenerationGenes.Genes;

public class CSRG_RegenerativeGene : Gene
{
    private int HealInterval => (int)ConfigurableShieldRegenerationGenes.settings.regenHealInterval;
    private float HealAmount => ConfigurableShieldRegenerationGenes.settings.regenHealAmount;
    private float SeverityPercent => ConfigurableShieldRegenerationGenes.settings.regenRegeneratingSeverityPercent;
    private int BurstInterval => (int)ConfigurableShieldRegenerationGenes.settings.regenBurstInterval;
    private int BurstDuration => (int)ConfigurableShieldRegenerationGenes.settings.regenBurstDuration;
    private float BurstMultiplier => ConfigurableShieldRegenerationGenes.settings.regenBurstPoolMultiplier;
    private float BurstThreshold => ConfigurableShieldRegenerationGenes.settings.regenBurstThreshold;
    private bool BurstEnabled => ConfigurableShieldRegenerationGenes.settings.regenBurstEnabled;
    private int ScarInterval => (int)(ConfigurableShieldRegenerationGenes.settings.regenScarIntervalDays * 60000f);
    private int LimbInterval => (int)(ConfigurableShieldRegenerationGenes.settings.regenLimbIntervalDays * 60000f);

    public override void Tick()
    {
        base.Tick();

        var burst = pawn.health.hediffSet
            .GetFirstHediffOfDef(CSRG_DefOf.CSRG_RegenerationBurst)
            as Hediff_RegenerationBurst;

        if (burst == null || burst.inCooldown)
        {
            if (pawn.IsHashIntervalTick(HealInterval))
                RegenerationPulse(healRegenerating: true, poolMultiplier: 1f);
        }

        if (BurstEnabled)
        {
            if (burst == null
                && pawn.health.summaryHealth.SummaryHealthPercent < BurstThreshold
                && pawn.health.hediffSet.HasNaturallyHealingInjury()
                && Find.TickManager.TicksGame - pawn.mindState.lastHarmTick < BurstDuration)
            {
                var h = (Hediff_RegenerationBurst)HediffMaker.MakeHediff(
                    CSRG_DefOf.CSRG_RegenerationBurst, pawn);
                h.InitBurst(BurstDuration);
                pawn.health.AddHediff(h);
            }
            else if (burst != null && !burst.inCooldown
                && pawn.IsHashIntervalTick(BurstInterval))
            {
                RegenerationPulse(healRegenerating: false, poolMultiplier: BurstMultiplier);
            }
        }

        if (pawn.IsHashIntervalTick(ScarInterval))
            HediffComp_HealPermanentWounds.TryHealRandomPermanentWound(pawn, LabelCap);

        if (pawn.IsHashIntervalTick(LimbInterval))
            RegenerateLimb();
    }

    private void RegenerationPulse(bool healRegenerating, float poolMultiplier)
    {
        float pool = HealAmount * poolMultiplier;

        TendWounds();
        HealNormalWounds(ref pool);
        if (healRegenerating)
            HealRegeneratingWounds(ref pool);

        RestoreBloodLoss();
    }

    private void TendWounds()
    {
        Hediff? tendTarget = null;
        float maxPriority = 0f;

        foreach (var h in pawn.health.hediffSet.hediffs)
        {
            if (h.TendableNow() && h.TendPriority > maxPriority)
            {
                maxPriority = h.TendPriority;
                tendTarget = h;
            }
        }

        if (tendTarget != null)
            tendTarget.Tended(1f, 1f, 0);
    }

    private bool HealNormalWounds(ref float pool)
    {
        var candidates = new List<Hediff_Injury>();
        foreach (var h in pawn.health.hediffSet.hediffs)
        {
            if (h is Hediff_Injury injury
                && !(h is Hediff_RegeneratingInjury)
                && injury.CanHealNaturally()
                && !injury.IsPermanent())
                candidates.Add(injury);
        }

        return ConsumePool(candidates, ref pool);
    }

    private bool HealRegeneratingWounds(ref float pool)
    {
        var candidates = new List<Hediff_RegeneratingInjury>();
        foreach (var h in pawn.health.hediffSet.hediffs)
        {
            if (h is Hediff_RegeneratingInjury regenInjury)
                candidates.Add(regenInjury);
        }

        if (candidates.Count == 0 || pool <= 0f) return false;

        foreach (var regenInjury in candidates)
        {
            float maxHealth = regenInjury.Part?.def.GetMaxHealth(pawn) ?? 1f;
            float healHP = Mathf.Min(pool, regenInjury.Severity * maxHealth);
            regenInjury.Severity -= healHP / maxHealth;
            pool -= healHP;
            if (pool <= 0f) break;
        }

        pawn.health.Notify_HediffChanged(candidates[candidates.Count - 1]);
        return true;
    }

    private bool ConsumePool(List<Hediff_Injury> candidates, ref float pool)
    {
        if (candidates.Count == 0 || pool <= 0f) return false;
        foreach (var injury in candidates)
        {
            float heal = Mathf.Min(pool, injury.Severity);
            injury.Heal(heal);
            pool -= heal;
            if (pool <= 0f) break;
        }

        return true;
    }

    private void RestoreBloodLoss()
    {
        var bloodLoss = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.BloodLoss);
        if (bloodLoss != null && bloodLoss.Severity > 0f)
            bloodLoss.Severity -= HealAmount * 0.1f;
    }

    private void RegenerateLimb()
    {
        var missingParts = pawn.health.hediffSet.GetMissingPartsCommonAncestors();
        if (missingParts.NullOrEmpty()) return;

        foreach (var missing in missingParts)
        {
            if (pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(missing.Part))
                continue;

            var part = missing.Part;
            pawn.health.RestorePart(part);

            var newborn = (Hediff_RegeneratingInjury)HediffMaker.MakeHediff(
                CSRG_DefOf.CSRG_RegeneratingInjury, pawn, part);
            newborn.Severity = SeverityPercent;
            pawn.health.AddHediff(newborn);
            return;
        }
    }
}
