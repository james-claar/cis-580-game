using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace cis_580_game;

public enum InputType
{
    MouseAndKeyboard = 0,
    Controller = 1
}

/// <summary>
/// Class responsible for handling all user input
/// </summary>
public class InputHandler
{
    private bool _newDataValid = false;
    private bool _oldDataValid = false;
    private ScaledRenderer _scaledRenderer;

    private KeyboardState _currentKeyboardState = new();
    public KeyboardState CurrentKeyboardState => _currentKeyboardState;

    private KeyboardState _priorKeyboardState = new();
    public KeyboardState PriorKeyboardState => _priorKeyboardState;

    private MouseState _currentMouseState = new();
    public MouseState CurrentMouseState => _currentMouseState;

    private MouseState _priorMouseState = new();
    public MouseState PriorMouseState => _priorMouseState;

    private GamePadState _currentGamePadState = new();
    public GamePadState CurrentGamePadState => _currentGamePadState;

    private GamePadState _priorGamePadState = new();
    public GamePadState PriorGamePadState => _priorGamePadState;

    private bool _keyboardChanged = false;
    public bool KeyboardChanged => _keyboardChanged;

    private bool _mouseChanged = false;
    public bool MouseChanged => _mouseChanged;

    private bool _gamepadChanged = false;
    public bool GamepadChanged => _gamepadChanged;

    private int _scrollWheelBuffer = 0;
    public int ScrollWheelBuffer => _scrollWheelBuffer;

    private int _horizontalScrollWheelBuffer = 0;
    public int HorizontalScrollWheelBuffer => _horizontalScrollWheelBuffer;

    private Vector2 _virtualMousePosition = Vector2.Zero;
    public Vector2 VirtualMousePosition => _virtualMousePosition;

    private bool _isMouseOnScreen = false;
    public bool IsMouseOnScreen => _isMouseOnScreen;

    private InputType _lastUsedInputType = InputType.MouseAndKeyboard;
    public InputType LastUsedInputType => _lastUsedInputType;

    public Keybinds Keybinds;

    public InputHandler(ScaledRenderer renderer)
    {
        _scaledRenderer = renderer;

        _keyboardChanged = false;
        _mouseChanged = false;
        _gamepadChanged = false;

        Keybinds = new(this);

        foreach (Keybind keybind in Keybinds.AllKeybinds) keybind.TriggerEvent += GetInputTypeFromEvent;
    }

    public void Update(GameTime gt)
    {
        // Update old state
        _priorKeyboardState = _currentKeyboardState;
        _priorMouseState = _currentMouseState;
        _priorGamePadState = _currentGamePadState;

        // Get new state
        _currentKeyboardState = Keyboard.GetState();
        _currentMouseState = Mouse.GetState();
        _currentGamePadState = GamePad.GetState(0);

        // Update validity flags
        if (!_oldDataValid || !_newDataValid)
        {
            _oldDataValid = _newDataValid;
            _newDataValid = true;
            return;
        }

        // Detect state changes
        _keyboardChanged = _currentKeyboardState != _priorKeyboardState;
        _mouseChanged = _currentMouseState != _priorMouseState;
        _gamepadChanged = _currentGamePadState != _priorGamePadState;
        _scrollWheelBuffer += _currentMouseState.ScrollWheelValue - _priorMouseState.ScrollWheelValue;
        _horizontalScrollWheelBuffer += _currentMouseState.HorizontalScrollWheelValue - _priorMouseState.HorizontalScrollWheelValue;
        if (_scaledRenderer is not null)
        {
            _virtualMousePosition = _scaledRenderer.GetVirtualPosFromScaled(new(_currentMouseState.X, _currentMouseState.Y));
            _isMouseOnScreen = ScaledRenderer.VirtualScreenRectangle.Contains(_virtualMousePosition);
        }

        // Determine which device was used most recently
        if (_keyboardChanged || _mouseChanged)
        {
            _lastUsedInputType = InputType.MouseAndKeyboard;
        }
        else if (_gamepadChanged)
        {
            _lastUsedInputType = InputType.Controller;
        }

        // Handle all actions
        Keybinds.Update(gt);

        // Bring buffers closer to 0
        _scrollWheelBuffer -= Math.Sign(_scrollWheelBuffer);
        _horizontalScrollWheelBuffer -= Math.Sign(_horizontalScrollWheelBuffer);
    }

    private void GetInputTypeFromEvent(object sender, KeybindEventArgs e)
    {
        _lastUsedInputType = e.ButtonType switch
        {
            ButtonType.Gamepad => InputType.Controller,
            _ => InputType.MouseAndKeyboard
        };
    }
}
