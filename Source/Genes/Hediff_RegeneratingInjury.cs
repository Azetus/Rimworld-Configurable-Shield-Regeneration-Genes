using Verse;

namespace CSRG_ConfigurableShieldRegenerationGenes.Genes;

public class Hediff_RegeneratingInjury : Hediff_Injury
{
    public override float BleedRate => 0f;
    public override float PainOffset => 0f;
    public override bool ShouldRemove => Severity <= 0f;
}
