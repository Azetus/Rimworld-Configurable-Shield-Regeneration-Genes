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

    private readonly List<Hediff_Injury> normalWoundBuffer = new();
    private readonly List<Hediff_RegeneratingInjury> regenWoundBuffer = new();

    public override void Tick()
    {
        base.Tick();
        if (pawn?.health?.hediffSet == null) return;

        bool isHealTick = pawn.IsHashIntervalTick(HealInterval);
        bool isBurstPulseTick = BurstEnabled && pawn.IsHashIntervalTick(BurstInterval);
        bool isScarTick = pawn.IsHashIntervalTick(ScarInterval);
        bool isLimbTick = pawn.IsHashIntervalTick(LimbInterval);
        bool needFullClassify = isHealTick || isBurstPulseTick;

        if (!needFullClassify && !isScarTick && !isLimbTick)
            return;

        Hediff_RegenerationBurst? burst = null;
        Hediff? tendTarget = null;
        float maxTendPriority = float.MinValue;
        Hediff? bloodLoss = null;

        if (needFullClassify)
        {
            normalWoundBuffer.Clear();
            regenWoundBuffer.Clear();
        }

        foreach (var h in pawn.health.hediffSet.hediffs)
        {
            if (h == null) continue;

            if (BurstEnabled && h is Hediff_RegenerationBurst burstCandidate)
                burst = burstCandidate;

            if (!needFullClassify) continue;

            if (h.TendableNow() && h.TendPriority > maxTendPriority)
            {
                maxTendPriority = h.TendPriority;
                tendTarget = h;
            }

            if (h is Hediff_Injury injury
                && !(h is Hediff_RegeneratingInjury)
                && injury.CanHealNaturally()
                && !injury.IsPermanent())
                normalWoundBuffer.Add(injury);

            if (h is Hediff_RegeneratingInjury regenInjury)
                regenWoundBuffer.Add(regenInjury);

            if (h.def == HediffDefOf.BloodLoss)
                bloodLoss = h;
        }

        if (isHealTick && (burst == null || burst.inCooldown))
            ExecuteHealing(tendTarget, bloodLoss, healRegenerating: true, poolMultiplier: 1f);

        if (BurstEnabled)
        {
            if (burst == null
                && pawn.health.summaryHealth.SummaryHealthPercent < BurstThreshold
                && pawn.health.hediffSet.HasNaturallyHealingInjury()
                && pawn.mindState != null
                && Find.TickManager.TicksGame - pawn.mindState.lastHarmTick < BurstDuration)
            {
                var h = (Hediff_RegenerationBurst)HediffMaker.MakeHediff(
                    CSRG_DefOf.CSRG_RegenerationBurst, pawn);
                if (h != null)
                {
                    h.InitBurst(BurstDuration);
                    pawn.health.AddHediff(h);
                }
            }
            else if (isBurstPulseTick && burst != null && !burst.inCooldown)
            {
                ExecuteHealing(tendTarget, bloodLoss, healRegenerating: false, poolMultiplier: BurstMultiplier);
            }
        }

        if (isScarTick)
            HediffComp_HealPermanentWounds.TryHealRandomPermanentWound(pawn, LabelCap);

        if (isLimbTick)
            RegenerateLimb();
    }

    private void ExecuteHealing(Hediff? tendTarget, Hediff? bloodLoss, bool healRegenerating, float poolMultiplier)
    {
        float pool = HealAmount * poolMultiplier;

        if (tendTarget != null)
            tendTarget.Tended(1f, 1f, 0);

        ConsumePool(normalWoundBuffer, ref pool);

        if (healRegenerating)
            HealRegeneratingWoundsFromBuffer(ref pool);

        if (bloodLoss != null && bloodLoss.Severity > 0f)
            bloodLoss.Severity -= HealAmount * 0.1f;
    }

    private void HealRegeneratingWoundsFromBuffer(ref float pool)
    {
        if (regenWoundBuffer.Count == 0 || pool <= 0f) return;

        bool healed = false;
        foreach (var regenInjury in regenWoundBuffer)
        {
            if (regenInjury == null) continue;
            float maxHealth = regenInjury.Part?.def.GetMaxHealth(pawn) ?? 1f;
            float healHP = Mathf.Min(pool, regenInjury.Severity * maxHealth);
            if (healHP > 0f)
            {
                regenInjury.Severity -= healHP / maxHealth;
                pool -= healHP;
                healed = true;
                if (pool <= 0f) break;
            }
        }

        if (healed)
            pawn.health.Notify_HediffChanged(regenWoundBuffer[regenWoundBuffer.Count - 1]);
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
            if (newborn == null) return;
            newborn.Severity = SeverityPercent;
            pawn.health.AddHediff(newborn);
            return;
        }
    }
}