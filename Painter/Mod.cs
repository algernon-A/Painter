using ICities;
using CitiesHarmony.API;
using Painter.TranslationFramework;


namespace Painter
{
    public class RepaintMod : IUserMod
    {
        public string Name => "Repaint";
        public static string Version => "1.0";
        public string Description => Translation.GetTranslation("PAINTER-DESCRIPTION");
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