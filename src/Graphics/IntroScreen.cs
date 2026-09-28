/*
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace cis_580_game;

public enum IntroState
{
    Standby,
    Booting,
    Assessment
}

public class IntroScreen
{
    private IntroState _state = IntroState.Standby;

    public readonly string BootTitle = "Booting BaseOS v21.05";

    /// <summary>
    /// Text for the boot section of the intro
    /// </summary>
    public readonly List<string> PostText = [
        "DIAGNOSTICS: Running Power-On Self Test...",
        "Hull integrity: PASS",
        "Power core: PASS",
        "Engine: PASS",
        "Field Resource Processor: PASS",
        "Targeting Computer: FAIL [Error 1: Device did not respond]",
        "Weapons System: FAIL [Error 7: No weapons installed]",
        "Ship AI Status: UNKNOWN [Warning: No response yet]"
    ];

    public readonly string AssessmentTitle = "Situation Assessment";

    /// <summary>
    /// Text for the assessment portion of the intro
    /// </summary>
    public readonly List<string> AssessmentText = [
        "- Targeting computer fried by precise EMP blast",
        "- Nearby area appears safe and has compatible resources",
        "- Waiting for input from Ship AI..."
    ];

    public IntroScreen()
    {
        
    }

    public void LoadContent()
    {
        
    }

    public void Draw(GameTime gt)
    {
        switch (_state)
        {
            
            default:
        }
    }
}
*/