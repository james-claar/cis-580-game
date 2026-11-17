
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace cis_580_game;

public class Projectile : Sprite
{
    AnimatedTile Tile;

    TimeSpan ShootAnimationLength = new(0,0,0,0,500);

    float DefaultScale = 4f;

    bool FirstDrawn = false;

    public override Vector2 Drag => Vector2.Zero;

    public float TextureAngleOffset = MathHelper.PiOver2;

    public bool Special = false;

    public Projectile()
    {

    }

    public override void Initialize(Resources resources)
    {
        base.Initialize(resources);

        Tile = new(_resources.MainShipProjectiles[0], [true], [ShootAnimationLength]);
    }

    public override void Draw(GameTime gt, SpriteBatch sb)
    {
        if (!_isInitialized) return;
        if (Special)
        {
            
        }
        else
        {
            if (!FirstDrawn)
            {
                Tile.Play(0, gt);
                FirstDrawn = true;
            }
            Tile.Draw(sb, gt, Position, Color.White, Angle + TextureAngleOffset, DefaultScale, SpriteEffects.None, Layers.GameplayStructures);
        }
    }
}