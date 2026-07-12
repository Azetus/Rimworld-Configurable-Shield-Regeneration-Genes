using Verse;

namespace CSRG_ConfigurableShieldRegenerationGenes.Hediffs;

public class Hediff_RegenerationBurst : HediffWithComps
{
    private int remainingTicks;
    public bool inCooldown;

    public void InitBurst(int duration)
    {
        remainingTicks = duration;
        inCooldown = false;
        Severity = 0f;
        SetVisible();
    }

    public override bool ShouldRemove => false;

    public override void Tick()
    {
        base.Tick();
        remainingTicks--;
        if (remainingTicks > 0) return;

        if (!inCooldown)
        {
            inCooldown = true;
            remainingTicks = (int)(ConfigurableShieldRegenerationGenes.settings.regenBurstCooldownDays * 60000f);
            Severity = 0.6f;
            pawn.health.Notify_HediffChanged(this);
        }
        else
        {
            pawn.health.RemoveHediff(this);
        }
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref remainingTicks, "remainingTicks", 0);
        Scribe_Values.Look(ref inCooldown, "inCooldown", false);
    }
}
