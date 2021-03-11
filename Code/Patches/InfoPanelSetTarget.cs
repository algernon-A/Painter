using System.Reflection;
using HarmonyLib;


namespace Repaint
{

    /// <summary>
    /// Harmony patch for ShelterWorldInfoPanel.OnSetTarget to show the the color field and set it to the current building's settings.
    /// </summary>
    [HarmonyPatch(typeof(ShelterWorldInfoPanel), "OnSetTarget")]
    public static class ShelterWorldInfoPanelPatchTarget
    {
        /// <summary>
        /// Harmony Postfix patch to show color field and set to current building's settings.
        /// </summary>
        /// <param name="__instance">Panel instance</param>
        public static void Postfix(ShelterWorldInfoPanel __instance)
        {
            // Show panel.
            Repaint.instance.IsPanelVisible = true;

            // Set color to this building's setting.
            Repaint.instance.BuildingID = ((InstanceID)__instance.GetType().GetField("m_InstanceID", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance)).Building;
            Repaint.instance.ColorFields[PanelType.Shelter].selectedColor = Repaint.instance.GetColor();
        }
    }


    /// <summary>
    /// Harmony patch for CityServiceWorldInfoPanel.OnSetTarget to show the the color field and set it to the current building's settings.
    /// </summary>
    [HarmonyPatch(typeof(CityServiceWorldInfoPanel), "OnSetTarget")]
    public static class CityServiceWorldInfoPanelPatchTarget
    {
        /// <summary>
        /// Harmony Postfix patch to show color field and set to current building's settings.
        /// </summary>
        /// <param name="__instance">Panel instance</param>
        public static void Postfix(CityServiceWorldInfoPanel __instance)
        {
            // Show panel.
            Repaint.instance.IsPanelVisible = true;

            // Set color to this building's setting.
            Repaint.instance.BuildingID = ((InstanceID)__instance.GetType().GetField("m_InstanceID", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance)).Building;
            Repaint.instance.ColorFields[PanelType.Service].selectedColor = Repaint.instance.GetColor();
        }
    }


    /// <summary>
    /// Harmony patch for ZonedBuildingWorldInfoPanel.OnSetTarget to show the the color field and set it to the current building's settings.
    /// </summary>
    [HarmonyPatch(typeof(ZonedBuildingWorldInfoPanel), "OnSetTarget")]
    public static class ZonedBuildingWorldInfoPanelPatchTarget
    {
        /// <summary>
        /// Harmony Postfix patch to show color field and set to current building's settings.
        /// </summary>
        /// <param name="__instance">Panel instance</param>
        public static void Postfix(ZonedBuildingWorldInfoPanel __instance)
        {
            // Show panel.
            Repaint.instance.IsPanelVisible = true;

            // Set color to this building's setting.
            Repaint.instance.BuildingID = ((InstanceID)__instance.GetType().GetField("m_InstanceID", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance)).Building;
            Repaint.instance.ColorFields[PanelType.Zoned].selectedColor = Repaint.instance.GetColor();
        }
    }


    /// <summary>
    /// Harmony patch for WarehouseWorldInfoPanelPatchTarget.OnSetTarget to show the the color field and set it to the current building's settings.
    /// </summary>
    [HarmonyPatch(typeof(WarehouseWorldInfoPanel), "OnSetTarget")]
    public static class WarehouseWorldInfoPanelPatchTarget
    {
        /// <summary>
        /// Harmony Postfix patch to show color field and set to current building's settings.
        /// </summary>
        /// <param name="__instance">Panel instance</param>
        public static void Postfix(WarehouseWorldInfoPanel __instance)
        {
            // Show panel.
            Repaint.instance.IsPanelVisible = true;

            // Set color to this building's setting.
            Repaint.instance.BuildingID = ((InstanceID)__instance.GetType().GetField("m_InstanceID", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance)).Building;
            Repaint.instance.ColorFields[PanelType.Warehouse].selectedColor = Repaint.instance.GetColor();
        }
    }


    /// <summary>
    /// Harmony patch for FootballPanel.OnSetTarget to show the the color field and set it to the current building's settings.
    /// </summary>
    [HarmonyPatch(typeof(FootballPanel), "OnSetTarget")]
    public static class FootballPanelPatch
    {
        /// <summary>
        /// Harmony Postfix patch to show color field and set to current building's settings.
        /// </summary>
        /// <param name="__instance">Panel instance</param>
        public static void Postfix(FootballPanel __instance)
        {
            // Show panel.
            Repaint.instance.IsPanelVisible = true;

            // Set color to this building's setting.
            Repaint.instance.BuildingID = ((InstanceID)__instance.GetType().GetField("m_InstanceID", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance)).Building;
            Repaint.instance.ColorFields[PanelType.Stadium].selectedColor = Repaint.instance.GetColor();
        }
    }
}
