// Modified from Microsoft.Xna.Framework.Rectangle

using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;

namespace cis_580_game;

//
// Summary:
//     Describes a 2D-rectangle.
[DataContract]
[DebuggerDisplay("{DebugDisplayString,nq}")]
public struct RectangleF : IEquatable<RectangleF>
{
    private static RectangleF emptyRectangle = new();

    //
    // Summary:
    //     The x coordinate of the top-left corner of this RectangleF.
    [DataMember]
    public float X;

    //
    // Summary:
    //     The y coordinate of the top-left corner of this RectangleF.
    [DataMember]
    public float Y;

    //
    // Summary:
    //     The width of this RectangleF.
    [DataMember]
    public float Width;

    //
    // Summary:
    //     The height of this RectangleF.
    [DataMember]
    public float Height;

    //
    // Summary:
    //     Returns a RectangleF with X=0, Y=0, Width=0, Height=0.
    public static RectangleF Empty => emptyRectangle;

    //
    // Summary:
    //     Returns the x coordinate of the left edge of this RectangleF.
    public float Left => X;

    //
    // Summary:
    //     Returns the x coordinate of the right edge of this RectangleF.
    public float Right => X + Width;

    //
    // Summary:
    //     Returns the y coordinate of the top edge of this RectangleF.
    public float Top => Y;

    //
    // Summary:
    //     Returns the y coordinate of the bottom edge of this RectangleF.
    public float Bottom => Y + Height;

    //
    // Summary:
    //     Whether or not this RectangleF has a RectangleF.Width
    //     and RectangleF.Height of 0, and a RectangleF.Location
    //     of (0, 0).
    public bool IsEmpty
    {
        get
        {
            if (Width == 0 && Height == 0 && X == 0)
            {
                return Y == 0;
            }

            return false;
        }
    }

    //
    // Summary:
    //     The top-left coordinates of this RectangleF.
    public Vector2 Location
    {
        get
        {
            return new Vector2(X, Y);
        }
        set
        {
            X = value.X;
            Y = value.Y;
        }
    }

    //
    // Summary:
    //     The width-height coordinates of this RectangleF.
    public Vector2 Size
    {
        get
        {
            return new Vector2(Width, Height);
        }
        set
        {
            Width = value.X;
            Height = value.Y;
        }
    }

    //
    // Summary:
    //     A Vector2 located in the center of this RectangleF.
    //
    //
    // Remarks:
    //     If RectangleF.Width or RectangleF.Height
    //     is an odd number, the center point will be rounded down.
    public Vector2 Center => new Vector2(X + Width / 2, Y + Height / 2);

    internal string DebugDisplayString => X + "  " + Y + "  " + Width + "  " + Height;

    //
    // Summary:
    //     Creates a new instance of RectangleF struct, with the
    //     specified position, width, and height.
    //
    // Parameters:
    //   x:
    //     The x coordinate of the top-left corner of the created RectangleF.
    //
    //
    //   y:
    //     The y coordinate of the top-left corner of the created RectangleF.
    //
    //
    //   width:
    //     The width of the created RectangleF.
    //
    //   height:
    //     The height of the created RectangleF.
    public RectangleF(float x, float y, float width, float height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    //
    // Summary:
    //     Creates a new instance of RectangleF struct, with the
    //     specified location and size.
    //
    // Parameters:
    //   location:
    //     The x and y coordinates of the top-left corner of the created RectangleF.
    //
    //
    //   size:
    //     The width and height of the created RectangleF.
    public RectangleF(Vector2 location, Vector2 size)
    {
        X = location.X;
        Y = location.Y;
        Width = size.X;
        Height = size.Y;
    }

    //
    // Summary:
    //     Compares whether two RectangleF instances are equal.
    //
    // Parameters:
    //   a:
    //     RectangleF instance on the left of the equal sign.
    //
    //   b:
    //     RectangleF instance on the right of the equal sign.
    //
    // Returns:
    //     true if the instances are equal; false otherwise.
    public static bool operator ==(RectangleF a, RectangleF b)
    {
        if (a.X == b.X && a.Y == b.Y && a.Width == b.Width)
        {
            return a.Height == b.Height;
        }

        return false;
    }

    //
    // Summary:
    //     Compares whether two RectangleF instances are not equal.
    //
    //
    // Parameters:
    //   a:
    //     RectangleF instance on the left of the not equal sign.
    //
    //
    //   b:
    //     RectangleF instance on the right of the not equal sign.
    //
    //
    // Returns:
    //     true if the instances are not equal; false otherwise.
    public static bool operator !=(RectangleF a, RectangleF b)
    {
        return !(a == b);
    }

    //
    // Summary:
    //     Gets whether or not the provided coordinates lie within the bounds of this RectangleF.
    //
    //
    // Parameters:
    //   x:
    //     The x coordinate of the point to check for containment.
    //
    //   y:
    //     The y coordinate of the point to check for containment.
    //
    // Returns:
    //     true if the provided coordinates lie inside this RectangleF;
    //     false otherwise.
    public bool Contains(float x, float y)
    {
        if (X <= x && x < X + Width && Y <= y)
        {
            return y < Y + Height;
        }

        return false;
    }

    //
    // Summary:
    //     Gets whether or not the provided Point lies within the
    //     bounds of this RectangleF.
    //
    // Parameters:
    //   value:
    //     The coordinates to check for inclusion in this RectangleF.
    //
    //
    // Returns:
    //     true if the provided Point lies inside this RectangleF;
    //     false otherwise.
    public bool Contains(Point value)
    {
        if (X <= value.X && value.X < X + Width && Y <= value.Y)
        {
            return value.Y < Y + Height;
        }

        return false;
    }

    //
    // Summary:
    //     Gets whether or not the provided Point lies within the
    //     bounds of this RectangleF.
    //
    // Parameters:
    //   value:
    //     The coordinates to check for inclusion in this RectangleF.
    //
    //
    //   result:
    //     true if the provided Point lies inside this RectangleF;
    //     false otherwise. As an output parameter.
    public void Contains(ref Point value, out bool result)
    {
        result = X <= value.X && value.X < X + Width && Y <= value.Y && value.Y < Y + Height;
    }

    //
    // Summary:
    //     Gets whether or not the provided Microsoft.Xna.Framework.Vector2 lies within
    //     the bounds of this RectangleF.
    //
    // Parameters:
    //   value:
    //     The coordinates to check for inclusion in this RectangleF.
    //
    //
    // Returns:
    //     true if the provided Microsoft.Xna.Framework.Vector2 lies inside this RectangleF;
    //     false otherwise.
    public bool Contains(Vector2 value)
    {
        if (X <= value.X && value.X < X + Width && Y <= value.Y)
        {
            return value.Y < Y + Height;
        }

        return false;
    }

    //
    // Summary:
    //     Gets whether or not the provided Microsoft.Xna.Framework.Vector2 lies within
    //     the bounds of this RectangleF.
    //
    // Parameters:
    //   value:
    //     The coordinates to check for inclusion in this RectangleF.
    //
    //
    //   result:
    //     true if the provided Microsoft.Xna.Framework.Vector2 lies inside this RectangleF;
    //     false otherwise. As an output parameter.
    public void Contains(ref Vector2 value, out bool result)
    {
        result = X <= value.X && value.X < X + Width && Y <= value.Y && value.Y < Y + Height;
    }

    //
    // Summary:
    //     Gets whether or not the provided RectangleF lies within
    //     the bounds of this RectangleF.
    //
    // Parameters:
    //   value:
    //     The RectangleF to check for inclusion in this RectangleF.
    //
    //
    // Returns:
    //     true if the provided RectangleF's bounds lie entirely
    //     inside this RectangleF; false otherwise.
    public bool Contains(RectangleF value)
    {
        if (X <= value.X && value.X + value.Width <= X + Width && Y <= value.Y)
        {
            return value.Y + value.Height <= Y + Height;
        }

        return false;
    }

    //
    // Summary:
    //     Gets whether or not the provided RectangleF lies within
    //     the bounds of this RectangleF.
    //
    // Parameters:
    //   value:
    //     The RectangleF to check for inclusion in this RectangleF.
    //
    //
    //   result:
    //     true if the provided RectangleF's bounds lie entirely
    //     inside this RectangleF; false otherwise. As an output
    //     parameter.
    public void Contains(ref RectangleF value, out bool result)
    {
        result = X <= value.X && value.X + value.Width <= X + Width && Y <= value.Y && value.Y + value.Height <= Y + Height;
    }

    //
    // Summary:
    //     Compares whether current instance is equal to specified System.Object.
    //
    // Parameters:
    //   obj:
    //     The System.Object to compare.
    //
    // Returns:
    //     true if the instances are equal; false otherwise.
    public override bool Equals(object obj)
    {
        if (obj is Rectangle)
        {
            return this == (RectangleF)obj;
        }

        return false;
    }

    //
    // Summary:
    //     Compares whether current instance is equal to specified RectangleF.
    //
    //
    // Parameters:
    //   other:
    //     The RectangleF to compare.
    //
    // Returns:
    //     true if the instances are equal; false otherwise.
    public bool Equals(RectangleF other)
    {
        return this == other;
    }

    //
    // Summary:
    //     Gets the hash code of this RectangleF.
    //
    // Returns:
    //     Hash code of this RectangleF.
    public override int GetHashCode()
    {
        return (((17 * 23 + X.GetHashCode()) * 23 + Y.GetHashCode()) * 23 + Width.GetHashCode()) * 23 + Height.GetHashCode();
    }

    //
    // Summary:
    //     Adjusts the edges of this RectangleF by specified horizontal
    //     and vertical amounts.
    //
    // Parameters:
    //   horizontalAmount:
    //     Value to adjust the left and right edges.
    //
    //   verticalAmount:
    //     Value to adjust the top and bottom edges.
    public void Inflate(float horizontalAmount, float verticalAmount)
    {
        X -= horizontalAmount;
        Y -= verticalAmount;
        Width += horizontalAmount * 2;
        Height += verticalAmount * 2;
    }

    //
    // Summary:
    //     Gets whether or not the other RectangleF intersects with
    //     this rectangle.
    //
    // Parameters:
    //   value:
    //     The other rectangle for testing.
    //
    // Returns:
    //     true if other RectangleF intersects with this rectangle;
    //     false otherwise.
    public bool Intersects(RectangleF value)
    {
        if (value.Left < Right && Left < value.Right && value.Top < Bottom)
        {
            return Top < value.Bottom;
        }

        return false;
    }

    //
    // Summary:
    //     Gets whether or not the other RectangleF intersects with
    //     this rectangle.
    //
    // Parameters:
    //   value:
    //     The other rectangle for testing.
    //
    //   result:
    //     true if other RectangleF intersects with this rectangle;
    //     false otherwise. As an output parameter.
    public void Intersects(ref RectangleF value, out bool result)
    {
        result = value.Left < Right && Left < value.Right && value.Top < Bottom && Top < value.Bottom;
    }

    //
    // Summary:
    //     Creates a new RectangleF that contains overlapping region
    //     of two other rectangles.
    //
    // Parameters:
    //   value1:
    //     The first RectangleF.
    //
    //   value2:
    //     The second RectangleF.
    //
    // Returns:
    //     Overlapping region of the two rectangles.
    public static RectangleF Intersect(RectangleF value1, RectangleF value2)
    {
        Intersect(ref value1, ref value2, out var result);
        return result;
    }

    //
    // Summary:
    //     Creates a new RectangleF that contains overlapping region
    //     of two other rectangles.
    //
    // Parameters:
    //   value1:
    //     The first RectangleF.
    //
    //   value2:
    //     The second RectangleF.
    //
    //   result:
    //     Overlapping region of the two rectangles as an output parameter.
    public static void Intersect(ref RectangleF value1, ref RectangleF value2, out RectangleF result)
    {
        if (value1.Intersects(value2))
        {
            float num = Math.Min(value1.X + value1.Width, value2.X + value2.Width);
            float num2 = Math.Max(value1.X, value2.X);
            float num3 = Math.Max(value1.Y, value2.Y);
            float num4 = Math.Min(value1.Y + value1.Height, value2.Y + value2.Height);
            result = new RectangleF(num2, num3, num - num2, num4 - num3);
        }
        else
        {
            result = new RectangleF(0, 0, 0, 0);
        }
    }

    //
    // Summary:
    //     Changes the RectangleF.Location of this RectangleF.
    //
    //
    // Parameters:
    //   offsetX:
    //     The x coordinate to add to this RectangleF.
    //
    //   offsetY:
    //     The y coordinate to add to this RectangleF.
    public void Offset(float offsetX, float offsetY)
    {
        X += offsetX;
        Y += offsetY;
    }

    //
    // Summary:
    //     Changes the RectangleF.Location of this RectangleF.
    //
    //
    // Parameters:
    //   amount:
    //     The x and y components to add to this RectangleF.
    public void Offset(Point amount)
    {
        X += amount.X;
        Y += amount.Y;
    }

    //
    // Summary:
    //     Changes the RectangleF.Location of this RectangleF.
    //
    //
    // Parameters:
    //   amount:
    //     The x and y components to add to this RectangleF.
    public void Offset(Vector2 amount)
    {
        X += amount.X;
        Y += amount.Y;
    }

    //
    // Summary:
    //     Returns a System.String representation of this RectangleF
    //     in the format: {X:[RectangleF.X] Y:[RectangleF.Y]
    //     Width:[RectangleF.Width] Height:[RectangleF.Height]}
    //
    //
    // Returns:
    //     System.String representation of this RectangleF.
    public override string ToString()
    {
        return "{X:" + X + " Y:" + Y + " Width:" + Width + " Height:" + Height + "}";
    }

    //
    // Summary:
    //     Creates a new RectangleF that completely contains two
    //     other rectangles.
    //
    // Parameters:
    //   value1:
    //     The first RectangleF.
    //
    //   value2:
    //     The second RectangleF.
    //
    // Returns:
    //     The union of the two rectangles.
    public static RectangleF Union(RectangleF value1, RectangleF value2)
    {
        float num = Math.Min(value1.X, value2.X);
        float num2 = Math.Min(value1.Y, value2.Y);
        return new RectangleF(num, num2, Math.Max(value1.Right, value2.Right) - num, Math.Max(value1.Bottom, value2.Bottom) - num2);
    }

    //
    // Summary:
    //     Creates a new RectangleF that completely contains two
    //     other rectangles.
    //
    // Parameters:
    //   value1:
    //     The first RectangleF.
    //
    //   value2:
    //     The second RectangleF.
    //
    //   result:
    //     The union of the two rectangles as an output parameter.
    public static void Union(ref RectangleF value1, ref RectangleF value2, out RectangleF result)
    {
        result.X = Math.Min(value1.X, value2.X);
        result.Y = Math.Min(value1.Y, value2.Y);
        result.Width = Math.Max(value1.Right, value2.Right) - result.X;
        result.Height = Math.Max(value1.Bottom, value2.Bottom) - result.Y;
    }

    //
    // Summary:
    //     Deconstruction method for RectangleF.
    //
    // Parameters:
    //   x:
    //
    //   y:
    //
    //   width:
    //
    //   height:
    public void Deconstruct(out float x, out float y, out float width, out float height)
    {
        x = X;
        y = Y;
        width = Width;
        height = Height;
    }

    public static implicit operator RectangleF(Rectangle rect) => new(rect.X, rect.Y, rect.Width, rect.Height);

    public static explicit operator Rectangle(RectangleF rectF) => new(
        (int)Math.Round(rectF.X),
        (int)Math.Round(rectF.Y),
        (int)Math.Round(rectF.Width),
        (int)Math.Round(rectF.Height)
    );
}
