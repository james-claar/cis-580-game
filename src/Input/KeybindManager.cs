using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace cis_580_game;

public enum KeybindNames
{
    // DUMMY KEYBINDS
    Dummy, // Dummy keybind


    // GUI KEYBINDS
    GuiClickButton, // Clicking a GUI button
    GuiBack, // Move back in the GUI
    GuiUp, // Move up in the GUI
    GuiDown, // Move down in the GUI
    GuiLeft, // Move left in the GUI
    GuiRight, // Move right in the GUI


    // GAMEPLAY KEYBINDS
    GameplayMoveForward, // Move forward
    GameplayMoveBackward, // Move backward
    GameplayTurnLeft, // Turn left
    GameplayTurnRight, // Turn right
    GameplayZoomIn, // Zoom in camera
    GameplayZoomOut, // Zoom out camera
    GameplayFirePrimaryWeapon, // Fire primary weapon
    GameplayFireSecondaryWeapon, // Fire secondary weapon
    GameplayInteract, // Interact with something
    GameplayOpenInventory // Open inventory
}

public class KeybindManager
{
    private Resources _resources;

    private Dictionary<KeybindNames, Keybind> _keybinds = [];

    public Keybind this[KeybindNames index]
    {
        get
        {
            bool success = _keybinds.TryGetValue(index, out Keybind keybind);
            if (success) return keybind;
            else
            {
                Console.WriteLine("Tried to read undefined keybind for " + Enum.GetName(index));
                return new Keybind(_resources) {IsDummyKeybind=true};
            }
        }
    }

    public KeybindManager(Resources resources, Dictionary<GenericButton, PressableGenericButton> pressableButtons)
    {
        _resources = resources;
        foreach (KeybindNames name in Enum.GetValues<KeybindNames>()) _keybinds.Add(name, new Keybind(_resources, pressableButtons));
        this[KeybindNames.Dummy].IsDummyKeybind = true;
        SetDefaultKeybinds();
    }

    public void LoadContent(ContentManager content)
    {
        
    }

    public void SetDefaultKeybinds()
    {
        // GUI triggers
        this[KeybindNames.GuiClickButton].AddTriggerButtons(
            MouseButtons.LeftButton,
            Buttons.A
        );
        this[KeybindNames.GuiBack].AddTriggerButtons(
            Keys.Escape,
            Buttons.B
        );
        this[KeybindNames.GuiUp].AddTriggerButtons(
            Buttons.DPadUp,
            Buttons.LeftThumbstickUp
        );
        this[KeybindNames.GuiDown].AddTriggerButtons(
            Buttons.DPadDown,
            Buttons.LeftThumbstickDown
        );
        this[KeybindNames.GuiLeft].AddTriggerButtons(
            Buttons.DPadLeft,
            Buttons.LeftThumbstickLeft
        );
        this[KeybindNames.GuiRight].AddTriggerButtons(
            Buttons.DPadRight,
            Buttons.LeftThumbstickRight
        );
        this[KeybindNames.GameplayMoveForward].AddTriggerButtons(
            Keys.W,
            Keys.Up,
            Buttons.LeftThumbstickUp
        );
        this[KeybindNames.GameplayMoveBackward].AddTriggerButtons(
            Keys.S,
            Keys.Down,
            Buttons.LeftThumbstickDown
        );
        this[KeybindNames.GameplayTurnLeft].AddTriggerButtons(
            Keys.A,
            Keys.Left,
            Buttons.LeftThumbstickLeft
        );
        this[KeybindNames.GameplayTurnRight].AddTriggerButtons(
            Keys.D,
            Keys.Right,
            Buttons.LeftThumbstickRight
        );
        this[KeybindNames.GameplayZoomIn].AddTriggerButtons(
            ScrollWheelChanges.Up,
            Keys.OemPlus,
            Buttons.DPadUp
        );
        this[KeybindNames.GameplayZoomOut].AddTriggerButtons(
            ScrollWheelChanges.Down,
            Keys.OemMinus,
            Buttons.DPadDown
        );
        this[KeybindNames.GameplayFirePrimaryWeapon].AddTriggerButtons(
            MouseButtons.LeftButton,
            Keys.Space,
            Buttons.RightTrigger
        );
        this[KeybindNames.GameplayFireSecondaryWeapon].AddTriggerButtons(
            MouseButtons.MiddleButton,
            Buttons.RightStick
        );
        this[KeybindNames.GameplayInteract].AddTriggerButtons(
            MouseButtons.RightButton,
            Keys.E,
            Buttons.RightShoulder
        );
        this[KeybindNames.GameplayOpenInventory].AddTriggerButtons(
            Keys.Tab,
            Buttons.LeftShoulder
        );
    }

    public void Update(GameTime gt)
    {
        foreach (Keybind keybind in _keybinds.Values) keybind.Update(gt);
    }
}
