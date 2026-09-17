using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace cis_580_game;

public class MenuButton : GuiElement
{
    private Texture2D _texture;

    /// <summary>
    /// The button text's font
    /// </summary>
    private SpriteFont font;

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
    /// Whether the button is selected
    /// </summary>
    public bool Selected;

    /// <summary>
    /// Constructor that creates a primitive rectangular button
    /// </summary>
    /// <param name="position">The button's position to align to</param>
    /// <param name="width">The button's width</param>
    /// <param name="height">The button's height</param>
    /// <param name="text">The label text</param>
    /// <param name="color">Color of the button's rectangle</param>
    /// <param name="textColor">Color of the button's rectangle</param>
    public MenuButton(Vector2 position, float width, float height, string text, SpriteFont textFont, Color textColor, Color backgroundColor, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment)
    {
        Position = position;
        Width = width;
        Height = height;
        Text = text;
        font = textFont;
        BackgroundColor = backgroundColor;
        TextColor = textColor;
        HorizontalAlignment = horizontalAlignment;
        VerticalAlignment = verticalAlignment;
    }

    /// <summary>
    /// Constructor that creates a textured rectangular button
    /// </summary>
    /// <param name="position"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="text"></param>
    /// <param name="textureFileName"></param>
    public MenuButton(Resources resources, Vector2 position, float width, float height, string text, SpriteFont textFont, Color textColor, Texture2D texture, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment)
    {
        _resources = resources;
        Position = position;
        Width = width;
        Height = height;
        Text = text;
        font = textFont;
        BackgroundColor = Color.White;
        TextColor = textColor;
        _texture = texture;
        HorizontalAlignment = horizontalAlignment;
        VerticalAlignment = verticalAlignment;
    }

    /// <summary>
    /// Updates the button
    /// </summary>
    /// <param name="gameTime">The GameTime</param>
    public void Update(GameTime gameTime)
    {
        // TODO: Add something here
    }

    /// <summary>
    /// Draws the sprite using the supplied SpriteBatch
    /// </summary>
    /// <param name="gameTime">The game time</param>
    /// <param name="spriteBatch">The spritebatch to render with</param>
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        // Draw texture
        if (_texture != null)
        {
            _resources.ScaledRenderer.Draw(_texture, BoundingBox.ToRectangle(), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, Layers.GuiObjectsBackground);
        }
        else
        {
            PrimitiveRenderer.DrawRectangle(_resources.ScaledRenderer, gameTime, spriteBatch, BoundingBox.ToRectangle(), BackgroundColor, Layers.GuiObjectsBackground);
        }

        // Draw text inside button
        _resources.ScaledRenderer.DrawString(font, Text, new Vector2(BoundingBox.X+50, BoundingBox.Y+10), TextColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, Layers.GuiObjectsForeground);
    }
}
