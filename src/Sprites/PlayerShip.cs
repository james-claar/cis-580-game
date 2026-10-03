
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
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
    private readonly TimeSpan _weaponAnimationLength = new(0,0,0,0,250);

    public Vector2 Position = ScaledRenderer.VirtualScreenCenter;
    public Vector2 Velocity = Vector2.Zero;
    public Vector2 Acceleration = Vector2.Zero;

    public float Angle = 0f;
    public float AngularVelocity = 0f;
    public float AngularAcceleration = 0f;

    public float EnginePower = 1000f;
    public float TurnPower = 10f;
    public Vector2 Drag => 0.0008f*(Velocity.GetAbs()*Velocity);
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
    private Texture2D _weaponTextureTilemap;

    public PlayerShip(Resources resources)
    {
        _resources = resources;

        _resources.Input.Keybinds[KeybindNames.GameplayFirePrimaryWeapon].StateChangedEvent   += HandleKeybind_GameplayFirePrimaryWeapon;
        _resources.Input.Keybinds[KeybindNames.GameplayFireSecondaryWeapon].StateChangedEvent += HandleKeybind_GameplayFireSecondaryWeapon;
        _resources.Input.Keybinds[KeybindNames.GameplayMoveForward].StateChangedEvent         += HandleKeybind_GameplayMoveForward;
        _resources.Input.Keybinds[KeybindNames.GameplayMoveBackward].StateChangedEvent        += HandleKeybind_GameplayMoveBackward;
        _resources.Input.Keybinds[KeybindNames.GameplayTurnLeft].StateChangedEvent            += HandleKeybind_GameplayTurnLeft;
        _resources.Input.Keybinds[KeybindNames.GameplayTurnRight].StateChangedEvent           += HandleKeybind_GameplayTurnRight;
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
        Vector2 direction = Vector2.Rotate(-Vector2.UnitY, Angle);

        // Handle weapon input
        if (_firePrimaryKeyDown && gt.TotalGameTime > _lastFiredPrimary + _primaryCooldownPeriod)
        {
            _lastFiredPrimary = gt.TotalGameTime;
            _resources.PlayerShipShootSFX[10].Play();
        }

        // Handle movement input
        if      (_forwardKeyDown && !_backwardKeyDown) Acceleration = direction * EnginePower;
        else if (!_forwardKeyDown && _backwardKeyDown) Acceleration = Vector2.Negate(direction) * (EnginePower * ReverseAccelMultiplier);
        else Acceleration = Vector2.Zero;

        if      (_leftKeyDown && !_rightKeyDown) AngularAcceleration = -TurnPower;
        else if (!_leftKeyDown && _rightKeyDown) AngularAcceleration = TurnPower;
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

        _engineTexture = _resources.MainShipEngines[0];

        _weaponTextureTilemap = _resources.MainShipWeapons[1];
        Rectangle weaponTextureTilemapBounds = new(0,0,48,48);
        Rectangle weaponTextureFinalBounds = new(0,0,48,48);
        if (gt.TotalGameTime < _lastFiredPrimary + _weaponAnimationLength)
        {
            int numAnimatedFrames = (_weaponTextureTilemap.Bounds.Width / 48) - 1;
            int animationFrameIndex = MathHelper.Clamp(1 + (int)Math.Floor((gt.TotalGameTime - _lastFiredPrimary) / _weaponAnimationLength), 0, numAnimatedFrames-1);
            weaponTextureTilemapBounds.X = animationFrameIndex * 48;
        }

        _resources.ScaledRenderer.Draw(_engineTexture, Position, _engineTexture.Bounds, Color.White, Angle, _engineTexture.Bounds.Center.ToVector2(), 10f*Vector2.One, SpriteEffects.None, Layers.GameplayShips);
        _resources.ScaledRenderer.Draw(_weaponTextureTilemap, Position, weaponTextureTilemapBounds, Color.White, Angle, weaponTextureFinalBounds.Center.ToVector2(), 10*Vector2.One, SpriteEffects.None, Layers.GameplayShips);
        _resources.ScaledRenderer.Draw(_baseTexture, Position, _baseTexture.Bounds, Color.White, Angle, _baseTexture.Bounds.Center.ToVector2(), 10f*Vector2.One, SpriteEffects.None, Layers.GameplayShips);
    }
}
