using CSRG_ConfigurableShieldRegenerationGenes.Settings;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace CSRG_ConfigurableShieldRegenerationGenes;

public class ConfigurableShieldRegenerationGenes : Mod
{
    public static CSRGModSettings settings;

    public ConfigurableShieldRegenerationGenes(ModContentPack content) : base(content)
    {
        settings = GetSettings<CSRGModSettings>();
        Log.Message("<color=cyan>[Configurable Shield & Regeneration Genes]</color> loaded!");
        try
        {
            new Harmony("Aliza.ConfigurableShieldRegenerationGenes").PatchAll();
        }
        catch (Exception ex)
        {
            Log.Error("<color=red>[Configurable Shield & Regeneration Genes]</color> Harmony patch failed: " + ex);
        }
    }

    public override string SettingsCategory()
    {
        return "CSRG_ModName".Translate();
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        base.DoSettingsWindowContents(inRect);
        CSRGModSettingsUI.SettingsWindowContents(inRect);
    }
}