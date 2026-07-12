using CSRG_ConfigurableShieldRegenerationGenes.Genes;
using UnityEngine;
using Verse;

namespace CSRG_ConfigurableShieldRegenerationGenes.Gizmos;

[StaticConstructorOnStartup]
public class Gizmo_GeneShieldStatus : Gizmo
{
    public CSRG_ShieldGene gene;

    private static readonly Texture2D FullShieldBarTex =
        SolidColorMaterials.NewSolidColorTexture(new Color(0.2f, 0.2f, 0.24f));

    private static readonly Texture2D EmptyShieldBarTex =
        SolidColorMaterials.NewSolidColorTexture(Color.clear);

    public Gizmo_GeneShieldStatus(CSRG_ShieldGene gene)
    {
        this.gene = gene;
        Order = -100f;
    }

    public override float GetWidth(float maxWidth)
    {
        return 140f;
    }

    public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
    {
        if (gene == null) return new GizmoResult(GizmoState.Clear);
        Rect rect = new Rect(topLeft.x, topLeft.y, GetWidth(maxWidth), 75f);
        Rect rect2 = rect.ContractedBy(6f);
        Widgets.DrawWindowBackground(rect);

        Rect rect3 = rect2;
        rect3.height = rect.height / 2f;
        Text.Font = GameFont.Tiny;
        Widgets.Label(rect3, gene.LabelCap);

        Rect rect4 = rect2;
        rect4.yMin = rect2.y + rect2.height / 2f;
        float fillPercent = gene.Energy / Mathf.Max(1f, gene.MaxEnergy);
        Widgets.FillableBar(rect4, fillPercent, FullShieldBarTex, EmptyShieldBarTex, doBorder: false);

        Text.Font = GameFont.Small;
        Text.Anchor = TextAnchor.MiddleCenter;
        Widgets.Label(rect4,
            gene.Energy.ToString("F0") + " / " + gene.MaxEnergy.ToString("F0"));
        Text.Anchor = TextAnchor.UpperLeft;

        TooltipHandler.TipRegion(rect2, "CSRG.ShieldGeneTip".Translate());
        return new GizmoResult(GizmoState.Clear);
    }
}
