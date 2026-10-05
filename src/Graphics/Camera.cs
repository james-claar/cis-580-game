using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace cis_580_game;

public class Camera
{
    private Resources _resources;

    private Vector2 _position = VirtualScreen.Center;

    /// <summary>
    /// Position of the camera's center in the virtual world space
    /// </summary>
    public Vector2 Position => _position;

    private float _angle = 0f;
    /// <summary>
    /// Rotation of the camera
    /// </summary>

    public float Angle => _angle;

    private float _zoom = 1f;

    /// <summary>
    /// Camera zoom. At zoom == 1f, width on screen in px = zoom*(object width in world space units).
    /// </summary>
    public float Zoom => _zoom;

    private Matrix _transformationMatrix;

    /// <summary>
    /// Transformation matrix to map game world points to window points
    /// </summary>
    public Matrix TransformationMatrix => _transformationMatrix;

    private Matrix _inverseTransformationMatrix;

    /// <summary>
    /// Reverse transformation matrix to map real points to game world ones
    /// </summary>
    public Matrix InverseTransformationMatrix => _inverseTransformationMatrix;

    private Matrix _virtualScreenTransformationMatrix;

    /// <summary>
    /// Transformation matrix to map game world points to ones on the virtual screen
    /// </summary>
    public Matrix VirtualScreenTransormationMatrix => _virtualScreenTransformationMatrix;

    private Matrix _inverseVirtualScreenTransformationMatrix;

    /// <summary>
    /// Transformation matrix to map virtual screen points to ones in the game world
    /// </summary>
    public Matrix InverseVirtualScreenTransformationMatrix => _inverseVirtualScreenTransformationMatrix;

    public Camera(Resources resources)
    {
        _resources = resources;
        SetCameraPosition(Vector2.Zero, 0f, 1f);
    }

    /// <summary>
    /// Get the real window position from a game world position
    /// </summary>
    /// <param name="gameWorldPos">The game world source position</param>
    /// <returns>The final position coordinates</returns>
    public Vector2 GetRealWindowPos(Vector2 gameWorldPos)
    {
        return Vector2.Transform(gameWorldPos, InverseTransformationMatrix);
    }

    /// <summary>
    /// Get the game world position from a real window position
    /// </summary>
    /// <param name="realWindowPos">The game world source position</param>
    /// <returns>The final position coordinates</returns>
    public Vector2 GetGameWorldPosFromRealWindowPos(Vector2 realWindowPos)
    {
        return Vector2.Transform(realWindowPos, InverseTransformationMatrix);
    }

    /// <summary>
    /// Get the virtual screen position from a game world position
    /// </summary>
    /// <param name="gameWorldPos">The game world source position</param>
    /// <returns>The final position coordinates</returns>
    public Vector2 GetVirtualPos(Vector2 gameWorldPos)
    {
        return Vector2.Transform(gameWorldPos, VirtualScreenTransormationMatrix);
    }

    /// <summary>
    /// Translates virtual screen position to a game world position
    /// </summary>
    /// <param name="virtualScreenPos">A position in the outer game window</param>
    /// <param name="effects">SpriteEffects to account for</param>
    /// <returns></returns>
    public Vector2 GetGameWorldPosFromVirtualScreenPos(Vector2 virtualScreenPos)
    {
        return Vector2.Transform(virtualScreenPos, InverseVirtualScreenTransformationMatrix);
    }

    public void SetCameraPosition(Vector2 position, float angle, float zoom)
    {
        // Create transformation matrices
        _virtualScreenTransformationMatrix = Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0))
            * Matrix.CreateRotationZ(-angle)
            * Matrix.CreateScale(zoom,-zoom,zoom)
            * Matrix.CreateTranslation(new Vector3(VirtualScreen.Center.X, VirtualScreen.Center.Y, 0))
        ;
        _transformationMatrix = _virtualScreenTransformationMatrix * _resources.VirtualScreen.TransformationMatrix;
        _inverseVirtualScreenTransformationMatrix = Matrix.Invert(_virtualScreenTransformationMatrix);
        _inverseTransformationMatrix = Matrix.Invert(_transformationMatrix);
    }

    public void Update(GameTime gt)
    {
        
    }

    public void Draw(GameTime gt, SpriteBatch sb)
    {
        
    }
}
