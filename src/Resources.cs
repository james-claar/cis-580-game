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

public class Resources
{
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

            if (GameState.IsDefined(value)) _gameState = value;
        }
    }


    // Assets

    public SpriteFont ArialFont;

    public Texture2D MenuButtonTexture {get; private set;}



    public void Initialize()
    {
        CurrentGameState = GameState.TitleScreen;
    }

    /// <summary>
    /// Loads all necessary content
    /// </summary>
    /// <param name="content">The ContentManager</param>
    public void LoadContent(ContentManager content)
    {
        ArialFont = content.Load<SpriteFont>("arial");
        MenuButtonTexture = content.Load<Texture2D>("Complete_UI_Essential_Pack_Free/01_Flat_Theme/Sprites/UI_Flat_Banner03a");
    }
}