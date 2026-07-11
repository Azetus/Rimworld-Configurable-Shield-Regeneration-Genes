using RimWorld;
using Verse;

namespace CSRG_ConfigurableShieldRegenerationGenes.Genes;

[DefOf]
public static class CSRG_DefOf
{
    public static HediffDef CSRG_RegeneratingInjury;
    public static HediffDef CSRG_RegenerationBurst;

    static CSRG_DefOf()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(CSRG_DefOf));
    }
}
