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
    private Resources _resources;

    private bool _isWindowInFocus = true;
    private bool _wasLastUpdateWindowInFocus = true;

    /// <summary>
    /// Whether the window is in focus
    /// </summary>
    public bool IsWindowInFocus => _isWindowInFocus;

    private bool _newDataValid = false;
    private bool _oldDataValid = false;

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

    /// <summary>
    /// List of buttons that can be pressed
    /// </summary>
    public Dictionary<GenericButton, PressableGenericButton> PressableButtons => _pressableButtons;

    /// <summary>
    /// How many virtual 
    /// </summary>
    public float ScrollWheelSensitivity = 0.01f;

    private int _deltaScrollWheel = 0;

    /// <summary>
    /// Change in the scroll wheel since the last update
    /// </summary>
    public int DeltaScrollWheel => _deltaScrollWheel;

    private int _deltaHorizontalScrollWheel = 0;

    /// <summary>
    /// Change in the horizontal scroll wheel since the last update
    /// </summary>
    public int DeltaHorizontalScrollWheel => _deltaHorizontalScrollWheel;

    private Vector2 _virtualMousePosition = Vector2.Zero;

    private Vector2 _gameWorldMousePosition = Vector2.Zero;

    /// <summary>
    /// The mouse position on the virtual screen
    /// </summary>
    public Vector2 VirtualMousePosition => _virtualMousePosition;

    /// <summary>
    /// The mouse position in the game world
    /// </summary>
    public Vector2 GameWorldMousePosition => _gameWorldMousePosition;

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

    public InputHandler(Resources resources)
    {
        _resources = resources;

        _pressableButtons = new();
        foreach (GenericButton button in GenericButton.GetAllPossible())
        {
            _pressableButtons.Add(button, new PressableGenericButton(button));
        }

        Keybinds = new(_resources, _pressableButtons);

        _resources.Game.Activated += OnRegainFocus;
        _resources.Game.Deactivated += OnLoseFocus;
    }

    public void LoadContent(ContentManager content)
    {
        Keybinds.LoadContent(content);
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
        _deltaScrollWheel = 0;
        _deltaHorizontalScrollWheel = 0;
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
            _deltaScrollWheel = _wasLastUpdateWindowInFocus ? _currentMouseState.ScrollWheelValue - _priorMouseState.ScrollWheelValue : 0;
            _deltaHorizontalScrollWheel = _wasLastUpdateWindowInFocus ? _currentMouseState.HorizontalScrollWheelValue - _priorMouseState.HorizontalScrollWheelValue : 0;
            if (_deltaScrollWheel > 0) _newlyPressedButtons.Add(ScrollWheelChanges.Up);
            if (_deltaScrollWheel < 0) _newlyPressedButtons.Add(ScrollWheelChanges.Down);
            if (_deltaHorizontalScrollWheel > 0) _newlyPressedButtons.Add(ScrollWheelChanges.Right);
            if (_deltaHorizontalScrollWheel < 0) _newlyPressedButtons.Add(ScrollWheelChanges.Left);

            // Get mouse position
            UpdateMousePosition();
            _isMouseOnScreen = VirtualScreen.Rectangle.Contains(_virtualMousePosition);

            _wasLastUpdateWindowInFocus = true;
        }
        else
        {
            // Clear any stuck keys on lost window focus
            _currentKeyboardState = new();
            _currentMouseState    = new();
            _currentGamePadState  = new();
            _deltaScrollWheel = 0;
            _deltaHorizontalScrollWheel = 0;
            _virtualMousePosition = Vector2.Zero;
            _isMouseOnScreen = false;

            _wasLastUpdateWindowInFocus = false;
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

    public void UpdateMousePosition()
    {
        if (!_isWindowInFocus) return;

        _virtualMousePosition = _resources.VirtualScreen.GetVirtualPosFromScaled(new(_currentMouseState.X, _currentMouseState.Y));
        _gameWorldMousePosition = _resources.Camera.GetGameWorldPosFromRealWindowPos(new(_currentMouseState.X, _currentMouseState.Y));
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
