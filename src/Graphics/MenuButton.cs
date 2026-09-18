using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace cis_580_game;


public class ButtonHoverEventArgs : EventArgs
{
    /// <summary>
    /// Whether the button is currently being hovered over.
    /// </summary>
    public bool CurrentlyHovering { get; set; }
}

public class ButtonClickEventArgs : EventArgs
{
    /// <summary>
    /// Whether the button was pressed (true) or released (false).
    /// </summary>
    public bool IsPressed { get; set; }
}


public class MenuButton : GuiElement
{
    private Texture2D _texture;

    /// <summary>
    /// The button text's font
    /// </summary>
    private SpriteFont _font;

    /// <summary>
    /// The text of the button
    /// </summary>
    public string Text;

    /// <summary>
    /// The button's background color
    /// </summary>
    public Color BackgroundColor;

    /// <summary>
    /// The button's color
    /// </summary>
    public Color TextColor;

    /// <summary>
    /// Whether the mouse is currently over the button
    /// </summary>
    public bool IsHoveredOver;

    /// <summary>
    /// Whether the button is selected
    /// </summary>
    public bool Selected;

    /// <summary>
    /// Triggered when the mouse moves onto or off of the button
    /// </summary>
    public EventHandler<ButtonHoverEventArgs> HoverEvent;

    /// <summary>
    /// Triggered when the user clicks the button with the mouse
    /// </summary>
    public EventHandler<ButtonClickEventArgs> ClickEvent;

    /// <summary>
    /// Constructor that creates a primitive rectangular button
    /// </summary>
    /// <param name="position">The button's position to align to</param>
    /// <param name="width">The button's width</param>
    /// <param name="height">The button's height</param>
    /// <param name="text">The label text</param>
    /// <param name="color">Color of the button's rectangle</param>
    /// <param name="textColor">Color of the button's rectangle</param>
    public MenuButton(Resources resources, Vector2 position, float width, float height, string text, SpriteFont textFont, Color textColor, Color backgroundColor, Alignment alignment)
    {
        IsSelectable = true;
        _resources = resources;
        Position = position;
        Width = width;
        Height = height;
        Text = text;
        _font = textFont;
        BackgroundColor = backgroundColor;
        TextColor = textColor;
        Alignment = alignment;
        _resources.Input.Keybinds.GuiClickButton.TriggerEvent += HandleGuiButtonClickKeybind;
    }

    /// <summary>
    /// Constructor that creates a textured rectangular button
    /// </summary>
    /// <param name="position"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="text"></param>
    /// <param name="textureFileName"></param>
    public MenuButton(Resources resources, Vector2 position, float width, float height, string text, SpriteFont textFont, Color textColor, Texture2D texture, Alignment alignment)
    {
        IsSelectable = true;
        _resources = resources;
        Position = position;
        Width = width;
        Height = height;
        Text = text;
        _font = textFont;
        BackgroundColor = Color.White;
        TextColor = textColor;
        _texture = texture;
        Alignment = alignment;
        _resources.Input.Keybinds.GuiClickButton.TriggerEvent += HandleGuiButtonClickKeybind;
    }

    /// <summary>
    /// Updates the button
    /// </summary>
    /// <param name="gameTime">The GameTime</param>
    public void Update(GameTime gameTime)
    {
        // Handle hover
        bool overButton = Visible && _resources.Input.IsMouseOnScreen && BoundingBox.Contains(_resources.Input.VirtualMousePosition);
        if (overButton != IsHoveredOver)
        {
            IsHoveredOver = overButton;
            HoverEvent?.Invoke(this, new ButtonHoverEventArgs {CurrentlyHovering = IsHoveredOver});
            Selected = true;
        }
    }

    /// <summary>
    /// Draws the sprite using the supplied SpriteBatch
    /// </summary>
    /// <param name="gameTime">The game time</param>
    /// <param name="spriteBatch">The spritebatch to render with</param>
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (!Visible) return;

        // Draw texture
        if (_texture is null)
        {
            Color color = IsHoveredOver ? GameColors.SelectedButtonColor : GameColors.ButtonColor;
            PrimitiveRenderer.DrawRectangle(_resources.ScaledRenderer, gameTime, spriteBatch, (Rectangle)BoundingBox, color, Layers.GuiObjectsBackground);
        }
        else
        {
            _resources.ScaledRenderer.Draw(_texture, (Rectangle)BoundingBox, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, Layers.GuiObjectsBackground);
        }

        // Draw text inside button
        _resources.ScaledRenderer.DrawString(_font, Text, new Vector2(BoundingBox.X+20, BoundingBox.Y+20), TextColor, 0f, Vector2.Zero, 3f, SpriteEffects.None, Layers.GuiObjectsForeground);
    }

    /// <summary>
    /// Sends a click event if the mouse is inside the button
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">Event args</param>
    public void HandleGuiButtonClickKeybind(object sender, KeybindEventArgs e)
    {
        if (Visible && IsHoveredOver && e.IsPressed) ClickEvent?.Invoke(this, new ButtonClickEventArgs {IsPressed = true});
    }
}
