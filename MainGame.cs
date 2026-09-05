using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace cis_580_game;

public enum GameState
{
    TitleScreen = 0,
    Paused = 1,
    Playing = 2
}

public class MainGame : Game
{

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private GameState _gameState = GameState.TitleScreen;

    // TODO: Move to its own class
    private List<MenuButton> _menuButtons = [];

    private SpriteFont arial;

    private Vector2 titlePosition = new();

    /// <summary>
    /// The game's current state
    /// </summary>
    public GameState CurrentGameState
    {
        get => _gameState;
        set
        {
            // TODO: Add transition updates
            switch(value)
            {
                case GameState.TitleScreen:
                    // TODO: Unload all game elements
                    break;
            }

            if (GameState.IsDefined(value)) _gameState = value;
        }
    }

    /// <summary>
    /// Whether the game's physics and timers should run
    /// </summary>
    public bool GameRunning => CurrentGameState == GameState.Playing;

    public MainGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Window.AllowUserResizing = true;
    }

    /// <summary>
    /// Event triggered when the user resizes the game window
    /// </summary>
    /// <param name="sender">The sender of the event</param>
    /// <param name="e">The event arguments</param>
    protected void Window_ClientSizeChanged(object sender, EventArgs e)
    {
        // TODO: Handle window resize
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        CurrentGameState = GameState.TitleScreen;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        arial = Content.Load<SpriteFont>("arial");

        MenuButton playButton1 = new(new Vector2(200, 30+1*85), 200, 75, "Play",  arial, Color.Black, "Complete_UI_Essential_Pack_Free/01_Flat_Theme/Sprites/UI_Flat_Banner03a");
        MenuButton playButton2 = new(new Vector2(200, 30+2*85), 200, 75, "Beans", arial, Color.Black, "Complete_UI_Essential_Pack_Free/01_Flat_Theme/Sprites/UI_Flat_Banner03a");
        MenuButton playButton3 = new(new Vector2(200, 30+3*85), 200, 75, "Settings", arial, Color.Black, "Complete_UI_Essential_Pack_Free/01_Flat_Theme/Sprites/UI_Flat_Banner03a");
        MenuButton playButton4 = new(new Vector2(200, 30+4*85), 200, 75, "Exit",  arial, Color.Black, "Complete_UI_Essential_Pack_Free/01_Flat_Theme/Sprites/UI_Flat_Banner03a");

        _menuButtons.Add(playButton1);
        _menuButtons.Add(playButton2);
        _menuButtons.Add(playButton3);
        _menuButtons.Add(playButton4);

        foreach (MenuButton playButton in _menuButtons) playButton.LoadContent(Content);

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        titlePosition.X = 150+75*(float)Math.Sin((float)gameTime.TotalGameTime.TotalMilliseconds/1000f);
        titlePosition.Y = 35;
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin(SpriteSortMode.BackToFront);

        _spriteBatch.DrawString(arial, "Press 'Back' or 'Escape' to exit", new Vector2(20, 20), Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.5f);
        _spriteBatch.DrawString(arial, "Blasteroidz", titlePosition, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0.5f);
        foreach (var button in _menuButtons) button.Draw(gameTime, _spriteBatch);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
