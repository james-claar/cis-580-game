
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace cis_580_game;

public class TilemapAnimation
{
    /// <summary>
    /// List of tiles
    /// </summary>
    public readonly List<Rectangle> Tiles;

    /// <summary>
    /// Animation length
    /// </summary>
    public TimeSpan AnimationLength;

    private bool _repeating;

    /// <summary>
    /// Whether the animation should restart after finishing
    /// </summary>
    public bool Repeating => _repeating;

    private bool _hasBeenTriggered;

    /// <summary>
    /// Wheher the animation has been triggered
    /// </summary>
    public bool HasBeenTriggered => _hasBeenTriggered;

    private TimeSpan _lastTriggeredTimestamp = TimeSpan.MaxValue;

    public TilemapAnimation(List<Rectangle> tiles, TimeSpan animationLength, bool repeating = false)
    {
        if (tiles.Count <= 0) throw new ArgumentException("Tiles list must not be empty.");
        Tiles = tiles;
        AnimationLength = animationLength;
        _repeating = repeating;
        _hasBeenTriggered = false;
    }

    /// <summary>
    /// Starts/Restarts this animation
    /// </summary>
    /// <param name="gt">Current GameTime</param>
    public void Play(GameTime gt)
    {
        _hasBeenTriggered = true;
        _lastTriggeredTimestamp = gt.TotalGameTime;
    }

    public void Stop()
    {
        _hasBeenTriggered = false;
    }

    /// <summary>
    /// Returns whether the animation is currently playing
    /// </summary>
    /// <param name="gt">Current GameTime</param>
    /// <returns>True if the animation is playing, false otherwise</returns>
    public bool IsPlaying(GameTime gt)
    {
        return _hasBeenTriggered && (_repeating || (gt.TotalGameTime - _lastTriggeredTimestamp) < AnimationLength);
    }

    /// <summary>
    /// Returns the current animation tile
    /// </summary>
    /// <param name="gt">Current GameTime</param>
    public Rectangle GetTile(GameTime gt)
    {
        int unboundedIndex = Math.Max(0, (int)Math.Floor((gt.TotalGameTime - _lastTriggeredTimestamp) / AnimationLength * Tiles.Count));
        int index;
        if (_repeating) index = unboundedIndex % Tiles.Count;
        else           index = Math.Clamp(unboundedIndex, 0, Tiles.Count - 1);
        return Tiles[index];
    }
}
