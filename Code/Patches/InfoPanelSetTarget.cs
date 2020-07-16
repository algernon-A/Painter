using System.Reflection;
using HarmonyLib;


namespace Repaint
{

    /// <summary>
    /// Harmony patch for ShelterWorldInfoPanel.OnSetTarget to show the the color field and set it to the current building's settings.
    /// </summary>
    [HarmonyPatch(typeof(ShelterWorldInfoPanel), "OnSetTarget")]
    class ShelterWorldInfoPanelPatchTarget
    {
        /// <summary>
        /// Harmony Postfix patch to show color field and set to current building's settings.
        /// </summary>
        /// <param name="__instance">Panel instance</param>
        static void Postfix(ShelterWorldInfoPanel __instance)
        {
            // Show panel.
            Repaint.instance.IsPanelVisible = true;

            // Set color to this building's setting.
            Repaint.instance.BuildingID = ((InstanceID)__instance.GetType().GetField("m_InstanceID", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance)).Building;
            Repaint.instance.ColorFields[PanelType.Shelter].selectedColor = Repaint.instance.GetColor();
        }
    }


    /// <summary>
    /// Harmony patch for ShelterWorldInfoPanel.OnSetTarget to show the the color field and set it to the current building's settings.
    /// </summary>
    [HarmonyPatch(typeof(CityServiceWorldInfoPanel), "OnSetTarget")]
    class CityServiceWorldInfoPanelPatchTarget
    {
        /// <summary>
        /// Harmony Postfix patch to show color field and set to current building's settings.
        /// </summary>
        /// <param name="__instance">Panel instance</param>
        static void Postfix(CityServiceWorldInfoPanel __instance)
        {
            // Show panel.
            Repaint.instance.IsPanelVisible = true;

            // Set color to this building's setting.
            Repaint.instance.BuildingID = ((InstanceID)__instance.GetType().GetField("m_InstanceID", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance)).Building;
            Repaint.instance.ColorFields[PanelType.Service].selectedColor = Repaint.instance.GetColor();
        }
    }


    /// <summary>
    /// Harmony patch for ShelterWorldInfoPanel.OnSetTarget to show the the color field and set it to the current building's settings.
    /// </summary>
    [HarmonyPatch(typeof(ZonedBuildingWorldInfoPanel), "OnSetTarget")]
    class ZonedBuildingWorldInfoPanelPatchTarget
    {
        /// <summary>
        /// Harmony Postfix patch to show color field and set to current building's settings.
        /// </summary>
        /// <param name="__instance">Panel instance</param>
        static void Postfix(ZonedBuildingWorldInfoPanel __instance)
        {
            // Show panel.
            Repaint.instance.IsPanelVisible = true;

            // Set color to this building's setting.
            Repaint.instance.BuildingID = ((InstanceID)__instance.GetType().GetField("m_InstanceID", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance)).Building;
            Repaint.instance.ColorFields[PanelType.Zoned].selectedColor = Repaint.instance.GetColor();
        }
    }
}
