
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace cis_580_game;

public class SpritePool
{
    private Resources _resources;

    [JsonProperty]
    List<Projectile> ProjectilePool = [];

    [JsonProperty]
    List<Turret> TurretPool = [];

    /// <summary>
    /// An enumerator for every sprite
    /// </summary>
    [JsonIgnore]
    public IEnumerable<ISprite> AllSprites =>
    [
        .._resources is not null && _resources.PlayerShip is not null ? [_resources.PlayerShip] : Array.Empty<ISprite>(),
        ..ProjectilePool,
        ..TurretPool
    ];

    public SpritePool(Resources resources)
    {
        Initialize(resources);
    }

    public void Initialize(Resources resources)
    {
        _resources = resources;

        // Ensure any existing sprites (loaded) are initialized
        foreach (ISprite sprite in AllSprites) sprite.Initialize(resources);
    }

    public void Add(params Projectile[] sprites) => ProjectilePool.AddRange(sprites);
    public void Remove(params Projectile[] sprites) {foreach (Projectile sprite in sprites) ProjectilePool.Remove(sprite);}
    public void Add(params Turret[] sprites) => TurretPool.AddRange(sprites);
    public void Remove(params Turret[] sprites) {foreach (Turret sprite in sprites) TurretPool.Remove(sprite);}

    public void Update(GameTime gt)
    {
        foreach (ISprite sprite in AllSprites) sprite.Update(gt);
    }

    public void Draw(GameTime gt, SpriteBatch sb)
    {
        foreach (ISprite sprite in AllSprites) sprite.Draw(gt, sb);
    }
}

