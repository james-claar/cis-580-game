
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace cis_580_game;

public class PlayerShip : Sprite
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

    /// <summary>
    /// The texture's angle relative to facing direction
    /// </summary>
    public static readonly float TextureAngleOffset = MathHelper.PiOver2;

    /// <summary>
    /// Ship's size multiplier
    /// </summary>
    public static readonly float TextureSizeMultiplier = 10f;

    public float EnginePower = 4000f;
    public float TurnPower = 5f;
    public Vector2 Drag => 0.0008f*(Velocity.AbsPos*Velocity) + 0.5f*Velocity;
    public float AngularDrag => 0.5f*(Math.Abs(AngularVelocity)*AngularVelocity) + 1f*AngularVelocity;
    public float ReverseAccelMultiplier = 0.3f;

    private Rectangle _baseTile;
    private int _engineIndex;
    private AnimatedTile _weaponTile;
    private AnimatedTile _engineEffectsTile;

    private bool _currentMovementInput = false;

    public PlayerShip()
    {

    }

    public PlayerShip(Resources resources)
    {
        Angle = MathHelper.PiOver2;

        Initialize(resources);
    }

    public void Initialize(Resources resources)
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
        _engineIndex = 0;
        _weaponTile = new(_resources.MainShipWeaponTilemaps[0], [false], [_weaponAnimationLength]);
        _engineEffectsTile = new(_resources.MainShipEngineEffectTilemaps[_engineIndex], [false, true], [_engineBurstAnimationLength, _engineIdleAnimationLength]);
    }

    public void HandleKeybind_GameplayFirePrimaryWeapon(object sender, KeybindEventArgs e)
    {
        if (_resources.CurrentGameState != GameState.Playing) return;
        if (sender is Keybind k) _firePrimaryKeyDown = k.IsCurrentlyPressed;
    }

    public void HandleKeybind_GameplayFireSecondaryWeapon(object sender, KeybindEventArgs e)
    {
        if (_resources.CurrentGameState != GameState.Playing) return;
        if (sender is Keybind k) _fireSecondaryKeyDown = k.IsCurrentlyPressed;
    }

    public void HandleKeybind_GameplayMoveForward(object sender, KeybindEventArgs e)
    {
        if (_resources.CurrentGameState != GameState.Playing) return;
        if (sender is Keybind k) _forwardKeyDown = k.IsCurrentlyPressed;
    }

    public void HandleKeybind_GameplayMoveBackward(object sender, KeybindEventArgs e)
    {
        if (_resources.CurrentGameState != GameState.Playing) return;
        if (sender is Keybind k) _backwardKeyDown = k.IsCurrentlyPressed;
    }

    public void HandleKeybind_GameplayTurnLeft(object sender, KeybindEventArgs e)
    {
        if (_resources.CurrentGameState != GameState.Playing) return;
        if (sender is Keybind k) _leftKeyDown = k.IsCurrentlyPressed;
    }

    public void HandleKeybind_GameplayTurnRight(object sender, KeybindEventArgs e)
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
            _weaponTile.Play(0, gt);
        }

        // Handle movement input
        bool prevMovementInput = _currentMovementInput;
        _currentMovementInput = _forwardKeyDown != _backwardKeyDown || _leftKeyDown != _rightKeyDown;

        if (_currentMovementInput && !prevMovementInput)
        {
            _engineEffectsTile.Play(0, gt);
            _engineEffectsTile.Play(1, gt);
        }
        else if (prevMovementInput && !_currentMovementInput)
        {
            _engineEffectsTile.StopAll();
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
        Velocity = Velocity.GetRotatedOntoLine(Angle);
    }

    public void Draw(GameTime gt, SpriteBatch sb)
    {
        if (_resources.CurrentGameState != GameState.Playing) return;

        // Update base tile based on health
        if      (HealthPercentage <= 75f) _baseTile = _resources.MainShipBase[1];
        else if (HealthPercentage <= 50f) _baseTile = _resources.MainShipBase[2];
        else if (HealthPercentage <= 25f) _baseTile = _resources.MainShipBase[3];
        else                              _baseTile = _resources.MainShipBase[0];

        _weaponTile.Draw(sb, gt, Position, Color.White, TextureAngleOffset + Position.AnglePointingAt(_resources.Input.GameWorldMousePosition), TextureSizeMultiplier, SpriteEffects.None, Layers.GameplayShips);
        _engineEffectsTile.Draw(sb, gt, Position, Color.White, Angle + TextureAngleOffset, TextureSizeMultiplier, SpriteEffects.None, Layers.GameplayShips);
        sb.Draw(_resources.MainShipEngines.Texture, Position, _resources.MainShipEngines[_engineIndex], Color.White, Angle + TextureAngleOffset, _resources.MainShipEngines.TileOrigin, TextureSizeMultiplier*Vector2.One, SpriteEffects.None, Layers.GameplayShips);
        sb.Draw(_resources.MainShipBase.Texture, Position, _baseTile, Color.White, Angle + TextureAngleOffset, _resources.MainShipBase.TileOrigin, TextureSizeMultiplier*Vector2.One, SpriteEffects.None, Layers.GameplayShips);
    }
}
