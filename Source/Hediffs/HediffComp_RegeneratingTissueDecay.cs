using Verse;

namespace CSRG_ConfigurableShieldRegenerationGenes.Hediffs;

public class HediffCompProperties_RegeneratingTissueDecay : HediffCompProperties_SeverityPerDay
{
    public HediffCompProperties_RegeneratingTissueDecay()
    {
        compClass = typeof(HediffComp_RegeneratingTissueDecay);
    }
}

public class HediffComp_RegeneratingTissueDecay : HediffComp_SeverityPerDay
{
    public override float SeverityChangePerDay()
    {
        return -ConfigurableShieldRegenerationGenes.settings.regenTissueDecayPerDay;
    }
}
