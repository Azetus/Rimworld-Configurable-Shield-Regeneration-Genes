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
        new Harmony("Aliza.ConfigurableShieldRegenerationGenes").PatchAll();
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        base.DoSettingsWindowContents(inRect);
        CSRGModSettingsUI.SettingsWindowContents(inRect);
    }
}