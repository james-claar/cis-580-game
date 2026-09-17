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

    public KeyboardState CurrentKeyboardState { get; private set; }
    public KeyboardState PriorKeyboardState { get; private set; }
    public MouseState CurrentMouseState { get; private set; }
    public MouseState PriorMouseState { get; private set; }
    public GamePadState CurrentGamePadState { get; private set; }
    public GamePadState PriorGamePadState { get; private set; }

    public bool KeyboardChanged { get; private set; }
    public bool MouseChanged { get; private set; }
    public bool GamepadChanged { get; private set; }

    public int ScrollWheelBuffer { get; private set; }
    public int HorizontalScrollWheelBuffer { get; private set; }

    public InputType LastUsedInputType { get; private set; } = InputType.MouseAndKeyboard;

    public Keybinds Keybinds;

    public InputHandler()
    {
        KeyboardChanged = false;
        MouseChanged = false;
        GamepadChanged = false;

        Keybinds = new(this);
        foreach (Keybind keybind in Keybinds.AllKeybinds) keybind.TriggerEvent += GetInputTypeFromEvent;
    }

    public void Update(GameTime gt)
    {
        // Update old state
        PriorKeyboardState = CurrentKeyboardState;
        PriorMouseState = CurrentMouseState;
        PriorGamePadState = CurrentGamePadState;

        // Update validity flags
        if (!_oldDataValid || !_newDataValid)
        {
            _oldDataValid = _newDataValid;
            _newDataValid = true;
            return;
        }

        // Get new state
        CurrentKeyboardState = Keyboard.GetState();
        CurrentMouseState = Mouse.GetState();
        CurrentGamePadState = GamePad.GetState(0);

        // Detect state changes
        KeyboardChanged = CurrentKeyboardState != PriorKeyboardState;
        MouseChanged = CurrentMouseState != PriorMouseState;
        GamepadChanged = CurrentGamePadState != PriorGamePadState;
        ScrollWheelBuffer += CurrentMouseState.ScrollWheelValue - PriorMouseState.ScrollWheelValue;
        HorizontalScrollWheelBuffer += CurrentMouseState.HorizontalScrollWheelValue - PriorMouseState.HorizontalScrollWheelValue;

        // Determine which device was used most recently
        if (KeyboardChanged || MouseChanged)
        {
            LastUsedInputType = InputType.MouseAndKeyboard;
        }
        else if (GamepadChanged)
        {
            LastUsedInputType = InputType.Controller;
        }

        // Handle all actions
        Keybinds.Update(gt);

        // Bring buffers closer to 0
        ScrollWheelBuffer -= Math.Sign(ScrollWheelBuffer);
        HorizontalScrollWheelBuffer -= Math.Sign(HorizontalScrollWheelBuffer);
    }

    private void GetInputTypeFromEvent(object sender, KeybindEventArgs e)
    {
        LastUsedInputType = e.ButtonType switch
        {
            ButtonType.Gamepad => InputType.Controller,
            _ => InputType.MouseAndKeyboard
        };
    }
}
