using ICities;


namespace Repaint
{
    /// <summary>
    /// Main loading class: the mod runs from here.
    /// </summary>
    public class Loading : LoadingExtensionBase
    {
        public override void OnLevelLoaded(LoadMode mode)
        {
            base.OnLevelLoaded(mode);

            // Don't do anything if not in game.
            if (mode != LoadMode.LoadGame && mode != LoadMode.NewGame)
            {
                return;
            }

            // Wait for loading to fully complete.
            while (!LoadingManager.instance.m_loadingComplete) { }

            // Add color fields to building info panels.
            Repaint.instance.AddColorFieldsToPanels();

            // Queue action for simulation manager to update buildings with loaded colours.
            SimulationManager.instance.AddAction(() =>
            {
                // Iterate through each building.
                for (ushort i = 0; i < BuildingManager.instance.m_buildings.m_buffer.Length; i++)
                {
                    // Check for existant buildings.
                    if (BuildingManager.instance.m_buildings.m_buffer[i].m_flags != Building.Flags.None)
                    {
                        // Update colours.
                        BuildingManager.instance.UpdateBuildingColors(i);
                    }
                }
            });
        }
    }
}
