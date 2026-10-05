
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace cis_580_game;

public class AnimatedTilemap
{
    private Texture2D _texture;

    /// <summary>
    /// The tilemap texture
    /// </summary>
    public Texture2D Texture => _texture;

    /// <summary>
    /// Width of a tile
    /// </summary>
    public int TileWidth {get; private set;}

    /// <summary>
    /// Height of a tile
    /// </summary>
    public int TileHeight {get; private set;}

    private Vector2 _relativeTileOrigin;

    /// <summary>
    /// Absolute origin of the source tile
    /// </summary>
    public Vector2 TileOrigin {get; private set;}

    /// <summary>
    /// Number of tiles in the texture horizontally
    /// </summary>
    public int NumTilesWidth => _texture.Bounds.Width / TileWidth;

    /// <summary>
    /// Number of tiles in the texture vertically
    /// </summary>
    public int NumTilesHeight => _texture.Bounds.Height / TileHeight;

    private int _defaultTileIndexX;
    private int _defaultTileIndexY;

    /// <summary>
    /// List of animations
    /// </summary>
    public List<TilemapAnimation> Animations = [];

    /// <summary>
    /// Constructs an AnimatedTilemap
    /// </summary>
    /// <param name="texture">The tilemap texture to use</param>
    /// <param name="tile">The tile size</param>
    public AnimatedTilemap(Texture2D texture, int tileWidth, int tileHeight, Vector2? tileOrigin = null, int defaultTileIndexX = 0, int defaultTileIndexY = 0)
    {
        _texture = texture;
        TileWidth = tileWidth;
        TileHeight = tileHeight;
        if (NumTilesWidth <= 0 || NumTilesHeight <= 0) throw new ArgumentException("Texture must be large enough for at least one tile!");

        _defaultTileIndexX = defaultTileIndexX;
        _defaultTileIndexY = defaultTileIndexY;
        _relativeTileOrigin = (tileOrigin is not null) ? (Vector2)tileOrigin : new(tileWidth / 2f, tileHeight / 2f);
    }

    /// <summary>
    /// Indexes the tilemap to get bounds at a specific tile
    /// </summary>
    /// <param name="tileIndexX">tile ___ from the left, zero-indexed</param>
    /// <param name="tileIndexY">tile ___ from the top, zero-indexed</param>
    /// <returns>The bounds of the tile</returns>
    public Rectangle TileAt(int tileIndexX, int tileIndexY)
    {
        return new(
            Math.Clamp(tileIndexX, 0, NumTilesWidth - 1) * TileWidth,
            Math.Clamp(tileIndexY, 0, NumTilesHeight - 1) * TileHeight,
            TileWidth,
            TileHeight
        );
    }

    /// <summary>
    /// Gets the current tile
    /// </summary>
    /// <param name="gt">The GameTime</param>
    /// <returns>The current tile</returns>
    public Rectangle GetCurrentTile(GameTime gt)
    {
        foreach (TilemapAnimation animation in Animations)
        {
            if (animation.IsPlaying(gt)) return animation.GetTile(gt);
        }
        return TileAt(_defaultTileIndexX, _defaultTileIndexY);
    }

    public bool IsAnimationPlaying(GameTime gt)
    {
        return Animations.Any(animation => animation.IsPlaying(gt));
    }

    public void AddAnimation(IEnumerable<(int, int)> tileIndices, TimeSpan animationLength, bool repeating = false)
    {
        List<Rectangle> tiles = [];
        foreach ((int, int) tileIndex in tileIndices) tiles.Add(TileAt(tileIndex.Item1, tileIndex.Item2));
        Animations.Add(new(tiles, animationLength, repeating));
    }
}

