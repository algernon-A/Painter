using ICities;
using CitiesHarmony.API;


namespace Repaint
{
    public class RepaintMod : IUserMod
    {
        public static string ModName => "Repaint ";
        public static string Version => "1.2.2";
        public string Name => ModName + " " + Version;
        public string Description => Translations.Translate("PAINTER-DESCRIPTION");
        public static Translation Translation = new Translation();

        public void OnEnabled()
        {
            // Apply Harmony patches via Cities Harmony.
            // Called here instead of OnCreated to allow the auto-downloader to do its work prior to launch.
            HarmonyHelper.DoOnHarmonyReady(() => Patcher.PatchAll());
        }

        public void OnDisabled()
        {
            // Unapply Harmony patches via Cities Harmony.
            if (HarmonyHelper.IsHarmonyInstalled)
            {
                Patcher.UnpatchAll();
            }
        }
    }
}