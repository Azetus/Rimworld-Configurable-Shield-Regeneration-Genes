using RimWorld;
using Verse;

namespace CSRG_ConfigurableShieldRegenerationGenes.Genes;

public class CSRG_RegenerativeGene : Gene
{
    private int ticksToHeal;
    private int ticksToRegrow;

    private int HealInterval => (int)ConfigurableShieldRegenerationGenes.settings.regenHealInterval;
    private int RegrowInterval => (int)ConfigurableShieldRegenerationGenes.settings.regenRegrowInterval;
    private float HealAmount => ConfigurableShieldRegenerationGenes.settings.regenHealAmount;
    private bool CanHealScars => ConfigurableShieldRegenerationGenes.settings.regenCanHealScars;
    private bool CanHealLimbs => ConfigurableShieldRegenerationGenes.settings.regenCanHealLimbs;

    public override void PostAdd()
    {
        base.PostAdd();
        ticksToHeal = HealInterval;
        ticksToRegrow = RegrowInterval;
    }

    public override void TickInterval(int delta)
    {
        base.TickInterval(delta);
        ticksToHeal -= delta;
        ticksToRegrow -= delta;

        if (ticksToHeal <= 0)
        {
            TryStopBleeding();
            TryRestoreBloodLoss();
            TryHealWound();
            ticksToHeal = HealInterval;
        }

        if (ticksToRegrow <= 0)
        {
            if (CanHealScars)
                HediffComp_HealPermanentWounds.TryHealRandomPermanentWound(pawn, LabelCap);
            if (CanHealLimbs)
                TryRegenerateLimb();
            ticksToRegrow = RegrowInterval;
        }
    }

    private void TryStopBleeding()
    {
        Hediff_Injury worstBleeder = null;
        float maxBleed = 0f;

        foreach (var h in pawn.health.hediffSet.hediffs)
        {
            if (h is Hediff_Injury injury && !injury.IsPermanent() && injury.BleedRate > 0f)
            {
                if (injury.BleedRate > maxBleed)
                {
                    maxBleed = injury.BleedRate;
                    worstBleeder = injury;
                }
            }
        }

        if (worstBleeder != null)
            worstBleeder.Heal(HealAmount);
    }

    private void TryRestoreBloodLoss()
    {
        var bloodLoss = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.BloodLoss);
        if (bloodLoss != null && bloodLoss.Severity > 0f)
            bloodLoss.Severity -= HealAmount * 0.1f;
    }

    private void TryHealWound()
    {
        Hediff_Injury worst = null;
        float maxSev = 0f;

        foreach (var h in pawn.health.hediffSet.hediffs)
        {
            if (h is Hediff_Injury injury && injury.CanHealNaturally()
                && !injury.IsPermanent() && injury.BleedRate <= 0f)
            {
                if (injury.Severity > maxSev)
                {
                    maxSev = injury.Severity;
                    worst = injury;
                }
            }
        }

        if (worst != null)
            worst.Heal(HealAmount);
    }

    private void TryRegenerateLimb()
    {
        var missingParts = pawn.health.hediffSet.GetMissingPartsCommonAncestors();
        if (missingParts.NullOrEmpty()) return;

        BodyPartRecord biggestMissing = null;
        foreach (var missing in missingParts)
        {
            if (pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(missing.Part))
                continue;
            if (biggestMissing == null
                || missing.Part.coverageAbsWithChildren > biggestMissing.coverageAbsWithChildren)
            {
                biggestMissing = missing.Part;
            }
        }

        if (biggestMissing != null)
            pawn.health.RestorePart(biggestMissing);
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref ticksToHeal, "ticksToHeal", 0);
        Scribe_Values.Look(ref ticksToRegrow, "ticksToRegrow", 0);
    }
}
