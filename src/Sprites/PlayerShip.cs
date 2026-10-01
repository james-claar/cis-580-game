
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace cis_580_game;

public class PlayerShip
{
    private Resources _resources;

    private bool _forwardKeyDown = false;
    private bool _backwardKeyDown = false;
    private bool _leftKeyDown = false;
    private bool _rightKeyDown = false;

    public Vector2 Position = ScaledRenderer.VirtualScreenCenter;
    public Vector2 Velocity = Vector2.Zero;
    public Vector2 Acceleration = Vector2.Zero;

    public float Angle = 0f;
    public float AngularVelocity = 0f;
    public float AngularAcceleration = 0f;

    public float EnginePower = 500f;
    public float TurnPower = 10f;
    public float Drag = 0.001f;
    public float AngularDrag = 0.5f;

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
        if (sender is Keybind k)
        {
            
        }
    }

    public void HandleKeybind_GameplayFireSecondaryWeapon(object sender, EventArgs e)
    {
        if (sender is Keybind k)
        {
            
        }
    }

    public void HandleKeybind_GameplayMoveForward(object sender, EventArgs e)
    {
        if (sender is Keybind k) _forwardKeyDown = k.IsCurrentlyPressed;
    }

    public void HandleKeybind_GameplayMoveBackward(object sender, EventArgs e)
    {
        if (sender is Keybind k) _backwardKeyDown = k.IsCurrentlyPressed;
    }

    public void HandleKeybind_GameplayTurnLeft(object sender, EventArgs e)
    {
        if (sender is Keybind k) _leftKeyDown = k.IsCurrentlyPressed;
    }

    public void HandleKeybind_GameplayTurnRight(object sender, EventArgs e)
    {
        if (sender is Keybind k) _rightKeyDown = k.IsCurrentlyPressed;
    }

    public void Update(GameTime gt)
    {
        if (_resources.CurrentGameState != GameState.Playing) return;

        float dt = (float)gt.ElapsedGameTime.TotalSeconds;
        Vector2 direction = Vector2.Rotate(-Vector2.UnitY, Angle);

        // Handle input
        if      (_forwardKeyDown && !_backwardKeyDown) Acceleration = direction * EnginePower;
        else if (!_forwardKeyDown && _backwardKeyDown) Acceleration = Vector2.Negate(direction) * EnginePower;
        else Acceleration = Vector2.Zero;

        float turnCheatVelocityFactor = 0;
        if      (_leftKeyDown && !_rightKeyDown) AngularAcceleration = -TurnPower;
        else if (!_leftKeyDown && _rightKeyDown) AngularAcceleration = TurnPower;
        else AngularAcceleration = 0f;

        // Handle physics
        Position += Velocity * dt;
        Velocity += (Acceleration - Drag*(Velocity.Length()*Velocity)) * dt;
        Angle += (AngularVelocity + turnCheatVelocityFactor) * dt;
        AngularVelocity += (AngularAcceleration - AngularDrag*(Math.Abs(AngularVelocity)*AngularVelocity)) * dt;
    }

    public void Draw(GameTime gt, SpriteBatch sb)
    {
        if (_resources.CurrentGameState != GameState.Playing) return;

        if      (HealthPercentage <= 75f) _baseTexture = _resources.MainShipBases[1];
        else if (HealthPercentage <= 50f) _baseTexture = _resources.MainShipBases[2];
        else if (HealthPercentage <= 25f) _baseTexture = _resources.MainShipBases[3];
        else                              _baseTexture = _resources.MainShipBases[0];
        _engineTexture = _resources.MainShipEngines[0];
        _resources.ScaledRenderer.Draw(_engineTexture, Position, _engineTexture.Bounds, Color.White, Angle, _engineTexture.Bounds.Center.ToVector2(), 10f*Vector2.One, SpriteEffects.None, Layers.GameplayShips);
        _resources.ScaledRenderer.Draw(_baseTexture, Position, _baseTexture.Bounds, Color.White, Angle, _baseTexture.Bounds.Center.ToVector2(), 10f*Vector2.One, SpriteEffects.None, Layers.GameplayShips);
    }
}
