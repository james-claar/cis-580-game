using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace cis_580_game;

public enum InputType
{
    MouseAndKeyboard,
    Controller
}

/// <summary>
/// Class responsible for handling all user input
/// </summary>
public class InputHandler
{
    private Game _game;

    private bool _isWindowInFocus = true;

    /// <summary>
    /// Whether the window is in focus
    /// </summary>
    public bool IsWindowInFocus => _isWindowInFocus;

    private bool _newDataValid = false;
    private bool _oldDataValid = false;
    private ScaledRenderer _scaledRenderer;

    private KeyboardState _currentKeyboardState = new();
    private KeyboardState _priorKeyboardState = new();
    private MouseState _currentMouseState = new();
    private MouseState _priorMouseState = new();
    private GamePadState _currentGamePadState = new();
    private GamePadState _priorGamePadState = new();

    private HashSet<GenericButton> _currentPressedButtons = new();

    /// <summary>
    /// Set of buttons that are currently pressed
    /// </summary>
    public HashSet<GenericButton> CurrentPressedButtons => _currentPressedButtons;

    private HashSet<GenericButton> _priorPressedButtons = new HashSet<GenericButton>();

    private HashSet<GenericButton> _newlyPressedButtons = new();

    /// <summary>
    /// Set of buttons that have been pressed since the last update
    /// </summary>
    public HashSet<GenericButton> NewlyPressedButtons => _newlyPressedButtons;

    private HashSet<GenericButton> _newlyReleasedButtons = new HashSet<GenericButton>();

    /// <summary>
    /// Buttons that have been newly released
    /// </summary>
    public HashSet<GenericButton> NewlyReleasedButtons => _newlyReleasedButtons;

    private Dictionary<GenericButton, PressableGenericButton> _pressableButtons = [];
    public Dictionary<GenericButton, PressableGenericButton> PressableButtons => _pressableButtons;

    private int _scrollWheelBuffer = 0;
    private int _horizontalScrollWheelBuffer = 0;

    private Vector2 _virtualMousePosition = Vector2.Zero;

    /// <summary>
    /// The mouse position on the virtual screen
    /// </summary>
    public Vector2 VirtualMousePosition => _virtualMousePosition;

    private bool _isMouseOnScreen = false;

    /// <summary>
    /// Whether the mouse is on the gameplay screen
    /// </summary>
    public bool IsMouseOnScreen => _isMouseOnScreen;

    private InputType _lastUsedInputType = InputType.MouseAndKeyboard;

    /// <summary>
    /// The last used input type
    /// </summary>
    public InputType LastUsedInputType => _lastUsedInputType;

    /// <summary>
    /// The keybind manager
    /// </summary>
    public KeybindManager Keybinds;

    public InputHandler(Game game, ScaledRenderer renderer)
    {
        _game = game;
        _scaledRenderer = renderer;

        _pressableButtons = new();
        foreach (GenericButton button in GenericButton.GetAllPossible())
        {
            _pressableButtons.Add(button, new PressableGenericButton(button));
        }

        Keybinds = new(this);

        _game.Activated += OnRegainFocus;
        _game.Deactivated += OnLoseFocus;
    }

    public void Update(GameTime gt)
    {
        // Update old state
        _priorKeyboardState  = _currentKeyboardState;
        _priorMouseState     = _currentMouseState;
        _priorGamePadState   = _currentGamePadState;
        _priorPressedButtons = [.. _currentPressedButtons];

        // Get new state
        _currentPressedButtons.Clear();
        _newlyPressedButtons.Clear();
        _newlyReleasedButtons.Clear();
        if (_isWindowInFocus)
        {
            // Handle all button changes
            _currentKeyboardState = Keyboard.GetState();
            _currentMouseState    = Mouse.GetState();
            _currentGamePadState  = GamePad.GetState(0);
            foreach (GenericButton button in _currentKeyboardState.GetPressedKeys())   _currentPressedButtons.Add(button);
            foreach (GenericButton button in _currentMouseState.GetPressedButtons())   _currentPressedButtons.Add(button);
            foreach (GenericButton button in _currentGamePadState.GetPressedButtons()) _currentPressedButtons.Add(button);
            _newlyPressedButtons  = _currentPressedButtons.Except(_priorPressedButtons).ToHashSet();
            _newlyReleasedButtons = _priorPressedButtons.Except(_currentPressedButtons).ToHashSet();

            // Handle all scroll wheel changes
            _scrollWheelBuffer += _currentMouseState.ScrollWheelValue - _priorMouseState.ScrollWheelValue;
            _horizontalScrollWheelBuffer += _currentMouseState.HorizontalScrollWheelValue - _priorMouseState.HorizontalScrollWheelValue;
            if (_scrollWheelBuffer > 0) _newlyPressedButtons.Add(ScrollWheelChanges.Up);
            if (_scrollWheelBuffer < 0) _newlyPressedButtons.Add(ScrollWheelChanges.Down);
            if (_horizontalScrollWheelBuffer > 0) _newlyPressedButtons.Add(ScrollWheelChanges.Right);
            if (_horizontalScrollWheelBuffer < 0) _newlyPressedButtons.Add(ScrollWheelChanges.Left);
            _scrollWheelBuffer -= Math.Sign(_scrollWheelBuffer);
            _horizontalScrollWheelBuffer -= Math.Sign(_horizontalScrollWheelBuffer);

            // Get mouse position
            _virtualMousePosition = _scaledRenderer is not null ? _scaledRenderer.GetVirtualPosFromScaled(new(_currentMouseState.X, _currentMouseState.Y)) : Vector2.Zero;
            _isMouseOnScreen = _scaledRenderer is not null ? ScaledRenderer.VirtualScreenRectangle.Contains(_virtualMousePosition) : false;
        }
        else
        {
            // Clear any stuck keys on lost window focus
            _currentKeyboardState = new();
            _currentMouseState    = new();
            _currentGamePadState  = new();
            _scrollWheelBuffer = 0;
            _horizontalScrollWheelBuffer = 0;
            _virtualMousePosition = Vector2.Zero;
            _isMouseOnScreen = false;
        }

        // Update validity flags and exit if invalid
        if (!_oldDataValid || !_newDataValid)
        {
            _oldDataValid = _newDataValid;
            _newDataValid = true;
            return;
        }

        // Detect state changes
        bool keyboardChanged = _currentKeyboardState != _priorKeyboardState;
        bool mouseChanged = _currentMouseState != _priorMouseState;
        bool gamepadChanged = _currentGamePadState != _priorGamePadState;

        // Determine which input was used most recently
        if (keyboardChanged || mouseChanged)
        {
            _lastUsedInputType = InputType.MouseAndKeyboard;
        }
        else if (gamepadChanged)
        {
            _lastUsedInputType = InputType.Controller;
        }

        // Handle all actions
        foreach (PressableGenericButton button in _pressableButtons.Values) button.CheckInput(this);
        Keybinds.Update(gt);
    }

    /// <summary>
    /// Triggered when the window loses focus
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnLoseFocus(object sender, EventArgs e)
    {
        _isWindowInFocus = false;
    }

    /// <summary>
    /// Triggered when the window regains focus
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnRegainFocus(object sender, EventArgs e)
    {
        _isWindowInFocus = true;
    }
}
