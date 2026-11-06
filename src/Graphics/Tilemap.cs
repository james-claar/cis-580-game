
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace cis_580_game;

public struct Tilemap
{
    private Texture2D _texture;
    /// <summary>
    /// The tilemap texture
    /// </summary>
    public Texture2D Texture => _texture;

    private int _tileWidth;
    /// <summary>
    /// Width of a tile
    /// </summary>
    public int TileWidth => _tileWidth;

    public int _tileHeight;
    /// <summary>
    /// Height of a tile
    /// </summary>
    public  int TileHeight => _tileHeight;

    private Vector2 _tileOrigin;
    /// <summary>
    /// Relative origin of a tile
    /// </summary>
    public Vector2 TileOrigin => _tileOrigin;

    private int _numTilesWidth;
    /// <summary>
    /// Number of tiles in the texture horizontally
    /// </summary>
    public int NumTilesWidth => _numTilesWidth;

    private int _numTilesHeight;
    /// <summary>
    /// Number of tiles in the texture vertically
    /// </summary>
    public int NumTilesHeight => _numTilesHeight;

    private Rectangle? _defaultTile;
    /// <summary>
    /// Default tile used when no animations are playing
    /// </summary>
    public Rectangle? DefaultTile => _defaultTile;

    private List<List<Rectangle>> _animations;
    /// <summary>
    /// Tiles used for each animation
    /// </summary>
    public List<List<Rectangle>> Animations => _animations;

    public Tilemap(Texture2D texture, int tileWidth, int tileHeight, Vector2? tileOrigin = null, (int, int)? defaultTileIndex = null)
    {
        if (texture is null) throw new ArgumentException("Texture cannot be null!");
        _texture = texture;
        _tileWidth = tileWidth;
        _tileHeight = tileHeight;
        _tileOrigin = (tileOrigin is null) ? new Vector2(0.5f*_tileWidth, 0.5f*_tileHeight) : (Vector2)tileOrigin;
        _numTilesWidth = texture.Width / tileWidth;
        _numTilesHeight = texture.Height / tileWidth;

        if (NumTilesWidth <= 0 || NumTilesHeight <= 0) throw new ArgumentException("Texture must be large enough for at least one tile!");

        _defaultTile = (defaultTileIndex is null) ? null : TileAt(((int, int))defaultTileIndex);
        _animations = [];
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
            MathHelper.Clamp(tileIndexX, 0, NumTilesWidth - 1) * TileWidth,
            MathHelper.Clamp(tileIndexY, 0, NumTilesHeight - 1) * TileHeight,
            TileWidth,
            TileHeight
        );
    }

    public Rectangle TileAt((int, int) tileIndex)
    {
        return TileAt(tileIndex.Item1, tileIndex.Item2);
    }

    /// <summary>
    /// Gets an animation using a continuous rectangular region of tiles.
    /// </summary>
    /// <param name="startTileIndex">Starting tile index</param>
    /// <param name="width">Number of tiles in the animation horizontally</param>
    /// <param name="height">Number of tiles in the animation vertically</param>
    /// <param name="horizontalFirst">Whether to loop over the map horizontally before vertically</param>
    /// <returns>A list of tiles</returns>
    public List<Rectangle> GetAnimationTiles((int, int) startTileIndex, int width = 1, int height = 1, bool horizontalFirst = true)
    {
        List<(int, int)> tileIndices = [];
        if (horizontalFirst) for (int row = 0; row < height; row++) for (int col = 0; col < width; col++)
        {
            tileIndices.Add((startTileIndex.Item1 + col, startTileIndex.Item2 + row));
        }
        else for (int col = 0; col < width; col++) for (int row = 0; row < height; row++)
        {
            tileIndices.Add((startTileIndex.Item1 + col, startTileIndex.Item2 + row));
        }

        return GetAnimationTiles(tileIndices);
    }

    /// <summary>
    /// Gets an animation using an arbitrary set of tile indices
    /// </summary>
    /// <param name="tileIndices">Tile indices</param>
    /// <returns>A list of tiles</returns>
    public List<Rectangle> GetAnimationTiles(IEnumerable<(int, int)> tileIndices)
    {
        List<Rectangle> tiles = [];
        foreach((int, int) tileIndex in tileIndices) tiles.Add(TileAt(tileIndex));
        return tiles;
    }

    /// <summary>
    /// Adds an animation using a continuous rectangular region of tiles.
    /// </summary>
    /// <param name="startTileIndex">Starting tile index</param>
    /// <param name="width">Number of tiles in the animation horizontally</param>
    /// <param name="height">Number of tiles in the animation vertically</param>
    /// <param name="horizontalFirst">Whether to loop over the map horizontally before vertically</param>
    public void AddAnimation((int, int) startTileIndex, int width = 1, int height = 1, bool horizontalFirst = true)
    {
        Animations.Add(GetAnimationTiles(startTileIndex, width, height, horizontalFirst));
    }

    /// <summary>
    /// Adds an animation using an arbitrary set of tile indices
    /// </summary>
    /// <param name="tileIndices">Tile indices</param>
    public void AddAnimation(IEnumerable<(int, int)> tileIndices)
    {
        Animations.Add(GetAnimationTiles(tileIndices));
    }

    public Rectangle this[int tileIndexX, int tileIndexY = 0] => TileAt(tileIndexX, tileIndexY);

    public Rectangle this[(int, int) tileIndex] => TileAt(tileIndex);
}
