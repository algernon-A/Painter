using ColossalFramework;
using HarmonyLib;


namespace Repaint
{
	/// <summary>
	/// Harmony Prefix patch to BuildingInfo.InitializePrefab to apply building colorization on load.
	/// </summary>
	[HarmonyPatch(typeof(BuildingInfo), "InitializePrefab")]
	internal static class ColorizePatch
	{
		/// <summary>
		/// Harmony Prefix patch to apply building colorization to prefabs as they are initialised.
		/// </summary>
		/// <param name="__instance">Instance reference</param>
		/// <returns>Always true (doesn't pre-empt original method)</returns>
		private static bool Prefix(BuildingInfo __instance)
		{
			// See if this building is in our lists.
			if (Singleton<Repaint>.instance.Colorizer.Colorized.Contains(__instance.name))
			{
				// Colorize building.
				Singleton<Repaint>.instance.Colorize(__instance, invert: false);
			}
			else if (Singleton<Repaint>.instance.Colorizer.Inverted.Contains(__instance.name))
			{
				// Invert building.
				Singleton<Repaint>.instance.Colorize(__instance, invert: true);
			}

			// Always continue on to original method.
			return true;
		}
    }
}