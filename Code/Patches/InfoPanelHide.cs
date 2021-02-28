using HarmonyLib;


namespace Repaint
{
    /// <summary>
    /// Harmony patch for WorldInfoPanel.OnHide to toggle visibility of Repaint color panel.
    /// </summary>
    [HarmonyPatch(typeof(WorldInfoPanel), "OnHide")]
    public static class WorldInfoPanelPatch
    {
        /// <summary>
        /// Harmony Postfix patch to hide color panel when info panel is no longer visible.
        /// </summary>
        public static void Postfix(WorldInfoPanel __instance)
        {
            if (__instance is ZonedBuildingWorldInfoPanel || __instance is CityServiceWorldInfoPanel || __instance is ShelterWorldInfoPanel)
            {
                Repaint.instance.IsPanelVisible = false;
            }
        }
    }
}
