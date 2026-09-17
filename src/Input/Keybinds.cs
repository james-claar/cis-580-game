using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace cis_580_game;

public class Keybinds
{
    private InputHandler _input;

    /// <summary>
    /// Clicking a GUI button
    /// </summary>
    public Keybind GuiClickButton = new();

    /// <summary>
    /// Move back in the GUI
    /// </summary>
    public Keybind GuiBack = new();

    /// <summary>
    /// Move up in the GUI
    /// </summary>
    public Keybind GuiUp = new();

    /// <summary>
    /// Move down in the GUI
    /// </summary>
    public Keybind GuiDown = new();

    /// <summary>
    /// Move left in the GUI
    /// </summary>
    public Keybind GuiLeft = new();

    /// <summary>
    /// Move right in the GUI
    /// </summary>
    public Keybind GuiRight = new();

    /// <summary>
    /// Move forward
    /// </summary>
    public Keybind GameplayMoveForward = new();

    /// <summary>
    /// Move backward
    /// </summary>
    public Keybind GameplayMoveBackward = new();

    /// <summary>
    /// Turn left
    /// </summary>
    public Keybind GameplayTurnLeft = new();

    /// <summary>
    /// Turn right
    /// </summary>
    public Keybind GameplayTurnRight = new();

    /// <summary>
    /// Fire primary weapon
    /// </summary>
    public Keybind GameplayFirePrimaryWeapon = new();

    /// <summary>
    /// Fire secondary weapon
    /// </summary>
    public Keybind GameplayFireSecondaryWeapon = new();

    /// <summary>
    /// Interact with something
    /// </summary>
    public Keybind GameplayInteract = new();

    /// <summary>
    /// Open inventory
    /// </summary>
    public Keybind GameplayOpenInventory = new();

    public List<Keybind> AllKeybinds;

    public Keybinds(InputHandler input)
    {
        _input = input;

        // GUI triggers
        GuiClickButton.AddTriggerButtons(
            MouseButtons.LeftButton,
            Buttons.A
        );
        GuiBack.AddTriggerButtons(
            Keys.Escape,
            Buttons.B
        );
        GuiUp.AddTriggerButtons(
            Buttons.DPadUp,
            Buttons.LeftThumbstickUp
        );
        GuiDown.AddTriggerButtons(
            Buttons.DPadDown,
            Buttons.LeftThumbstickDown
        );
        GuiLeft.AddTriggerButtons(
            Buttons.DPadLeft,
            Buttons.LeftThumbstickLeft
        );
        GuiRight.AddTriggerButtons(
            Buttons.DPadRight,
            Buttons.LeftThumbstickRight
        );
        GameplayMoveForward.AddTriggerButtons(
            Keys.W,
            Keys.Up,
            Buttons.DPadUp
        );
        GameplayMoveBackward.AddTriggerButtons(
            Keys.S,
            Keys.Down,
            Buttons.DPadDown
        );
        GameplayTurnLeft.AddTriggerButtons(
            Keys.A,
            Keys.Left,
            Buttons.DPadLeft
        );
        GameplayTurnRight.AddTriggerButtons(
            Keys.D,
            Keys.Right,
            Buttons.DPadRight
        );
        GameplayFirePrimaryWeapon.AddTriggerButtons(
            MouseButtons.LeftButton,
            Buttons.RightTrigger
        );
        GameplayFireSecondaryWeapon.AddTriggerButtons(
            MouseButtons.MiddleButton,
            Buttons.RightStick
        );
        GameplayInteract.AddTriggerButtons(
            MouseButtons.RightButton,
            Keys.E,
            Buttons.RightShoulder
        );
        GameplayOpenInventory.AddTriggerButtons(
            Keys.Tab,
            Buttons.LeftShoulder
        );

        // Add keybinds to list
        AllKeybinds.AddRange(
            GuiClickButton,
            GuiBack,
            GuiUp,
            GuiDown,
            GuiLeft,
            GuiRight,
            GameplayMoveForward,
            GameplayMoveBackward,
            GameplayTurnLeft,
            GameplayTurnRight,
            GameplayFirePrimaryWeapon,
            GameplayFireSecondaryWeapon,
            GameplayInteract,
            GameplayOpenInventory
        );
    }

    public void Update(GameTime gt)
    {
        foreach (Keybind keybind in AllKeybinds)
        {
            keybind.Process(_input);
        }
    }
}
