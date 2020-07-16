using UnityEngine;
using HarmonyLib;
using Painter;


namespace Repaint
{
    /// <summary>
    /// Harmony patch for CommonBuildingAI.GetColor to apply custom colour settings to buildings.
    /// </summary>
    [HarmonyPatch(typeof(CommonBuildingAI), "GetColor")]
    class CommonBuildingAIPatch
    {
        /// <summary>
        /// Harmony Prefix patch to apply custom colours to building.
        /// </summary>
        /// <param name="__result">Original method result</param>
        /// <param name="buildingID">Building instance ID</param>
        /// <param name="infoMode">InfoManager info mode</param>
        /// <returns>False (stop execution chain) if a custom colour is applied, true (continue on to original method) otherwise</returns>
        static bool Prefix(ref Color __result, ushort buildingID, InfoManager.InfoMode infoMode)
        {
            // Don't do anything if we're in a special info mode.
            if (infoMode == InfoManager.InfoMode.None)
            {
                // See if this building is in our custom colour list.
                if (Repaint.instance.Colors.TryGetValue(buildingID, out SerializableColor color))
                {
                    // It is; return our custom colour and abort execution chain (don't execute original method).
                    __result = color;
                    return false;
                }
            }

            // No custom colour application; continue on to original method.
            return true;
        }
    }
}