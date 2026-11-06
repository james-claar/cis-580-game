
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace cis_580_game;

public interface ISprite
{
    /// <summary>
    /// Position of this sprite
    /// </summary>
    Vector2 Position { get; set; }

    /// <summary>
    /// Velocity of this sprite
    /// </summary>
    Vector2 Velocity { get; set; }

    /// <summary>
    /// Acceleration of this sprite
    /// </summary>
    Vector2 Acceleration { get; set; }

    /// <summary>
    /// Angle of this sprite in radians, counterclockwise from +X axis
    /// </summary>
    float Angle  { get; set; }

    /// <summary>
    /// Angular velocity of this sprite
    /// </summary>
    float AngularVelocity  { get; set; }

    /// <summary>
    /// Angular acceleration of this sprite
    /// </summary>
    float AngularAcceleration  { get; set; }

    /// <summary>
    /// Whether this sprite can take damage
    /// </summary>
    bool CanBeDamaged { get; set; }

    /// <summary>
    /// Max health of this sprite
    /// </summary>
    int MaxHealth { get; set; }

    /// <summary>
    /// Health of this sprite
    /// </summary>
    int Health { get; set; }

    /// <summary>
    /// Health percentage from 0-100
    /// </summary>
    float HealthPercentage { get; }

    /// <summary>
    /// Whether the health is zero
    /// </summary>
    bool Dead { get; }

    /// <summary>
    /// Triggered when Health drops to 0
    /// </summary>
    event EventHandler<EventArgs> DeathEvent;

    /// <summary>
    /// List of properties to be saved
    /// </summary>
    static string[] SavedProperties;

    /// <summary>
    /// Points this sprite at a position
    /// </summary>
    /// <param name="position">Position to point at</param>
    void PointAt(Vector2 position);

    /// <summary>
    /// Sets the velocity to be along the forward/backward axis
    /// </summary>
    void ForceVelocityForwardBackward();

    /// <summary>
    /// Gets the first possible position to aim at
    /// </summary>
    /// <param name="sourcePos">Source position</param>
    /// <param name="bulletSpeed">Speed of the projectile</param>
    /// <param name="intersectionPoint">Will be set to the intersection point on success, or Vector2.NaN on fail</param>
    /// <returns>A boolean representing whether we can hit the target</returns>
    bool GetTargetLeadPosition(Vector2 sourcePos, float bulletSpeed, out Vector2 intersectionPoint, float linearEpsilon);
}
