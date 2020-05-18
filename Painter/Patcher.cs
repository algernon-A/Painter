using UnityEngine;
using HarmonyLib;
using CitiesHarmony.API;


namespace Painter
{
    /// <summary>
    /// Class to manage the mod's Harmony patches.
    /// </summary>
    public static class Patcher
    {
        // Unique harmony identifier.
        private const string harmonyID = "com.github.algernon-A.csl.repaint";

        // Flag.
        public static bool patched = false;


        /// <summary>
        /// Apply all Harmony patches.
        /// </summary>
        public static void PatchAll()
        {
            // Don't do anything if already patched.
            if (!patched)
            {
                // Ensure Harmony is ready before patching.
                if (HarmonyHelper.IsHarmonyInstalled)
                {
                    // Create harmony instance.
                    Debug.Log("Repaint v" + RepaintMod.Version + ": deploying Harmony patches.");

                    // Apply all annotated patches and update flag.
                    Harmony harmonyInstance = new Harmony(harmonyID);
                    harmonyInstance.PatchAll();
                    patched = true;
                }
                else
                {
                    Debug.Log("Repaint: Harmony not ready.");
                }
            }
        }


        public static void UnpatchAll()
        {
            // Only unapply if patches appplied.
            if (patched)
            {
                Debug.Log("Repaint: reverting Harmony patches.");

                // Unapply patches, but only with our HarmonyID.
                Harmony harmonyInstance = new Harmony(harmonyID);
                harmonyInstance.UnpatchAll(harmonyID);
                patched = false;
            }
        }
    }
}