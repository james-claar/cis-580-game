
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace cis_580_game;

public class AnimatedTile
{
    private Tilemap _tilemap;

    /// <summary>
    /// The tilemap texture
    /// </summary>
    public Tilemap Tilemap => _tilemap;

    /// <summary>
    /// Width of a tile
    /// </summary>
    public int Width => _tilemap.TileWidth;

    /// <summary>
    /// Height of a tile
    /// </summary>
    public int Height => _tilemap.TileHeight;

    /// <summary>
    /// Relative origin of this tile
    /// </summary>
    public Vector2 Origin => _tilemap.TileOrigin;

    /// <summary>
    /// Default tile
    /// </summary>
    public Rectangle? DefaultTile => _tilemap.DefaultTile;

    /// <summary>
    /// Animations
    /// </summary>
    public List<List<Rectangle>> Animations => _tilemap.Animations;

    private List<bool> _animationRepeating = [];

    private List<TimeSpan> _animationLengths = [];

    private List<TimeSpan?> _animationStartTimestamps = [];

    /// <summary>
    /// Constructs an AnimatedTile
    /// </summary>
    /// <param name="texture">The tilemap texture to use</param>
    /// <param name="tile">The tile size</param>
    public AnimatedTile(Tilemap tilemap, IEnumerable<bool> animationRepeating, IEnumerable<TimeSpan> animationLengths)
    {
        _tilemap = tilemap;
        _animationRepeating = animationRepeating.ToList();
        _animationLengths = animationLengths.ToList();
        if (_animationRepeating.Count != Animations.Count) throw new ArgumentException("Animation repeating count must be equal to the number of animations in the tilemap!");
        if (_animationLengths.Count != Animations.Count) throw new ArgumentException("Animation length count must be equal to the number of animations in the tilemap!");

        for (int i = 0; i < Animations.Count; i++) _animationStartTimestamps.Add(null);
    }

    /// <summary>
    /// Gets the current tile bounds
    /// </summary>
    /// <param name="gt">The GameTime</param>
    /// <returns>The current tile</returns>
    public Rectangle? GetCurrentTile(GameTime gt)
    {
        if (
            _animationLengths.Count != Animations.Count
            || _animationStartTimestamps.Count != Animations.Count
            || _animationRepeating.Count != Animations.Count
        ) throw new Exception("Animation data lists must be the same length!");

        for (int i = 0; i < Animations.Count; i++)
        {
            if (_animationStartTimestamps[i] is null) continue;
            TimeSpan start = (TimeSpan)_animationStartTimestamps[i];

            if (gt.TotalGameTime < start) continue;
            TimeSpan elapsed = gt.TotalGameTime - start;

            if (_animationRepeating[i]) elapsed %= _animationLengths[i];
            else if (elapsed > _animationLengths[i]) continue;

            TimeSpan timePerFrame = _animationLengths[i] / Animations[i].Count;
            int frame = MathHelper.Clamp((int)Math.Floor(elapsed / timePerFrame), 0, Animations[i].Count - 1);
            return Animations[i][frame];
        }

        return DefaultTile;
    }

    /// <summary>
    /// Starts an animation
    /// </summary>
    /// <param name="animationIndex">Index of the animation</param>
    /// <param name="gt">The GameTime</param>
    public void Play(int animationIndex, GameTime gt)
    {
        _animationStartTimestamps[animationIndex] = gt.TotalGameTime;
    }

    /// <summary>
    /// Stops an animation
    /// </summary>
    /// <param name="animationIndex">Index of the animation</param>
    public void Stop(int animationIndex)
    {
        _animationStartTimestamps[animationIndex] = null;
    }

    /// <summary>
    /// Stops all animations
    /// </summary>
    /// <param name="animationIndex">Index of the animation</param>
    public void StopAll()
    {
        for (int i = 0; i < _animationStartTimestamps.Count; i++) Stop(i);
    }

    public void Draw(SpriteBatch sb, GameTime gt, RectangleF destinationRectangle, Color color, float rotation, SpriteEffects effects, float layerDepth)
    {
        Rectangle? source = GetCurrentTile(gt);
        if (source is null) return;
        sb.Draw(_tilemap.Texture, destinationRectangle, source, color, rotation, _tilemap.TileOrigin, effects, layerDepth);
    }

    public void Draw(SpriteBatch sb, GameTime gt, Vector2 position, Color color, float rotation, float scale, SpriteEffects effects, float layerDepth)
    {
        Rectangle? source = GetCurrentTile(gt);
        if (source is null) return;
        sb.Draw(_tilemap.Texture, position, source, color, rotation, _tilemap.TileOrigin, scale, effects, layerDepth);
    }
}
