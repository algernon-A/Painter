using ColossalFramework;
using HarmonyLib;

using UnityEngine;


namespace Repaint
{
	/// <summary>
	/// Harmony Prefix patch to BuildingInfo.InitializePrefab to apply building colorization on load.
	/// </summary>
	[HarmonyPatch(typeof(BuildingInfo), "InitializePrefab")]
	public static class ColorizePatch
	{
		/// <summary>
		/// Harmony Prefix patch to apply building colorization to prefabs as they are initialised.
		/// </summary>
		/// <param name="__instance">Instance reference</param>
		/// <returns>Always true (doesn't pre-empt original method)</returns>
		public static bool Prefix(BuildingInfo __instance)
		{
			// See if this building is in our lists.
			if (Singleton<Repaint>.instance.Colorizer.Colorized.Contains(__instance.name))
			{
				// Colorize building.
				Singleton<Repaint>.instance.Colorize(__instance, invert: false);

				// Colorize sub-buildings.
				if (__instance.m_subBuildings != null && __instance.m_subBuildings.Length > 0)
				{
					foreach (BuildingInfo.SubInfo subInfo in __instance.m_subBuildings)
					{
						Singleton<Repaint>.instance.Colorize(subInfo.m_buildingInfo, invert: false);
					}
				}
			}
			else if (Singleton<Repaint>.instance.Colorizer.Inverted.Contains(__instance.name))
			{
				// Invert building.
				Singleton<Repaint>.instance.Colorize(__instance, invert: true);

				// Invert sub-buildings.
				if (__instance.m_subBuildings != null && __instance.m_subBuildings.Length > 0)
				{
					foreach (BuildingInfo.SubInfo subInfo in __instance.m_subBuildings)
					{
						Singleton<Repaint>.instance.Colorize(subInfo.m_buildingInfo, invert: true);
					}
				}
			}

			// Always continue on to original method.
			return true;
		}
    }
}