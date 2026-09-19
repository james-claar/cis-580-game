using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace cis_580_game;

public enum GameState
{
    TitleScreen = 0,
    Playing = 1,
    Paused = 2,
    Loading = 3
}

public class GameStateChangedEventArgs
{
    /// <summary>
    /// Old state to change from
    /// </summary>
    public GameState OldState;

    /// <summary>
    /// New state to change to
    /// </summary>
    public GameState NewState;
}

public class Resources
{
    // Parent
    public MainGame Game;

    // Graphics

    /// <summary>
    /// The graphics device manager
    /// </summary>
    public GraphicsDeviceManager Graphics;

    /// <summary>
    /// The default renderer, with no default scaling
    /// </summary>
    public SpriteBatch SpriteBatch;

    /// <summary>
    /// Custom wrapper for SpriteBatch
    /// </summary>
    public ScaledRenderer ScaledRenderer;

    // Input
    public InputHandler Input;


    // Engine

    private GameState _gameState = GameState.TitleScreen;

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

            GameState oldState = _gameState;
            _gameState = value;

            GameStateChangedEvent?.Invoke(this, new GameStateChangedEventArgs {OldState=oldState, NewState=value});
        }
    }

    // Assets

    public SpriteFont ArialFont {get; private set;}

    public Texture2D MenuButtonTexture {get; private set;}

    public Texture2D AsteroidTilemapTexture {get; private set;}


    // State

    private bool _isLoaded = false;

    /// <summary>
    /// Whether the resources have been loaded
    /// </summary>
    public bool IsLoaded => _isLoaded;

    public event EventHandler<GameStateChangedEventArgs> GameStateChangedEvent;

    public Resources()
    {
        CurrentGameState = GameState.TitleScreen;
    }

    /// <summary>
    /// Updates all resources
    /// </summary>
    /// <param name="gt">The GameTime</param>
    public void Update(GameTime gt)
    {
        Input?.Update(gt);
    }

    /// <summary>
    /// Loads all necessary content
    /// </summary>
    /// <param name="content">The ContentManager</param>
    public void LoadContent(ContentManager content)
    {
        // Load assets
        ArialFont = content.Load<SpriteFont>("arial");
        MenuButtonTexture = content.Load<Texture2D>("Complete_UI_Essential_Pack_Free/01_Flat_Theme/Sprites/UI_Flat_Banner03a");
        AsteroidTilemapTexture = content.Load<Texture2D>("Pixel_Art_Package_Asteroids/PixelStarshipsPackage_Asteroids_01");

        // Load components
        Input = new(ScaledRenderer);

        _isLoaded = true;
    }
}