using System;
using ColossalFramework;
using ColossalFramework.UI;
using System.Collections.Generic;
using UnityEngine;
using Painter;


namespace Repaint
{
    /// <summary>
    /// Reference indexes to the three types of info panels (service, shelter, zoned).
    /// </summary>
    public enum PanelType
    {
        None = -1,
        Service,
        Shelter,
        Zoned,
        Count
    }


    /// <summary>
    /// Main mod class - implements UI and applies colour settings.
    /// </summary>
    public class Repaint : Singleton<Repaint>
    {
        // UI Components - base functionality.
        private Dictionary<PanelType, BuildingWorldInfoPanel> Panels;
        internal Dictionary<PanelType, UIColorField> ColorFields;
        private UIColorField colorFieldTemplate;
        private UIButton copyButton;
        private UIButton resetButton;
        private UIButton pasteButton;

        // UI Components - colorizer.
        private PainterColorizer colorizer;
        private UICheckBox colorizeCheckbox;
        private UICheckBox invertCheckbox;

        // Variables and flags.
        private Color32 copyPasteColor;
        internal ushort BuildingID;
        internal bool IsPanelVisible;
        internal bool isPickerOpen;


        /// <summary>
        /// Dictionary of building colour settings.
        /// Key mod data record.
        /// </summary>
        internal Dictionary<ushort, SerializableColor> Colors
        {
            get
            {
                if (_colors == null)
                {
                    _colors = new Dictionary<ushort, SerializableColor>();
                }
                return _colors;
            }
            set
            {
                _colors = value;
            }
        }
        private Dictionary<ushort, SerializableColor> _colors;


        /// <summary>
        /// Loads and saves colorizer file settings.
        /// </summary>
        public PainterColorizer Colorizer
        {
            get
            {
                // Check to see that we haven't already done this.
                if (colorizer == null)
                {
                    // Attempt to load colorizer data from file.
                    colorizer = PainterColorizer.Load();

                    // If we failed, create a new colorizer and save it.
                    if (colorizer == null)
                    {
                        colorizer = new PainterColorizer();
                        colorizer.Save();
                    }
                }

                return colorizer;
            }
            set
            {
                colorizer = value;
            }
        }


        /// <summary>
        /// Colorizes or inverts a building.
        /// Part of the colorization function.
        /// </summary>
        /// <param name="building">Building prefab to colorize/invert</param>
        /// <param name="invert">True to invert, false otherwise</param>
        internal void Colorize(BuildingInfo building, bool invert)
        {
            try
            {
                // Apply ACI changes to main mesh and LOD.
                building.GetComponent<Renderer>().material.UpdateACI(invert);
                building.m_lodObject.GetComponent<Renderer>().material.UpdateACI(invert);

                // Iterate through submeshes and apply settings as well.
                BuildingInfo.MeshInfo[] subMeshes = building.m_subMeshes;
                foreach (BuildingInfo.MeshInfo meshInfo in subMeshes)
                {
                    try
                    {
                        // Apply ACI changes to submesh and LOD.
                        meshInfo.m_subInfo.GetComponent<Renderer>().material.UpdateACI(invert);
                        meshInfo.m_subInfo.m_lodObject.GetComponent<Renderer>().material.UpdateACI(invert);
                    }
                    catch (Exception message)
                    {
                        // Don't let a single failure stop the whole process.
                        Debug.LogWarning(message);
                    }
                }
            }
            catch (Exception message2)
            {
                // Don't let a single failure kill the mod.
                Debug.LogWarning(message2);
            }
        }


        /// <summary>
        /// Gets the current building's colour setting.
        /// </summary>
        /// <returns>Current building's colour</returns>
        internal Color GetColor()
        {
            // Get current building instance.
            Building building = BuildingManager.instance.m_buildings.m_buffer[BuildingID];

            // Lookup and return the set colour.
            return building.Info.m_buildingAI.GetColor(BuildingID, ref building, InfoManager.InfoMode.None);
        }


        /// <summary>
        /// Add color fields to building info panels.
        /// </summary>
        internal void AddColorFieldsToPanels()
        {
            // Create new panel dictionary and populate with relevant game info panel references.
            Panels = new Dictionary<PanelType, BuildingWorldInfoPanel>
            {
                [PanelType.Service] = UIView.library.Get<CityServiceWorldInfoPanel>(typeof(CityServiceWorldInfoPanel).Name),
                [PanelType.Shelter] = UIView.library.Get<ShelterWorldInfoPanel>(typeof(ShelterWorldInfoPanel).Name),
                [PanelType.Zoned] = UIView.library.Get<ZonedBuildingWorldInfoPanel>(typeof(ZonedBuildingWorldInfoPanel).Name)
            };

            // Create new color field dictionary and populate with new color fields.
            ColorFields = new Dictionary<PanelType, UIColorField>
            {
                [PanelType.Service] = CreateColorField(Panels[PanelType.Service]?.component),
                [PanelType.Shelter] = CreateColorField(Panels[PanelType.Shelter]?.component),
                [PanelType.Zoned] = CreateColorField(Panels[PanelType.Zoned]?.component),
            };
        }


        /// <summary>
        /// Unity update method to check and handle keystroke inputs for the color picker when it's open.
        /// </summary>
        private void Update()
        {
            // Only interested when the picker is open.
            if (isPickerOpen)
            {
                // Check for appropriate modifier keys.
                if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand))
                {
                    // Copy key.
                    if (Input.GetKeyDown(KeyCode.C))
                    {
                        // Copy the current colour selection.
                        copyPasteColor = GetColor();
                    }
                    // Paste key.
                    else if (Input.GetKeyDown(KeyCode.V))
                    {
                        // Paste the copied colour.
                        PasteColor();
                    }
                    // Erase key.
                    if (Input.GetKeyDown(KeyCode.Delete) || Input.GetKeyDown(KeyCode.Backspace))
                    {
                        // Erase custom colour setting.
                        EraseColor();
                    }
                }
            }
        }


        /// <summary>
        /// Update a building's colour setting/
        /// </summary>
        /// <param name="color">Colour to apply</param>
        /// <param name="currentBuilding">Building to apply to</param>
        private void UpdateColor(Color32 color, ushort currentBuilding)
        {
            // See if we already have a setting for this building.
            if (Colors.ContainsKey(currentBuilding))
            {
                // Yes - update current setting.
                Colors[currentBuilding] = color;
            }
            else
            {
                // No existing setting - add a new one.
                Colors.Add(currentBuilding, color);
            }

            // Apply colour change in-game via the building manager.
            BuildingManager.instance.UpdateBuildingColors(BuildingID);
        }


        /// <summary>
        /// Removes the current building's custom colour setting.
        /// </summary>
        private void ResetColor()
        {
            // Check that we already have a setting to remove.
            if (Colors.ContainsKey(BuildingID))
            {
                // Remove the setting.
                Colors.Remove(BuildingID);

                // Apply colour change in-game via the building manager.
                BuildingManager.instance.UpdateBuildingColors(BuildingID);
            }
        }


        /// <summary>
        /// Erases the custom colour selection for a building, as directed by the user.
        /// </summary>
        private void EraseColor()
        {
            // Get the current color field.
            UIColorField field = Panels[PanelType.Service].component.isVisible ? ColorFields[PanelType.Service] : Panels[PanelType.Shelter].component.isVisible ? ColorFields[PanelType.Shelter] : ColorFields[PanelType.Zoned];
            
            // Remove custom colour from building.
            ResetColor();

            // Reset the color field to the building's "natural" colour.
            field.selectedColor = GetColor();

            // Close and re-open the popup (to refresh).
            field.SendMessage("ClosePopup", false);
            field.SendMessage("OpenPopup");
        }


        /// <summary>
        /// Pastes a custom colour selection to the selected building, as directed by the user.
        /// </summary>
        private void PasteColor()
        {
            // Get the current color field.
            UIColorField field = Panels[PanelType.Service].component.isVisible ? ColorFields[PanelType.Service] : Panels[PanelType.Shelter].component.isVisible ? ColorFields[PanelType.Shelter] : ColorFields[PanelType.Zoned];

            // Update building's colour to the copied value.
            UpdateColor(copyPasteColor, BuildingID);

            // Reset the color field to the building's "natural" colour.
            field.selectedColor = copyPasteColor;

            // Close and re-open the popup (to refresh).
            field.SendMessage("ClosePopup", false);
            field.SendMessage("OpenPopup");
        }


        /// <summary>
        /// Create a new color field for a building info panel.
        /// </summary>
        /// <param name="parent">Parent component</param>
        /// <returns>New color field</returns>
        private UIColorField CreateColorField(UIComponent parent)
        {
            // Component to position the field against.
            UIComponent problemsPanel;


            // Create the color field template if we haven't already.
            if (colorFieldTemplate == null)
            {
                // Get line template, and return null if we fail.
                UIComponent template = UITemplateManager.Get("LineTemplate");
                if (template == null)
                {
                    return null;
                }

                // Get colour template, and return null if we fail.
                colorFieldTemplate = template.Find<UIColorField>("LineColor");
                if (colorFieldTemplate == null)
                {
                    return null;
                }
            }

            // Intantiate our template as a new color field and attach to our parent.
            UIColorField colorField = Instantiate(colorFieldTemplate.gameObject).GetComponent<UIColorField>();
            parent.AttachUIComponent(colorField.gameObject);

            // Find ProblemsPanel relative position to position ColorField correctly.
            // We'll use 43f as a default relative Y in case something doesn't work.
            float relativeY = 43f;

            // Player info panels have wrappers, zoned ones don't.
            UIComponent wrapper = parent.Find("Wrapper");
            if (wrapper == null)
            {
                problemsPanel = parent.Find("ProblemsPanel");
            }
            else
            {
                problemsPanel = wrapper.Find("ProblemsPanel");
            }

            try
            {
                // Position ColorField vertically in the middle of the problems panel.  If wrapper panel exists, we need to add its offset as well.
                relativeY = (wrapper == null ? 0 : wrapper.relativePosition.y) + problemsPanel.relativePosition.y + ((problemsPanel.height - 26) / 2);
            }
            catch
            {
                // Don't care; just use default relative Y.
                Debug.Log("Repaint: couldn't find ProblemsPanel relative position.");
            }

            // Set up the new color field.
            colorField.name = "RepaintColorField";
            colorField.AlignTo(parent, UIAlignAnchor.TopRight);
            colorField.relativePosition += new Vector3(-40f, relativeY, 0f);
            colorField.size = new Vector2(26f, 26f);
            colorField.pickerPosition = UIColorField.ColorPickerPosition.RightBelow;

            // Event handlers.
            colorField.eventSelectedColorChanged += EventSelectedColorChangedHandler;
            colorField.eventColorPickerOpen += EventColorPickerOpenHandler;
            colorField.eventColorPickerClose += EventColorPickerCloseHandler;

            return colorField;
        }


        /// <summary>
        /// Event handler - called when a new colour is selected.
        /// </summary>
        /// <param name="component">Calling component</param>
        /// <param name="value">New colour</param>
        private void EventSelectedColorChangedHandler(UIComponent component, Color value)
        {
            UpdateColor(value, BuildingID);
        }


        /// <summary>
        /// Event handler - opens the color picker.
        /// </summary>
        /// <param name="colorField">Calling color field</param>
        /// <param name="colorPicker">Color picker to open</param>
        /// <param name="overridden"></param>
        private void EventColorPickerOpenHandler(UIColorField colorField, UIColorPicker colorPicker, ref bool overridden)
        {
            // Increase the colour picker component height to accomodate controls below.
            colorPicker.component.height += 60f;

            // Create copy, paste, and reset buttons.
            copyButton = CreateButton(colorPicker.component, Translations.Translate("PAINTER-COPY"), 10f);
            pasteButton = CreateButton(colorPicker.component, Translations.Translate("PAINTER-PASTE"), 91.33333333333333f);
            resetButton = CreateButton(colorPicker.component, Translations.Translate("PAINTER-RESET"), 172.6666666666667f);

            // Create colorize and invert checkboxes.
            string prefabName = Singleton<BuildingManager>.instance.m_buildings.m_buffer[BuildingID].Info.name;
            colorizeCheckbox = CreateCheckBox(colorPicker.component, Translations.Translate("PAINTER-COLORIZE"), 10f, Colorizer.Colorized.Contains(prefabName));
            invertCheckbox = CreateCheckBox(colorPicker.component, Translations.Translate("PAINTER-INVERT"), 127f, Colorizer.Inverted.Contains(prefabName));

            // Set visibility flag.
            isPickerOpen = true;

            // Event handlers - copy, paste, reset.
            copyButton.eventClick += (comppnent, clickEvent) =>
            {
                copyPasteColor = GetColor();
            };
            pasteButton.eventClick += (comppnent, clickEvent) =>
            {
                PasteColor();
            };
            resetButton.eventClick += (comppnent, clickEvent) =>
            {
                EraseColor();
            };

            // Event handler - colorize.
            colorizeCheckbox.eventCheckChanged += (component, isChecked) =>
            {
                // Update list of colorized buildings accordingly.
                HandleCheckboxes(Colorizer.Colorized, isChecked);

                // If we're checked, deselect the invert checkbox.
                if (isChecked)
                {
                    invertCheckbox.isChecked = !isChecked;
                }

                // Save settings.
                Colorizer.Save();
            };

            // Event handler - invert.
            invertCheckbox.eventCheckChanged += (component, isChecked) =>
            {
                // Update list of inverted buildings accordingly.
                HandleCheckboxes(Colorizer.Inverted, isChecked);

                // If we're checked, deselect the colorized checkbox.
                if (isChecked)
                {
                    colorizeCheckbox.isChecked = !isChecked;
                }

                // Save settings.
                Colorizer.Save();
            };
        }


        /// <summary>
        /// Event handler - called when color picker is closed.
        /// </summary>
        /// <param name="colorField">Ignored</param>
        /// <param name="colorPicker">Ignored</param>
        /// <param name="overridden">Ignored</param>
        private void EventColorPickerCloseHandler(UIColorField colorField, UIColorPicker colorPicker, ref bool overridden)
        {
            // Set visibility flag.
            isPickerOpen = false;
        }


        /// <summary>
        /// Handles colorized/inverted checkbox state changes, adding/removing entries from lists as appropriate.
        /// </summary>
        /// <param name="list">List of prefabs</param>
        /// <param name="isChecked">Checkbox state</param>
        private void HandleCheckboxes(List<string> list, bool isChecked)
        {
            // Get name of current building.
            string buildingName = Singleton<BuildingManager>.instance.m_buildings.m_buffer[BuildingID].Info.name;

            // First, see if the given list contains the name of the current building.
            if (list.Contains(buildingName))
            {
                // In list - if the checkbox is not checked, remove the entry.
                if (!isChecked)
                {
                    list.RemoveAll((string building) => building == buildingName);
                }
            }
            else
            {
                // Not in list - if the checkbox is checked, add the entry.
                if (isChecked)
                {
                    list.Add(buildingName);
                }
            }
        }


        /// <summary>
        /// Creates a pushbutton.
        /// </summary>
        /// <param name="parent">Parent component</param>
        /// <param name="text">Button text label</param>
        /// <param name="xPos">Button relative X position</param>
        /// <returns>New pushbutton</returns>
        private UIButton CreateButton(UIComponent parent, string text, float xPos)
        {
            // Basic setup.
            UIButton button = parent.AddUIComponent<UIButton>();
            button.width = 71.33333333333333f;
            button.height = 20f;
            button.relativePosition = new Vector3(xPos, 233f);

            // Button text.
            button.textScale = 0.8f;
            button.textPadding = new RectOffset(0, 0, 5, 0);
            button.horizontalAlignment = UIHorizontalAlignment.Center;
            button.textVerticalAlignment = UIVerticalAlignment.Middle;
            button.text = text;

            // Button sprites.
            button.atlas = UIView.GetAView().defaultAtlas;
            button.normalBgSprite = "ButtonMenu";
            button.disabledBgSprite = "ButtonMenuDisabled";
            button.hoveredBgSprite = "ButtonMenuHovered";
            button.focusedBgSprite = "ButtonMenu";
            button.pressedBgSprite = "ButtonMenuPressed";

            return button;
        }


        /// <summary>
        /// Creates a checkbox.
        /// </summary>
        /// <param name="parent">Parent component</param>
        /// <param name="text">Button text label</param>
        /// <param name="xPos">Button relative X position</param>
        /// <param name="isChecked">Whether the button is initially checked or not</param>
        /// <returns>New checkbox</returns>
        private UICheckBox CreateCheckBox(UIComponent parent, string text, float xPos, bool isChecked)
        {
            // Basic setup.
            UICheckBox checkBox = parent.AddUIComponent<UICheckBox>();
            checkBox.width = 20f;
            checkBox.height = 20f;
            checkBox.relativePosition = new Vector3(xPos, 260f);

            // Checkbox text.
            UILabel label = checkBox.AddUIComponent<UILabel>();
            label.text = text;
            label.textScale = 0.8f;
            label.relativePosition = new Vector3(22f, 5f);

            // Sprites.
            UISprite sprite = checkBox.AddUIComponent<UISprite>();
            sprite.spriteName = "ToggleBase";
            sprite.size = new Vector2(16f, 16f);
            sprite.relativePosition = new Vector3(2f, 2f);

            checkBox.checkedBoxObject = sprite.AddUIComponent<UISprite>();
            ((UISprite)checkBox.checkedBoxObject).spriteName = "ToggleBaseFocused";
            checkBox.checkedBoxObject.size = new Vector2(16f, 16f);
            checkBox.checkedBoxObject.relativePosition = Vector3.zero;

            // Initial state.
            checkBox.isChecked = isChecked;

            // Tooltip.
            checkBox.tooltip = Translations.Translate("PAINTER-RELOAD-REQUIRED");

            return checkBox;
        }
    }
}
