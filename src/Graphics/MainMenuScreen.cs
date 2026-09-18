using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace cis_580_game;

/// <summary>
/// Main class for handling all GUI elements and interactions
/// </summary>
public class MainMenuScreen : IScreen
{
    private Resources _resources;

    private Vector2 _titlePosition = Vector2.Zero;

    public MainMenuScreen(Resources resources)
    {
        _resources = resources;
    }

    public List<MenuButton> ClickableButtons = [];


    private bool _visible = true;
    public bool Visible
    {
        get => _visible;
        set
        {
            _visible = value;
            foreach (MenuButton button in ClickableButtons) button.Visible = value;
        }
    }

    /// <summary>
    /// Loads all necessary content
    /// </summary>
    /// <param name="content">The ContentManager</param>
    public void LoadContent(ContentManager content)
    {
        MenuButton playButton = new(_resources, new Vector2(ScaledRenderer.VirtualScreenHorizontalCenter, 200+0*200), 600, 150, "Play", _resources.ArialFont, Color.Black, Color.White, HorizontalAlignment.Centered);
        MenuButton dummyButton = new(_resources, new Vector2(ScaledRenderer.VirtualScreenHorizontalCenter, 200+1*200), 600, 150, "Beans", _resources.ArialFont, Color.Black, Color.White, HorizontalAlignment.Centered);
        MenuButton settingsButton = new(_resources, new Vector2(ScaledRenderer.VirtualScreenHorizontalCenter, 200+2*200), 600, 150, "Settings", _resources.ArialFont, Color.Black, Color.White, HorizontalAlignment.Centered);
        MenuButton exitButton = new(_resources, new Vector2(ScaledRenderer.VirtualScreenHorizontalCenter, 200+3*200), 600, 150, "Exit", _resources.ArialFont, Color.Black, Color.White, HorizontalAlignment.Centered);

        ClickableButtons.AddRange(
            playButton,
            dummyButton,
            settingsButton,
            exitButton
        );

        playButton.ClickEvent += HandlePlayButtonClick;
        // TODO: Add settings button handler
        exitButton.ClickEvent += HandleExitButtonClick;

        _resources.GameStateChangedEvent += HandleGameStateChanged;
    }

    /// <summary>
    /// Updates everything in the screen
    /// </summary>
    /// <param name="gt">The GameTime</param>
    public void Update(GameTime gt)
    {
        _titlePosition.X = ScaledRenderer.VirtualScreenHorizontalCenter+75*(float)Math.Sin((float)gt.TotalGameTime.TotalMilliseconds/1000f);
        _titlePosition.Y = 35;

        foreach (MenuButton button in ClickableButtons) button.Update(gt);
    }

    /// <summary>
    /// Draws this screen
    /// </summary>
    /// <param name="sb">SpriteBatch to draw using</param>
    /// <param name="gt">The GameTime</param>
    public void Draw(GameTime gt, SpriteBatch sb)
    {
        if (Visible)
        {
            foreach (MenuButton button in ClickableButtons) button.Draw(gt, sb);

            _resources.ScaledRenderer.DrawString(_resources.ArialFont, "Press 'Back' or 'Escape' to exit", new Vector2(20, 20), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, Layers.GuiObjectsForeground);
            _resources.ScaledRenderer.DrawString(_resources.ArialFont, "Blasteroidz", _titlePosition, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, Layers.GuiObjectsForeground);
        }
    }

    public void HandleGameStateChanged(object sender, GameStateChangedEventArgs e)
    {
        if (e.OldState == GameState.TitleScreen)
        {
            Visible = false;
        }
        if (e.NewState == GameState.TitleScreen)
        {
            Visible = true;
        }
    }

    public void HandlePlayButtonClick(object sender, ButtonClickEventArgs e)
    {
        _resources.CurrentGameState = GameState.Playing;
    }

    public void HandleExitButtonClick(object sender, ButtonClickEventArgs e)
    {
        _resources.Game.Exit();
    }
}
