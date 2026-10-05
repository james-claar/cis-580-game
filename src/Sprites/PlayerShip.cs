
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace cis_580_game;

public class PlayerShip
{
    private Resources _resources;

    private bool _firePrimaryKeyDown = false;
    private bool _fireSecondaryKeyDown = false;
    private bool _forwardKeyDown = false;
    private bool _backwardKeyDown = false;
    private bool _leftKeyDown = false;
    private bool _rightKeyDown = false;

    private TimeSpan _lastFiredPrimary = TimeSpan.Zero;
    private TimeSpan _lastFiredSecondary = TimeSpan.Zero;

    private readonly TimeSpan _primaryCooldownPeriod = new(0,0,0,0,250);
    private readonly TimeSpan _secondaryCooldownPeriod = new(0,0,0,1,0);
    private readonly TimeSpan _weaponAnimationLength = new(0,0,0,0,150);
    private readonly TimeSpan _engineIdleAnimationLength = new(0,0,0,0,100);
    private readonly TimeSpan _engineBurstAnimationLength = new(0,0,0,0,250);

    public Vector2 Position = Vector2.Zero;
    public Vector2 Velocity = Vector2.Zero;
    public Vector2 Acceleration = Vector2.Zero;

    public float Angle = MathHelper.PiOver2;
    public float AngularVelocity = 0f;
    public float AngularAcceleration = 0f;

    /// <summary>
    /// The texture's angle relative to facing direction
    /// </summary>
    public readonly float TextureAngleOffset = MathHelper.PiOver2;

    public float EnginePower = 1000f;
    public float TurnPower = 10f;
    public Vector2 Drag => 0.01f*(Velocity.GetAbs()*Velocity); // 0.0008f*(Velocity.GetAbs()*Velocity);
    public float AngularDrag => 0.5f*(Math.Abs(AngularVelocity)*AngularVelocity) + 1f*AngularVelocity;
    public float ReverseAccelMultiplier = 0.3f;

    private int _health = 100;
    public int Health
    {
        get => _health;
        set => _health = MathHelper.Clamp(value, 0, _maxHealth);
    }

    private int _maxHealth = 100;
    public int MaxHealth
    {
        get => _maxHealth;
        set
        {
            int newValue = MathHelper.Min(1, value);
            _health = MathHelper.Max(MathHelper.Min(1, _health), (int)Math.Round((float)newValue/(float)_maxHealth));
            _maxHealth = newValue;
        }
    }

    public float HealthPercentage => Math.Clamp(100f * (float)_health / (float)_maxHealth, 0f, 100f);

    private Texture2D _baseTexture;
    private Texture2D _engineTexture;
    private AnimatedTilemap _weaponTextureTilemap;
    private AnimatedTilemap _engineEffectsTextureTilemap;

    private bool _currentMovementInput = false;

    /// <summary>
    /// The ship's texture height
    /// </summary>
    public readonly int TextureWidth = 48;

    /// <summary>
    /// The ship's texture width
    /// </summary>
    public readonly int TextureHeight = 48;

    public PlayerShip(Resources resources)
    {
        _resources = resources;

        // Register keybind actions
        _resources.Input.Keybinds[KeybindNames.GameplayFirePrimaryWeapon].StateChangedEvent   += HandleKeybind_GameplayFirePrimaryWeapon;
        _resources.Input.Keybinds[KeybindNames.GameplayFireSecondaryWeapon].StateChangedEvent += HandleKeybind_GameplayFireSecondaryWeapon;
        _resources.Input.Keybinds[KeybindNames.GameplayMoveForward].StateChangedEvent         += HandleKeybind_GameplayMoveForward;
        _resources.Input.Keybinds[KeybindNames.GameplayMoveBackward].StateChangedEvent        += HandleKeybind_GameplayMoveBackward;
        _resources.Input.Keybinds[KeybindNames.GameplayTurnLeft].StateChangedEvent            += HandleKeybind_GameplayTurnLeft;
        _resources.Input.Keybinds[KeybindNames.GameplayTurnRight].StateChangedEvent           += HandleKeybind_GameplayTurnRight;

        // Create tilemap animations
        _weaponTextureTilemap = new(_resources.MainShipWeapons[0], TextureWidth, TextureHeight);
        _weaponTextureTilemap.AddAnimation([(1,0),(2,0),(3,0),(4,0),(5,0),(6,0)], _weaponAnimationLength);

        _engineEffectsTextureTilemap = new(_resources.MainShipEngineEffects[3], TextureWidth, TextureHeight);
        _engineEffectsTextureTilemap.AddAnimation([(0,1),(1,1),(2,1),(3,1)], _engineBurstAnimationLength);
        _engineEffectsTextureTilemap.AddAnimation([(1,0),(2,0),(3,0)], _engineIdleAnimationLength, repeating: true);
    }

    public void HandleKeybind_GameplayFirePrimaryWeapon(object sender, EventArgs e)
    {
        if (_resources.CurrentGameState != GameState.Playing) return;
        if (sender is Keybind k) _firePrimaryKeyDown = k.IsCurrentlyPressed;
    }

    public void HandleKeybind_GameplayFireSecondaryWeapon(object sender, EventArgs e)
    {
        if (_resources.CurrentGameState != GameState.Playing) return;
        if (sender is Keybind k) _fireSecondaryKeyDown = k.IsCurrentlyPressed;
    }

    public void HandleKeybind_GameplayMoveForward(object sender, EventArgs e)
    {
        if (_resources.CurrentGameState != GameState.Playing) return;
        if (sender is Keybind k) _forwardKeyDown = k.IsCurrentlyPressed;
    }

    public void HandleKeybind_GameplayMoveBackward(object sender, EventArgs e)
    {
        if (_resources.CurrentGameState != GameState.Playing) return;
        if (sender is Keybind k) _backwardKeyDown = k.IsCurrentlyPressed;
    }

    public void HandleKeybind_GameplayTurnLeft(object sender, EventArgs e)
    {
        if (_resources.CurrentGameState != GameState.Playing) return;
        if (sender is Keybind k) _leftKeyDown = k.IsCurrentlyPressed;
    }

    public void HandleKeybind_GameplayTurnRight(object sender, EventArgs e)
    {
        if (_resources.CurrentGameState != GameState.Playing) return;
        if (sender is Keybind k) _rightKeyDown = k.IsCurrentlyPressed;
    }

    public void Update(GameTime gt)
    {
        if (_resources.CurrentGameState != GameState.Playing) return;
        if (gt.TotalGameTime < _resources.LastGameStateChangeTimestamp + new TimeSpan(0,0,1)) return;

        float dt = (float)gt.ElapsedGameTime.TotalSeconds;
        Vector2 direction = Vector2.Rotate(Vector2.UnitX, Angle);

        // Handle weapon input
        if (_firePrimaryKeyDown && gt.TotalGameTime > _lastFiredPrimary + _primaryCooldownPeriod)
        {
            _lastFiredPrimary = gt.TotalGameTime;
            _resources.PlayerShipShootSFX[10].Play();
            _weaponTextureTilemap.Animations[0].Play(gt);
        }

        // Handle movement input
        bool prevMovementInput = _currentMovementInput;
        _currentMovementInput = _forwardKeyDown != _backwardKeyDown || _leftKeyDown != _rightKeyDown;

        if (_currentMovementInput && !prevMovementInput)
        {
            _engineEffectsTextureTilemap.Animations[0].Play(gt);
            _engineEffectsTextureTilemap.Animations[1].Play(gt);
        }
        else if (prevMovementInput && !_currentMovementInput)
        {
            _engineEffectsTextureTilemap.Animations[0].Stop();
            _engineEffectsTextureTilemap.Animations[1].Stop();
        }

        if      (_forwardKeyDown && !_backwardKeyDown) Acceleration = direction * EnginePower;
        else if (!_forwardKeyDown && _backwardKeyDown) Acceleration = Vector2.Negate(direction) * (EnginePower * ReverseAccelMultiplier);
        else Acceleration = Vector2.Zero;

        if      (_leftKeyDown && !_rightKeyDown) AngularAcceleration = TurnPower;
        else if (!_leftKeyDown && _rightKeyDown) AngularAcceleration = -TurnPower;
        else AngularAcceleration = 0f;

        // Handle physics
        Position += Velocity * dt;
        Velocity += (Acceleration - Drag) * dt;
        Angle += AngularVelocity * dt;
        AngularVelocity += (AngularAcceleration - AngularDrag) * dt;
    }

    public void Draw(GameTime gt, SpriteBatch sb)
    {
        if (_resources.CurrentGameState != GameState.Playing) return;

        if      (HealthPercentage <= 75f) _baseTexture = _resources.MainShipBases[1];
        else if (HealthPercentage <= 50f) _baseTexture = _resources.MainShipBases[2];
        else if (HealthPercentage <= 25f) _baseTexture = _resources.MainShipBases[3];
        else                              _baseTexture = _resources.MainShipBases[0];

        _engineTexture = _resources.MainShipEngines[3];
        Rectangle textureFinalBounds = new(0,0,TextureWidth,TextureHeight);
        Rectangle weaponSourceTile = _weaponTextureTilemap.GetCurrentTile(gt);
        Rectangle engineEffectsSourceTile = _engineEffectsTextureTilemap.GetCurrentTile(gt);

        sb.Draw(_engineTexture, Position, _engineTexture.Bounds, Color.White, Angle + TextureAngleOffset, _engineTexture.Bounds.Center.ToVector2(), 10f*Vector2.One, SpriteEffects.None, Layers.GameplayShips);
        sb.Draw(_weaponTextureTilemap.Texture, Position, weaponSourceTile, Color.White, Angle + TextureAngleOffset, textureFinalBounds.Center.ToVector2(), 10*Vector2.One, SpriteEffects.None, Layers.GameplayShips);
        if (_engineEffectsTextureTilemap.IsAnimationPlaying(gt))
        {
            sb.Draw(_engineEffectsTextureTilemap.Texture, Position, engineEffectsSourceTile, Color.White, Angle + TextureAngleOffset, textureFinalBounds.Center.ToVector2(), 10f*Vector2.One, SpriteEffects.None, Layers.GameplayShips);
        }
        sb.Draw(_baseTexture, Position, _baseTexture.Bounds, Color.White, Angle + TextureAngleOffset, _baseTexture.Bounds.Center.ToVector2(), 10f*Vector2.One, SpriteEffects.None, Layers.GameplayShips);
    }
}
