

using System;
using System.Collections.Generic;
using System.Dynamic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace cis_580_game;

class Asteroid
{
    private Resources _resources;

    private float _radius = 0f;

    public float Radius
    {
        get => _radius;
        private set => _radius = value;
    }

    private Vector2 _position = Vector2.Zero;

    /// <summary>
    /// The asteroid's position
    /// </summary>
    public Vector2 Position
    {
        get => _position;
        private set => _position = value;
    }

    private Vector2 _velocity = Vector2.Zero;

    /// <summary>
    /// The asteroid's velocity
    /// </summary>
    public Vector2 Velocity
    {
        get => _velocity;
        private set => _velocity = value;
    }

    private float _angle = 0f;

    /// <summary>
    /// The asteroid's angle
    /// </summary>
    public float Angle
    {
        get => _angle;
        private set => _angle = value;
    }

    private float _angularVelocity = 0f;

    /// <summary>
    /// The asteroid's angular velocity
    /// </summary>
    public float AngularVelocity
    {
        get => _angularVelocity;
        private set => _angularVelocity = value;
    }

    private Texture2D _tilemapTexture;

    /// <summary>
    /// Rectangles for the tilemap PixelStarshipsPackage_Asteroids_01
    /// </summary>
    private static readonly List<Rectangle> _tilemapRectangles = [
        new(0, 0, 32, 32),
        new(32, 0, 32, 32),
        new(0, 32, 32, 32),
        new(32, 32, 32, 32),
        new(64, 0, 64, 64),
        new(0, 64, 64, 64),
        new(64, 64, 64, 64),
        new(128, 0, 64, 128),
        new(192, 0, 64, 128),
        new(0, 128, 128, 128),
        new(128, 128, 128, 128),
        new(256, 0, 128, 128),
        new(256, 128, 128, 128),
        new(384, 0, 128, 256),
        new(0, 256, 256, 256),
        new(256, 256, 256, 256)
    ];

    private Rectangle _tileBounds = _tilemapRectangles[0];

    private Vector2 _origin = Vector2.Zero;

    public Asteroid(Resources resources)
    {
        _resources = resources;
        _tilemapTexture = _resources.AsteroidTilemapTexture;

        Random RNG = new();
        _radius = 100f + 30f * (2f*RNG.NextSingle() - 1f);
        _position = new();
        _velocity = new();
        _position.X = RNG.NextSingle() * ScaledRenderer.VirtualScreenWidth;
        _position.Y = RNG.NextSingle() * ScaledRenderer.VirtualScreenHeight;
        _velocity.X = 300f * (2f*RNG.NextSingle() - 1f);
        _velocity.Y = 300f * (2f*RNG.NextSingle() - 1f);
        _angularVelocity = 0.5f * (2*(float)Math.PI * (2f*RNG.NextSingle() - 1f));
        _tileBounds = _tilemapRectangles[RNG.Next(7, _tilemapRectangles.Count - 1)];
        _origin = new(_tileBounds.Width / 2f, _tileBounds.Height / 2f);
    }

    public void Update(GameTime gt)
    {
        float dt = (float)gt.ElapsedGameTime.TotalSeconds;

        if (_position.X - _radius <= 0)
        {
            _position.X = _radius;
            _velocity.X = Math.Abs(_velocity.X);
        }
        if (_position.X + _radius >= ScaledRenderer.VirtualScreenWidth)
        {
            _position.X = ScaledRenderer.VirtualScreenWidth - _radius;
            _velocity.X = -Math.Abs(_velocity.X);
        }

        if (_position.Y - _radius <= 0)
        {
            _position.Y = _radius;
            _velocity.Y = Math.Abs(_velocity.Y);
        }
        if (_position.Y + _radius >= ScaledRenderer.VirtualScreenHeight)
        {
            _position.Y = ScaledRenderer.VirtualScreenHeight - _radius;
            _velocity.Y = -Math.Abs(_velocity.Y);
        }

        _position += _velocity * dt;
        _angle += _angularVelocity * dt;
    }

    public void HandleCollision(Asteroid other)
    {
        BoundingCircle thisBounds = new(_position, _radius);
        BoundingCircle otherBounds = new(other._position, other._radius);

        if (thisBounds.CollidesWith(otherBounds))
        {
            // TODO: use more refined collision resolution
            float sizeRatio = _radius*_radius / (other._radius*other._radius);

            // Swap velocities and rotational velocities
            (_velocity, other._velocity) = (other._velocity/sizeRatio, _velocity*sizeRatio);
            (_angularVelocity, other._angularVelocity) = (other._angularVelocity/sizeRatio, _angularVelocity*sizeRatio);

            // Push asteroids away from each other such that they are no longer touching
            float combinedRadius = _radius + other._radius;
            if (_position == other._position) _position += new Vector2(Math.Sign(ScaledRenderer.VirtualScreenHorizontalCenter - _position.X), Math.Sign(ScaledRenderer.VirtualScreenVerticalCenter - Position.Y));
            Vector2 positionDifference = _position - other._position;
            float centerDistance = positionDifference.Length();
            Vector2 normalizedDifference = positionDifference/Math.Max(centerDistance, 1f);
            float intersectSize = combinedRadius - centerDistance;
            Vector2 push = intersectSize * normalizedDifference / 2f;

            //_position += positionDifference * (intersectSize/combinedRadius);
            //other._position -= positionDifference * (intersectSize/combinedRadius);
            _position += push;
            other._position -= push;
        }
    }

    public void Draw(GameTime gt, SpriteBatch sb)
    {
        _resources.ScaledRenderer.Draw(_tilemapTexture, _position, _tileBounds, Color.White, _angle, _origin, 2f*_radius*new Vector2(1f/_tileBounds.Width, 1f/_tileBounds.Height), SpriteEffects.None, Layers.GameplayShips);
    }
}
