using UnityEngine;
using ColossalFramework.UI;


namespace Repaint
{
    /// <summary>
    /// UI textfield with an attached label.
    /// </summary>
    public class UILabelledTextfield
    {
        public UITextField textField;
        public UILabel label;
    }


    /// <summary>
    /// Static utilities class for creating UI controls.
    /// </summary>
    public static class UIControls
    {
        /// <summary>
        /// Adds a simple pushbutton.
        /// </summary>
        /// <param name="parent">Parent component</param>
        /// <param name="posX">Relative X postion</param>
        /// <param name="posY">Relative Y position</param>
        /// <param name="text">Button text</param>
        /// <param name="width">Button width (default 200)</param>
        /// <param name="height">Button height (default 30)</param>
        /// <param name="scale">Text scale (default 0.9)</param>
        /// <param name="vertPad">Vertical text padding within button (default 4)</param>
        /// <param name="tooltip">Tooltip, if any</param>
        /// <returns>New pushbutton</returns>
        public static UIButton AddButton(UIComponent parent, float posX, float posY, string text, float width = 200f, float height = 30f, float scale = 0.9f, int vertPad = 4, string tooltip = null)
        {
            UIButton button = parent.AddUIComponent<UIButton>();

            // Size and position.
            button.size = new Vector2(width, height);
            button.relativePosition = new Vector2(posX, posY);

            // Appearance.
            button.textScale = scale;
            button.normalBgSprite = "ButtonMenu";
            button.hoveredBgSprite = "ButtonMenuHovered";
            button.pressedBgSprite = "ButtonMenuPressed";
            button.disabledBgSprite = "ButtonMenuDisabled";
            button.disabledTextColor = new Color32(128, 128, 128, 255);
            button.canFocus = false;

            // Add tooltip.
            if (tooltip != null)
            {
                button.tooltip = tooltip;
            }

            // Text.
            button.textScale = scale;
            button.textPadding = new RectOffset(0, 0, vertPad, 0);
            button.textVerticalAlignment = UIVerticalAlignment.Middle;
            button.textHorizontalAlignment = UIHorizontalAlignment.Center;
            button.text = text;

            return button;
        }


        /// <summary>
        /// Adds a checkbox with a descriptive text label immediately to the right.
        /// </summary>
        /// <param name="parent">Parent component</param>
        /// <param name="text">Descriptive label text</param>
        /// <param name="xPos">Relative x position</param>
        /// <param name="yPos">Relative y position</param>
        /// <param name="textScale">Text scale of label (default 0.8)</param>
        /// <param name="tooltip">Tooltip, if any</param>
        /// <returns>New UI checkbox with attached labels</returns>
        public static UICheckBox AddCheckBox(UIComponent parent, float xPos, float yPos, string text, float textScale = 0.8f, string tooltip = null)
        {
            // Create base checkbox.
            UICheckBox checkBox = AddCheckBox(parent, xPos, yPos, tooltip);

            // Label.
            checkBox.label = checkBox.AddUIComponent<UILabel>();
            checkBox.label.relativePosition = new Vector3(21f, checkBox.height / 2f);
            checkBox.label.anchor = UIAnchorStyle.Left | UIAnchorStyle.CenterVertical;
            checkBox.label.textScale = textScale;
            checkBox.label.autoSize = true;
            checkBox.label.text = text;

            // Dynamic width to accomodate label.
            checkBox.width = checkBox.label.width + 21f;

            return checkBox;
        }


        /// <summary>
        /// Adds a checkbox without a label.
        /// </summary>
        /// <param name="parent">Parent component</param>
        /// <param name="xPos">Relative x position</param>
        /// <param name="yPos">Relative y position</param>
        /// <param name="tooltip">Tooltip, if any</param>
        /// <returns>New UI checkbox *without* attached labels</returns>
        public static UICheckBox AddCheckBox(UIComponent parent, float xPos, float yPos, string tooltip = null)
        {
            UICheckBox checkBox = parent.AddUIComponent<UICheckBox>();

            // Size and position.
            checkBox.height = 16f;
            checkBox.width = 16f;
            checkBox.clipChildren = false;
            checkBox.relativePosition = new Vector3(xPos, yPos);

            // Sprites.
            UISprite sprite = checkBox.AddUIComponent<UISprite>();
            sprite.spriteName = "check-unchecked";
            sprite.size = new Vector2(16f, 16f);
            sprite.relativePosition = Vector3.zero;

            checkBox.checkedBoxObject = sprite.AddUIComponent<UISprite>();
            ((UISprite)checkBox.checkedBoxObject).spriteName = "check-checked";
            checkBox.checkedBoxObject.size = new Vector2(16f, 16f);
            checkBox.checkedBoxObject.relativePosition = Vector3.zero;

            // Add tooltip.
            if (tooltip != null)
            {
                checkBox.tooltip = tooltip;
            }

            return checkBox;
        }


        /// <summary>
        /// Adds a textfield with an attached label to the left.
        /// </summary>
        /// <param name="parent">Parent component</param>
        /// <param name="posX">Relative X postion</param>
        /// <param name="posY">Relative Y position</param>
        /// <param name="text">Label text</param>
        /// <param name="width">Textfield width (default 200)</param>
        /// <param name="height">Textfield height (default 22)</param>
        /// <param name="scale">Text scale (default 1.0)</param>
        /// <param name="vertPad">Vertical text padding within textfield box (default 4)</param>
        /// <param name="tooltip">Tooltip, if any</param>
        /// <returns>New textfield with attached label</returns>
        public static UITextField LabelledTextField(UIComponent parent, float posX, float posY, string text, float width = 200f, float height = 22f, float scale = 1.0f, int vertPad = 4, string tooltip = null)
        {
            UITextField textField = AddTextField(parent, posX, posY, width, height, scale, vertPad, tooltip);

            // Label.
            UILabel label = textField.AddUIComponent<UILabel>();
            label.textScale = scale;
            label.text = text;
            label.autoSize = true;
            label.verticalAlignment = UIVerticalAlignment.Middle;
            label.wordWrap = true;

            // Set position.
            label.relativePosition = new Vector2(-(label.width + 5f), (height - label.height) / 2);

            return textField;
        }


        /// <summary>
        /// Adds an input text field at the specified coordinates.
        /// </summary>
        /// <param name="textField">Textfield object</param>
        /// <param name="posX">Relative X postion</param>
        /// <param name="posY">Relative Y position</param>
        /// <param name="parent">component to add to</param>
        /// <param name="height">Textfield height (default 22)</param>
        /// <param name="scale">Text scale (default 1.0)</param>
        /// <param name="vertPad">Vertical text padding within textfield box (default 4)</param>
        /// <param name="tooltip">Tooltip, if any</param>
        /// <returns>New textfield *without* attached label</returns>
        public static UITextField AddTextField(UIComponent parent, float posX, float posY, float width = 200f, float height = 22f, float scale = 1f, int vertPad = 4, string tooltip = null)
        {
            UITextField textField = parent.AddUIComponent<UITextField>();

            // Size and position.
            textField.size = new Vector2(width, height);
            textField.relativePosition = new Vector2(posX, posY);

            // Text settings.
            textField.textScale = scale;
            textField.padding = new RectOffset(6, 6, vertPad, 3);
            textField.horizontalAlignment = UIHorizontalAlignment.Center;

            // Behaviour.
            textField.builtinKeyNavigation = true;
            textField.isInteractive = true;
            textField.readOnly = false;

            // Appearance.
            textField.color = new Color32(255, 255, 255, 255);
            textField.textColor = new Color32(0, 0, 0, 255);
            textField.disabledTextColor = new Color32(0, 0, 0, 128);
            textField.selectionSprite = "EmptySprite";
            textField.selectionBackgroundColor = new Color32(0, 172, 234, 255);
            textField.normalBgSprite = "TextFieldPanelHovered";
            textField.disabledBgSprite = "TextFieldPanel";

            // Add tooltip.
            if (tooltip != null)
            {
                textField.tooltip = tooltip;
            }

            return textField;
        }
    }
}