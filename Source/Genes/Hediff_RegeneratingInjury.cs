using Verse;

namespace CSRG_ConfigurableShieldRegenerationGenes.Genes;

public class Hediff_RegeneratingInjury : HediffWithComps
{
    public override bool ShouldRemove => Severity <= 0f;
}
