using Microsoft.Xna.Framework;

namespace cis_580_game;

public class CameraRenderer
{
    private ScaledRenderer _scaledRenderer;

    /// <summary>
    /// Position of the camera's center in the virtual world space
    /// </summary>
    public Vector2 CameraPosition = ScaledRenderer.VirtualScreenCenter;

    /// <summary>
    /// Rotation of the camera
    /// </summary>
    public float CameraAngle = 0f;

    /// <summary>
    /// Camera zoom. At zoom == 1f, width on screen in px = zoom*(object width in world space units).
    /// </summary>
    public float CameraZoom = 1f;

    public CameraRenderer(ScaledRenderer renderer)
    {
        _scaledRenderer = renderer;
    }

    public void Update(GameTime gt)
    {
        
    }
}