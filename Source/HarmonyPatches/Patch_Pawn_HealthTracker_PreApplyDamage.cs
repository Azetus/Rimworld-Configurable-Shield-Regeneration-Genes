using CSRG_ConfigurableShieldRegenerationGenes.Genes;
using HarmonyLib;
using Verse;

namespace CSRG_ConfigurableShieldRegenerationGenes.HarmonyPatches;

[HarmonyPatch(typeof(Pawn_HealthTracker), nameof(Pawn_HealthTracker.PreApplyDamage))]
public static class Patch_Pawn_HealthTracker_PreApplyDamage
{
    public static void Postfix(Pawn_HealthTracker __instance, DamageInfo dinfo, ref bool absorbed)
    {
        if (absorbed) return;
        var shield = __instance.pawn.genes?.GetFirstGeneOfType<CSRG_ShieldGene>();
        if (shield != null && shield.Active)
            shield.ProcessDamage(dinfo, ref absorbed);
    }
}
