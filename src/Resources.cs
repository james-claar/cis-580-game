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
    /// The default renderer, with no scaling
    /// </summary>
    public SpriteBatch BaseRenderer;

    /// <summary>
    /// The renderer for the UI
    /// </summary>
    public SpriteBatch GuiRenderer;

    /// <summary>
    /// The renderer for the gameplay layer
    /// </summary>
    public SpriteBatch GameplayRenderer;

    /// <summary>
    /// The virtual screen for the GUI
    /// </summary>
    public VirtualScreen VirtualScreen;

    /// <summary>
    /// The camera for the gameplay
    /// </summary>
    public Camera Camera;

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

    /// <summary>
    /// Save manager
    /// </summary>
    public SaveManager SaveManager = new();


    // Fonts
    public SpriteFont ArialFont {get; private set;}


    // Textures
    public Texture2D BlankTexture {get; private set;}

    public Texture2D MenuButtonTexture {get; private set;}

    public Texture2D AsteroidTilemapTexture {get; private set;}

    public Tilemap MainShipBase {get; private set;}
    public List<Tilemap> MainShipEngineEffectTilemaps {get; private set;} = [];
    public Tilemap MainShipEngines {get; private set;}
    public List<Tilemap> MainShipShieldTilemaps {get; private set;} = [];
    public List<Tilemap> MainShipWeaponTilemaps {get; private set;} = [];
    public List<Tilemap> MainShipProjectiles {get; private set;} = [];

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

    // Sprites

    public PlayerShip PlayerShip;



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

        if (CurrentGameState == GameState.Playing)
        {
            PlayerShip.Update(gt);
            SaveGame(isAutosave: true);
        }
    }

    /// <summary>
    /// Loads all necessary content
    /// </summary>
    /// <param name="content">The ContentManager</param>
    public void LoadContent(ContentManager content)
    {
        Input.LoadContent(content);
        Input.Keybinds[KeybindNames.SaveGame].PressedEvent += HandleKeybind_SaveGame;
        Input.Keybinds[KeybindNames.LoadGame].PressedEvent += HandleKeybind_LoadGame;

        // Load assets
        ArialFont = content.Load<SpriteFont>("arial");

        BlankTexture = BaseRenderer.BlankTexture;
        MenuButtonTexture = content.Load<Texture2D>("Complete_UI_Essential_Pack_Free/01_Flat_Theme/Sprites/UI_Flat_Bar07a");
        AsteroidTilemapTexture = content.Load<Texture2D>("Pixel_Art_Package_Asteroids/PixelStarshipsPackage_Asteroids_01");

        // Main ship spritesheets
        Tilemap currentTilemap;

        MainShipBase = new(Texture2D.HorizontalConcatenation(
            content.Load<Texture2D>("Foozle_2DS0011_Void_MainShip/Main Ship/Main Ship - Bases/PNGs/Main Ship - Base - Full health"),
            content.Load<Texture2D>("Foozle_2DS0011_Void_MainShip/Main Ship/Main Ship - Bases/PNGs/Main Ship - Base - Slight damage"),
            content.Load<Texture2D>("Foozle_2DS0011_Void_MainShip/Main Ship/Main Ship - Bases/PNGs/Main Ship - Base - Damaged"),
            content.Load<Texture2D>("Foozle_2DS0011_Void_MainShip/Main Ship/Main Ship - Bases/PNGs/Main Ship - Base - Very damaged")
        ), 48, 48, null, (0, 0));

        currentTilemap = new(content.Load<Texture2D>("Foozle_2DS0011_Void_MainShip/Main Ship/Main Ship - Engine Effects/PNGs/Main Ship - Engines - Base Engine - Spritesheet"), 48, 48);
        currentTilemap.AddAnimation((0, 0), 3);
        currentTilemap.AddAnimation((0, 1), 4);
        MainShipEngineEffectTilemaps.Add(currentTilemap);
        currentTilemap = new(content.Load<Texture2D>("Foozle_2DS0011_Void_MainShip/Main Ship/Main Ship - Engine Effects/PNGs/Main Ship - Engines - Big Pulse Engine - Spritesheet"), 48, 48);
        currentTilemap.AddAnimation((0, 0), 4);
        currentTilemap.AddAnimation((0, 1), 4);
        MainShipEngineEffectTilemaps.Add(currentTilemap);
        currentTilemap = new(content.Load<Texture2D>("Foozle_2DS0011_Void_MainShip/Main Ship/Main Ship - Engine Effects/PNGs/Main Ship - Engines - Burst Engine - Spritesheet"), 48, 48);
        currentTilemap.AddAnimation((0, 0), 7);
        currentTilemap.AddAnimation((0, 1), 6);
        MainShipEngineEffectTilemaps.Add(currentTilemap);
        currentTilemap = new(content.Load<Texture2D>("Foozle_2DS0011_Void_MainShip/Main Ship/Main Ship - Engine Effects/PNGs/Main Ship - Engines - Supercharged Engine - Spritesheet"), 48, 48);
        currentTilemap.AddAnimation((1, 0), 3);
        currentTilemap.AddAnimation((0, 1), 4);
        MainShipEngineEffectTilemaps.Add(currentTilemap);

        MainShipEngines = new(Texture2D.HorizontalConcatenation(
            content.Load<Texture2D>("Foozle_2DS0011_Void_MainShip/Main Ship/Main Ship - Engines/PNGs/Main Ship - Engines - Base Engine"),
            content.Load<Texture2D>("Foozle_2DS0011_Void_MainShip/Main Ship/Main Ship - Engines/PNGs/Main Ship - Engines - Big Pulse Engine"),
            content.Load<Texture2D>("Foozle_2DS0011_Void_MainShip/Main Ship/Main Ship - Engines/PNGs/Main Ship - Engines - Burst Engine"),
            content.Load<Texture2D>("Foozle_2DS0011_Void_MainShip/Main Ship/Main Ship - Engines/PNGs/Main Ship - Engines - Supercharged Engine")
        ), 48, 48, null, (0, 0));

        currentTilemap = new(content.Load<Texture2D>("Foozle_2DS0011_Void_MainShip/Main Ship/Main Ship - Shields/PNGs/Main Ship - Shields - Front and Side Shield"), 48, 48);
        currentTilemap.AddAnimation((0, 0), 6);
        MainShipShieldTilemaps.Add(currentTilemap);
        currentTilemap = new(content.Load<Texture2D>("Foozle_2DS0011_Void_MainShip/Main Ship/Main Ship - Shields/PNGs/Main Ship - Shields - Front Shield"), 48, 48);
        currentTilemap.AddAnimation((0, 0), 10);
        MainShipShieldTilemaps.Add(currentTilemap);
        currentTilemap = new(content.Load<Texture2D>("Foozle_2DS0011_Void_MainShip/Main Ship/Main Ship - Shields/PNGs/Main Ship - Shields - Invincibility Shield"), 48, 48);
        currentTilemap.AddAnimation((0, 0), 10);
        MainShipShieldTilemaps.Add(currentTilemap);
        currentTilemap = new(content.Load<Texture2D>("Foozle_2DS0011_Void_MainShip/Main Ship/Main Ship - Shields/PNGs/Main Ship - Shields - Round Shield"), 48, 48);
        currentTilemap.AddAnimation((0, 0), 10);
        MainShipShieldTilemaps.Add(currentTilemap);

        currentTilemap = new(content.Load<Texture2D>("Foozle_2DS0011_Void_MainShip/Main Ship/Main Ship - Weapons/PNGs/Main Ship - Weapons - Auto Cannon"), 48, 48, null, (0, 0));
        currentTilemap.AddAnimation((1, 0), 6);
        MainShipWeaponTilemaps.Add(currentTilemap);
        currentTilemap = new(content.Load<Texture2D>("Foozle_2DS0011_Void_MainShip/Main Ship/Main Ship - Weapons/PNGs/Main Ship - Weapons - Big Space Gun"), 48, 48, null, (0, 0));
        currentTilemap.AddAnimation((1, 0), 6);
        currentTilemap.AddAnimation((7, 0), 5);
        MainShipWeaponTilemaps.Add(currentTilemap);
        currentTilemap = new(content.Load<Texture2D>("Foozle_2DS0011_Void_MainShip/Main Ship/Main Ship - Weapons/PNGs/Main Ship - Weapons - Rockets"), 48, 48, null, (0, 0));
        currentTilemap.AddAnimation((1, 0), 15);
        MainShipWeaponTilemaps.Add(currentTilemap);
        currentTilemap = new(content.Load<Texture2D>("Foozle_2DS0011_Void_MainShip/Main Ship/Main Ship - Weapons/PNGs/Main Ship - Weapons - Zapper"), 48, 48, null, (0, 0));
        currentTilemap.AddAnimation((1, 0), 6);
        currentTilemap.AddAnimation((7, 0), 5);
        MainShipWeaponTilemaps.Add(currentTilemap);

        currentTilemap = new(content.Load<Texture2D>("Foozle_2DS0011_Void_MainShip/Main ship weapons/PNGs/Main ship weapon - Projectile - Auto cannon bullet"), 32, 32, null, (0, 0));
        currentTilemap.AddAnimation((0, 0), 4);
        MainShipProjectiles.Add(currentTilemap);
        currentTilemap = new(content.Load<Texture2D>("Foozle_2DS0011_Void_MainShip/Main ship weapons/PNGs/Main ship weapon - Projectile - Big Space Gun"), 32, 32, null, (0, 0));
        currentTilemap.AddAnimation((0, 0), 5);
        MainShipProjectiles.Add(currentTilemap);
        currentTilemap = new(content.Load<Texture2D>("Foozle_2DS0011_Void_MainShip/Main ship weapons/PNGs/Main ship weapon - Projectile - Rocket"), 32, 32, null, (0, 0));
        currentTilemap.AddAnimation((0, 0), 3);
        MainShipProjectiles.Add(currentTilemap);
        currentTilemap = new(content.Load<Texture2D>("Foozle_2DS0011_Void_MainShip/Main ship weapons/PNGs/Main ship weapon - Projectile - Zapper"), 32, 32, null, (0, 0));
        currentTilemap.AddAnimation((0, 0), 8);
        MainShipProjectiles.Add(currentTilemap);

        // Music

        BackgroundMusic.Add(content.Load<Song>("Unpublished_Dark_SciFi_Synth_Music/Invasion"));
        BackgroundMusic.Add(content.Load<Song>("Unpublished_Dark_SciFi_Synth_Music/Mechanization"));
        BackgroundMusic.Add(content.Load<Song>("Unpublished_Dark_SciFi_Synth_Music/Pollution"));
        BackgroundMusic.Add(content.Load<Song>("Unpublished_Dark_SciFi_Synth_Music/Reality"));
        BackgroundMusic.Add(content.Load<Song>("Unpublished_Dark_SciFi_Synth_Music/Underground City"));
        BackgroundMusic.Add(content.Load<Song>("Unpublished_Dark_SciFi_Synth_Music/Watcher"));


        // SFX

        for (int i=0; i<11; i++) PlayerShipShootSFX.Add(content.Load<SoundEffect>("Laser_Weapons_SFX/light_blast_" + (i+1)));


        // Sprites
        PlayerShip = new(this);


        // Finalize

        _isLoaded = true;
        CurrentGameState = GameState.TitleScreen;
    }

    public void SaveGame(bool isAutosave = false)
    {
        SavedState state = new()
        {
            SavedAt = DateTime.UtcNow,
            PlayerShip = PlayerShip
        };
        SaveManager.CreateSaveFile(state, isAutosave);
    }

    public void LoadGame(bool isAutosave = false)
    {
        SavedState state = SaveManager.LoadSaveFile(isAutosave);
        if (state is null) return;

        PlayerShip = state.PlayerShip;
        PlayerShip.Initialize(this);
    }

    public void HandleKeybind_SaveGame(object sender, KeybindEventArgs e)
    {
        SaveGame();
    }

    public void HandleKeybind_LoadGame(object sender, KeybindEventArgs e)
    {
        LoadGame();
    }
}
