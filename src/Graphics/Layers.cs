

namespace cis_580_game;

/// <summary>
/// Layers to draw to
/// </summary>
public readonly struct Layers
{
    // Front Layer
    public static readonly float ForcedFront = 0f;

    // GUI Layers
    public static readonly float GuiObjectsForeground = 0.01f;
    public static readonly float GuiBackground = 0.02f;
    public static readonly float GuiObjectsBackground = 0.03f;

    // Gameplay layers
    public static readonly float GameplayBullets = 0.11f;
    public static readonly float GameplayShips = 0.12f;
    public static readonly float GameplayStructures = 0.13f;

    // Background layers
    public static readonly float BackgroundObjects = 0.21f;
    public static readonly float BackgroundStars = 0.22f;

    // Back layer
    public static readonly float ForcedBack = 1f;
}
