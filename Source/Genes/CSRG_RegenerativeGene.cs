using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace CSRG_ConfigurableShieldRegenerationGenes.Genes;

public class CSRG_RegenerativeGene : Gene
{
    private int HealInterval => (int)ConfigurableShieldRegenerationGenes.settings.regenHealInterval;
    private float HealAmount => ConfigurableShieldRegenerationGenes.settings.regenHealAmount;
    private float SeverityPercent => ConfigurableShieldRegenerationGenes.settings.regenRegeneratingSeverityPercent;

    public override void Tick()
    {
        base.Tick();
        if (pawn != null && pawn.IsHashIntervalTick(HealInterval))
            RegenerationPulse();
    }

    private void RegenerationPulse()
    {
        float pool = HealAmount;
        bool consumed = false;

        consumed |= HealBleedingWounds(ref pool);
        consumed |= HealNormalWounds(ref pool);
        consumed |= HealRegeneratingWounds(ref pool);

        RestoreBloodLoss();

        if (consumed && pool > 0f)
            HediffComp_HealPermanentWounds.TryHealRandomPermanentWound(pawn, LabelCap);

        if (!consumed)
            RegenerateLimb();
    }

    private bool HealBleedingWounds(ref float pool)
    {
        var candidates = new List<Hediff_Injury>();
        foreach (var h in pawn.health.hediffSet.hediffs)
        {
            if (h is Hediff_Injury injury
                && !(h is Hediff_RegeneratingInjury)
                && !injury.IsPermanent()
                && injury.BleedRate > 0f)
                candidates.Add(injury);
        }

        candidates.Sort((a, b) => b.BleedRate.CompareTo(a.BleedRate));
        return ConsumePool(candidates, ref pool);
    }

    private bool HealNormalWounds(ref float pool)
    {
        var candidates = new List<Hediff_Injury>();
        foreach (var h in pawn.health.hediffSet.hediffs)
        {
            if (h is Hediff_Injury injury
                && !(h is Hediff_RegeneratingInjury)
                && injury.CanHealNaturally()
                && !injury.IsPermanent()
                && injury.BleedRate <= 0f)
                candidates.Add(injury);
        }

        return ConsumePool(candidates, ref pool);
    }

    private bool HealRegeneratingWounds(ref float pool)
    {
        var candidates = new List<Hediff_Injury>();
        foreach (var h in pawn.health.hediffSet.hediffs)
        {
            if (h is Hediff_RegeneratingInjury injury
                && injury.CanHealNaturally())
                candidates.Add(injury);
        }

        return ConsumePool(candidates, ref pool);
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

            float maxHealth = part.def.GetMaxHealth(pawn);
            float severity = Mathf.Min(SeverityPercent * maxHealth, maxHealth - 1f);

            var newborn = (Hediff_RegeneratingInjury)HediffMaker.MakeHediff(
                HediffDef.Named("CSRG_RegeneratingInjury"), pawn, part);
            newborn.Severity = severity;
            pawn.health.AddHediff(newborn);
            return;
        }
    }
}