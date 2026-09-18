using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;

namespace cis_580_game;

/// <summary>
/// Main class for handling all GUI elements and interactions
/// </summary>
public class GuiManager : IScreen
{
    private Resources _resources;
    private MainMenuScreen _mainMenuScreen;

    public GuiManager(Resources resources)
    {
        _resources = resources;

        _mainMenuScreen = new(resources);
    }

    /// <summary>
    /// Loads all necessary content
    /// </summary>
    /// <param name="content">The ContentManager</param>
    public void LoadContent(ContentManager content)
    {
        _mainMenuScreen.LoadContent(content);
    }

    /// <summary>
    /// Updates everything in the screen
    /// </summary>
    /// <param name="gt">The GameTime</param>
    public void Update(GameTime gt)
    {
        _mainMenuScreen.Update(gt);
    }

    /// <summary>
    /// Draws this screen
    /// </summary>
    /// <param name="gt">The GameTime</param>
    /// <param name="sb">The SpriteBatch to draw using</param>
    public void Draw(GameTime gt, SpriteBatch sb)
    {
        _mainMenuScreen.Draw(gt, sb);
    }
}
