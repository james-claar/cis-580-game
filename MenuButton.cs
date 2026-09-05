using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using CollisionExercise;
using System.Xml.Linq;

namespace cis_580_game;

public class MenuButton
{
    private readonly bool useTexture;

    private readonly string textureName;

    private Texture2D texture;

    /// <summary>
    /// The button text's font
    /// </summary>
    private SpriteFont font;

    /// <summary>
    /// This button's rectangular position/boundary
    /// </summary>
    public Rectangle Rectangle;

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
    /// <param name="position">The position of the button's upper left corner</param>
    /// <param name="width">The button's width</param>
    /// <param name="height">The button's height</param>
    /// <param name="text">The label text</param>
    /// <param name="color">Color of the button's rectangle</param>
    /// <param name="textColor">Color of the button's rectangle</param>
    public MenuButton(Vector2 position, int width, int height, string text, SpriteFont textFont, Color textColor, Color backgroundColor)
    {
        Rectangle = new((int)Math.Round(position.X), (int)Math.Round(position.Y), width, height);
        Text = text;
        font = textFont;
        BackgroundColor = backgroundColor;
        TextColor = textColor;
        useTexture = false;
    }

    /// <summary>
    /// Constructor that creates a textured rectangular button
    /// </summary>
    /// <param name="position"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="text"></param>
    /// <param name="textureFileName"></param>
    public MenuButton(Vector2 position, int width, int height, string text, SpriteFont textFont, Color textColor, string textureFileName)
    {
        Rectangle = new((int)Math.Round(position.X), (int)Math.Round(position.Y), width, height);
        Text = text;
        font = textFont;
        BackgroundColor = Color.White;
        TextColor = textColor;
        useTexture = true;
        textureName = textureFileName;
    }

    /// <summary>
    /// Loads the sprite texture using the provided ContentManager
    /// </summary>
    /// <param name="content">The ContentManager to load with</param>
    public void LoadContent(ContentManager content)
    {
        if (useTexture)
        {
            texture = content.Load<Texture2D>(textureName);
        }
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
        if (useTexture)
        {
            spriteBatch.Draw(texture, Rectangle, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.6f);
        }
        else
        {
            PrimitiveRenderer.DrawRectangle(gameTime, spriteBatch, Rectangle, BackgroundColor, 0.6f);
        }

        // Draw text inside button
        spriteBatch.DrawString(font, Text, new Vector2(Rectangle.X+50, Rectangle.Y+10), TextColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
    }
}