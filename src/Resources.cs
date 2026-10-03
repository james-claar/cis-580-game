using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace cis_580_game;

public enum GameState
{
    TitleScreen,
    Playing,
    Paused,
    Loading
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

    private GameState _gameState = GameState.Loading;

    private GameState _nextGameState = GameState.Loading;

    /// <summary>
    /// The game's current state
    /// </summary>
    public GameState CurrentGameState
    {
        get => _gameState;
        set => _nextGameState = value;
    }

    private TimeSpan _lastGameStateChangeTimestamp = TimeSpan.Zero;

    /// <summary>
    /// Last time the game state changed
    /// </summary>
    public TimeSpan LastGameStateChangeTimestamp => _lastGameStateChangeTimestamp;

    /// <summary>
    /// Random number generator
    /// </summary>
    public Random RNG = new();


    // Textures

    public SpriteFont ArialFont {get; private set;}

    public Texture2D MenuButtonTexture {get; private set;}

    public Texture2D AsteroidTilemapTexture {get; private set;}

    public Texture2D MainShipTilemapTexture {get; private set;}
    public List<Texture2D> MainShipBases {get; private set;} = [];
    public List<Texture2D> MainShipEngineEffects {get; private set;} = [];
    public List<Texture2D> MainShipEngines {get; private set;} = [];
    public List<Texture2D> MainShipShields {get; private set;} = [];
    public List<Texture2D> MainShipWeapons {get; private set;} = [];
    public List<Texture2D> MainShipProjectiles {get; private set;} = [];

    // Music

    public List<Song> BackgroundMusic {get; private set;} = [];


    // SFX
    public List<SoundEffect> PlayerShipShootSFX {get; private set;} = [];


    // State

    private bool _isLoaded = false;

    /// <summary>
    /// Whether the resources have been loaded
    /// </summary>
    public bool IsLoaded => _isLoaded;

    public event EventHandler<GameStateChangedEventArgs> GameStateChangedEvent;

    public Resources(MainGame game)
    {
        Game = game;
        Input = new(this);
    }

    /// <summary>
    /// Updates all resources
    /// </summary>
    /// <param name="gt">The GameTime</param>
    public void Update(GameTime gt)
    {
        Input.Update(gt);

        // TODO: Add transition updates
        if (_nextGameState != _gameState)
        {
            switch(_nextGameState)
            {
                case GameState.TitleScreen:
                    // TODO: Unload all game elements
                    break;
            }

            GameState oldState = _gameState;
            _gameState = _nextGameState;
            _lastGameStateChangeTimestamp = gt.TotalGameTime;

            GameStateChangedEvent?.Invoke(this, new GameStateChangedEventArgs {OldState=oldState, NewState=_nextGameState});
        }
    }

    /// <summary>
    /// Loads all necessary content
    /// </summary>
    /// <param name="content">The ContentManager</param>
    public void LoadContent(ContentManager content)
    {
        Input.LoadContent(content);

        // Load assets
        ArialFont = content.Load<SpriteFont>("arial");
        MenuButtonTexture = content.Load<Texture2D>("Complete_UI_Essential_Pack_Free/01_Flat_Theme/Sprites/UI_Flat_Bar07a");
        AsteroidTilemapTexture = content.Load<Texture2D>("Pixel_Art_Package_Asteroids/PixelStarshipsPackage_Asteroids_01");

        // Main ship spritesheets
        MainShipBases.Add(content.Load<Texture2D>("Foozle_2DS0011_Void_MainShip/Main Ship/Main Ship - Bases/PNGs/Main Ship - Base - Full health"));
        MainShipBases.Add(content.Load<Texture2D>("Foozle_2DS0011_Void_MainShip/Main Ship/Main Ship - Bases/PNGs/Main Ship - Base - Slight damage"));
        MainShipBases.Add(content.Load<Texture2D>("Foozle_2DS0011_Void_MainShip/Main Ship/Main Ship - Bases/PNGs/Main Ship - Base - Damaged"));
        MainShipBases.Add(content.Load<Texture2D>("Foozle_2DS0011_Void_MainShip/Main Ship/Main Ship - Bases/PNGs/Main Ship - Base - Very damaged"));

        MainShipEngineEffects.Add(content.Load<Texture2D>("Foozle_2DS0011_Void_MainShip/Main Ship/Main Ship - Engine Effects/PNGs/Main Ship - Engines - Base Engine - Spritesheet"));
        MainShipEngineEffects.Add(content.Load<Texture2D>("Foozle_2DS0011_Void_MainShip/Main Ship/Main Ship - Engine Effects/PNGs/Main Ship - Engines - Big Pulse Engine - Spritesheet"));
        MainShipEngineEffects.Add(content.Load<Texture2D>("Foozle_2DS0011_Void_MainShip/Main Ship/Main Ship - Engine Effects/PNGs/Main Ship - Engines - Burst Engine - Spritesheet"));
        MainShipEngineEffects.Add(content.Load<Texture2D>("Foozle_2DS0011_Void_MainShip/Main Ship/Main Ship - Engine Effects/PNGs/Main Ship - Engines - Supercharged Engine - Spritesheet"));

        MainShipEngines.Add(content.Load<Texture2D>("Foozle_2DS0011_Void_MainShip/Main Ship/Main Ship - Engines/PNGs/Main Ship - Engines - Base Engine"));
        MainShipEngines.Add(content.Load<Texture2D>("Foozle_2DS0011_Void_MainShip/Main Ship/Main Ship - Engines/PNGs/Main Ship - Engines - Big Pulse Engine"));
        MainShipEngines.Add(content.Load<Texture2D>("Foozle_2DS0011_Void_MainShip/Main Ship/Main Ship - Engines/PNGs/Main Ship - Engines - Burst Engine"));
        MainShipEngines.Add(content.Load<Texture2D>("Foozle_2DS0011_Void_MainShip/Main Ship/Main Ship - Engines/PNGs/Main Ship - Engines - Supercharged Engine"));

        MainShipShields.Add(content.Load<Texture2D>("Foozle_2DS0011_Void_MainShip/Main Ship/Main Ship - Shields/PNGs/Main Ship - Shields - Front and Side Shield"));
        MainShipShields.Add(content.Load<Texture2D>("Foozle_2DS0011_Void_MainShip/Main Ship/Main Ship - Shields/PNGs/Main Ship - Shields - Front Shield"));
        MainShipShields.Add(content.Load<Texture2D>("Foozle_2DS0011_Void_MainShip/Main Ship/Main Ship - Shields/PNGs/Main Ship - Shields - Invincibility Shield"));
        MainShipShields.Add(content.Load<Texture2D>("Foozle_2DS0011_Void_MainShip/Main Ship/Main Ship - Shields/PNGs/Main Ship - Shields - Round Shield"));

        MainShipWeapons.Add(content.Load<Texture2D>("Foozle_2DS0011_Void_MainShip/Main Ship/Main Ship - Weapons/PNGs/Main Ship - Weapons - Auto Cannon"));
        MainShipWeapons.Add(content.Load<Texture2D>("Foozle_2DS0011_Void_MainShip/Main Ship/Main Ship - Weapons/PNGs/Main Ship - Weapons - Big Space Gun"));
        MainShipWeapons.Add(content.Load<Texture2D>("Foozle_2DS0011_Void_MainShip/Main Ship/Main Ship - Weapons/PNGs/Main Ship - Weapons - Rockets"));
        MainShipWeapons.Add(content.Load<Texture2D>("Foozle_2DS0011_Void_MainShip/Main Ship/Main Ship - Weapons/PNGs/Main Ship - Weapons - Zapper"));

        MainShipProjectiles.Add(content.Load<Texture2D>("Foozle_2DS0011_Void_MainShip/Main ship weapons/PNGs/Main ship weapon - Projectile - Auto cannon bullet"));
        MainShipProjectiles.Add(content.Load<Texture2D>("Foozle_2DS0011_Void_MainShip/Main ship weapons/PNGs/Main ship weapon - Projectile - Big Space Gun"));
        MainShipProjectiles.Add(content.Load<Texture2D>("Foozle_2DS0011_Void_MainShip/Main ship weapons/PNGs/Main ship weapon - Projectile - Rocket"));
        MainShipProjectiles.Add(content.Load<Texture2D>("Foozle_2DS0011_Void_MainShip/Main ship weapons/PNGs/Main ship weapon - Projectile - Zapper"));

        // Music

        BackgroundMusic.Add(content.Load<Song>("Unpublished_Dark_SciFi_Synth_Music/Invasion"));
        BackgroundMusic.Add(content.Load<Song>("Unpublished_Dark_SciFi_Synth_Music/Mechanization"));
        BackgroundMusic.Add(content.Load<Song>("Unpublished_Dark_SciFi_Synth_Music/Pollution"));
        BackgroundMusic.Add(content.Load<Song>("Unpublished_Dark_SciFi_Synth_Music/Reality"));
        BackgroundMusic.Add(content.Load<Song>("Unpublished_Dark_SciFi_Synth_Music/Underground City"));
        BackgroundMusic.Add(content.Load<Song>("Unpublished_Dark_SciFi_Synth_Music/Watcher"));


        // SFX

        for (int i=0; i<11; i++) PlayerShipShootSFX.Add(content.Load<SoundEffect>("Laser_Weapons_SFX/light_blast_" + (i+1)));

        _isLoaded = true;
        CurrentGameState = GameState.TitleScreen;
    }
}