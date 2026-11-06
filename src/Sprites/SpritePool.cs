
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Microsoft.Xna.Framework;

namespace cis_580_game;

public class SpritePool
{
    [JsonIgnore]
    private Resources _resources;

    private Dictionary<Type, List<ISprite>> _spriteLists;

    /// <summary>
    /// An enumerator for every sprite
    /// </summary>
    public IEnumerable<ISprite> AllSprites => _spriteLists.Values.SelectMany(x => x);

    /// <summary>
    /// Returns all sprites that are a certain type
    /// </summary>
    /// <param name="type">Type of sprite to get</param>
    /// <returns>Sprites of that type</returns>
    public List<ISprite> GetSpritesOfType(Type type)
    {
        return _spriteLists.TryGetValue(type, out List<ISprite> value) ? value : [];
    }
}

