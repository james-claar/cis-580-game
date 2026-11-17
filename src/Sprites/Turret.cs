
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace cis_580_game;

public class Turret : Sprite
{
    Tilemap BaseTilemap;

    AnimatedTile GunTile;

    TimeSpan BulletAnimationLength = new(0,0,0,0,500);

    TimeSpan ShootCooldown = new(0,0,0,0,1000);

    TimeSpan LastFiredTimestamp = TimeSpan.MinValue;

    float DefaultScale = 5f;

    float RotationSpeed = 0.5f * MathHelper.TwoPi;

    public float BulletSpeed = 2000f;

    float BulletRange = 3000f;

    float DetectionRange = 6000f;

    /// <summary>
    /// If true, uses target lead calculation for much better aim
    /// </summary>
    public bool AimBot = false;

    public override Vector2 Drag => Velocity;

    public float TextureAngleOffset = MathHelper.PiOver2;

    public Turret()
    {

    }

    public override void Initialize(Resources resources)
    {
        base.Initialize(resources);

        BaseTilemap = Team == Team.Enemy ? _resources.EnemyWallsTilemap : _resources.PlayerWallsTilemap;
        Tilemap gunTilemap = Team == Team.Enemy ? _resources.EnemyGunTurretTilemaps[0] : _resources.PlayerGunTurretTilemaps[0];
        GunTile = new(gunTilemap, [false], [BulletAnimationLength]);
    }

    public override void Update(GameTime gt)
    {
        if (!_isInitialized) return;
        float dt = (float)gt.ElapsedGameTime.TotalSeconds;

        // Search spritepool for nearest enemy that can be damaged
        ISprite nearest;
        float distanceSquared = float.PositiveInfinity;
        float targetLeadDistanceSquared = float.PositiveInfinity;
        float detectionRangeSquared = DetectionRange*DetectionRange;
        bool found = false;
        Vector2 targetLeadPosition = Vector2.Zero;
        foreach(ISprite sprite in _resources.SpritePool.AllSprites)
        {
            if (sprite is Projectile) continue;
            if (ReferenceEquals(this, sprite)) continue;
            // TODO: check team

            float newDistanceSquared = Vector2.DistanceSquared(Position, sprite.Position);

            float newTargetLeadDistanceSquared = newDistanceSquared;
            Vector2 newTargetLeadPosition = sprite.Position;
            if (AimBot)
            {
                bool canHit = sprite.GetTargetLeadPosition(Position, BulletSpeed, out Vector2 intersectionPoint);
                float intersectionDistanceSquared = Vector2.DistanceSquared(Position, intersectionPoint);
                if (canHit)
                {
                    newTargetLeadPosition = intersectionPoint;
                    newTargetLeadDistanceSquared = intersectionDistanceSquared;
                }
            }

            if (newTargetLeadDistanceSquared > targetLeadDistanceSquared) continue;
            if (newDistanceSquared > detectionRangeSquared) continue;

            found = true;
            distanceSquared = newDistanceSquared;
            targetLeadDistanceSquared = newTargetLeadDistanceSquared;
            targetLeadPosition = newTargetLeadPosition;
            nearest = sprite;
        }
        if (found)
        {
            float angleToTargetPos = Position.AnglePointingAt(targetLeadPosition);
            float allowedRotation = RotationSpeed * dt;
            Angle += Math.Clamp(Vector2.NormalizeAngle(angleToTargetPos - Angle, true), -allowedRotation, allowedRotation);

            // Fire at target
            if (
                gt.TotalGameTime > LastFiredTimestamp + ShootCooldown
                && distanceSquared < BulletRange*BulletRange
                && MathF.Abs(Vector2.NormalizeAngle(Angle - angleToTargetPos, true)) <= 1e-9f
            )
            {
                LastFiredTimestamp = gt.TotalGameTime;
                GunTile.Play(0, gt);

                Projectile bullet = new()
                {
                    Position = Position,
                    Velocity = BulletSpeed * Vector2.Unit(Angle),
                    Angle = Angle,
                    Team = Team
                };
                bullet.Initialize(_resources);
                _resources.SpritePool.Add(bullet);
            }
        }

        base.Update(gt);
    }

    public override void Draw(GameTime gt, SpriteBatch sb)
    {
        if (!_isInitialized) return;
        GunTile.Draw(sb, gt, Position, Color.White, Angle + TextureAngleOffset, DefaultScale, SpriteEffects.None, Layers.GameplayStructures);
    }
}