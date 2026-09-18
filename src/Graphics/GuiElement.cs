using Microsoft.Xna.Framework;

namespace cis_580_game;


/// <summary>
/// A parent class for all renderable GUI elements
/// </summary>
public class GuiElement
{
    protected Resources _resources;
    protected RectangleF _boundingBoxCache = new();
    protected bool _boundingBoxDirtyFlag = true;

    protected float _height = 0;

    /// <summary>
    /// Whether the element will be rendered
    /// </summary>
    public bool Visible = true;

    /// <summary>
    /// The element's height
    /// </summary>
    public float Height
    {
        get => _height;
        set
        {
            if (value != _height)
            {
                _height = value;
                _boundingBoxDirtyFlag = true;
            }
        }
    }

    protected float _width = 0;

    /// <summary>
    /// The element's width
    /// </summary>
    public float Width
    {
        get => _width;
        set
        {
            if (value != _width)
            {
                _width = value;
                _boundingBoxDirtyFlag = true;
            }
        }
    }

    protected Vector2 _position = Vector2.Zero;

    /// <summary>
    /// The element's position to align to
    /// </summary>
    public Vector2 Position
    {
        get => _position;
        set
        {
            if (value != _position)
            {
                _position = value;
                _boundingBoxDirtyFlag = true;
            }
        }
    }

    protected Alignment _alignment = new();

    /// <summary>
    /// The alignment of the element
    /// </summary>
    public Alignment Alignment
    {
        get => _alignment;
        set
        {
            if (value != _alignment)
            {
                _alignment = value;
                _boundingBoxDirtyFlag = true;
            }
        }
    }

    /// <summary>
    /// Whether the element should be selectable by mouse/controller input
    /// </summary>
    public bool IsSelectable = false;

    /// <summary>
    /// Whether this element is selected. This should be managed by the responsible class.
    /// </summary>
    public bool IsSelected = false;

    /// <summary>
    /// A get-only representation of the bounding box around this element
    /// </summary>
    public RectangleF BoundingBox
    {
        get
        {
            if (_boundingBoxDirtyFlag)
            {
                // Calculate bounding box position
                _boundingBoxCache = Alignment.GetAlignedBoundingBox(Position, Width, Height);

                // Clear dirty flag
                _boundingBoxDirtyFlag = false;
            }
            return _boundingBoxCache;
        }
    }
}
