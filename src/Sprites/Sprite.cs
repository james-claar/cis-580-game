
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace cis_580_game;

public class Sprite : ISprite
{
    protected Resources _resources;

    protected bool _isInitialized;
    /// <summary>
    /// Whether this sprite has been initialized
    /// </summary>
    public bool IsInitialized => _isInitialized;

    /// <summary>
    /// Position of this sprite
    /// </summary>
    public virtual Vector2 Position {get; set;} = Vector2.Zero;

    /// <summary>
    /// Velocity of this sprite
    /// </summary>
    public virtual Vector2 Velocity {get; set;} = Vector2.Zero;

    /// <summary>
    /// Acceleration of this sprite
    /// </summary>
    public virtual Vector2 Acceleration {get; set;} = Vector2.Zero;

    /// <summary>
    /// Angle of this sprite in radians, counterclockwise from +X axis
    /// </summary>
    public virtual float Angle {get; set;} = 0f;

    /// <summary>
    /// Angular velocity of this sprite
    /// </summary>
    public virtual float AngularVelocity {get; set;} = 0f;

    /// <summary>
    /// Angular acceleration of this sprite
    /// </summary>
    public virtual float AngularAcceleration {get; set;} = 0f;

    /// <summary>
    /// Drag formula
    /// </summary>
    public virtual Vector2 Drag => 0.0008f*(Velocity.AbsPos*Velocity) + 0.5f*Velocity;

    /// <summary>
    /// Angular drag formula
    /// </summary>
    public virtual float AngularDrag => 0.5f*(Math.Abs(AngularVelocity)*AngularVelocity) + 1f*AngularVelocity;

    protected bool _canBeDamaged;
    /// <summary>
    /// Whether this sprite can take damage
    /// </summary>
    public virtual bool CanBeDamaged {get => _canBeDamaged; set => _canBeDamaged = value;}

    /// <summary>
    /// Which team this sprite is aligned with
    /// </summary>
    public Team Team;

    protected int _maxHealth = 100;
    /// <summary>
    /// Max health of this sprite
    /// </summary>
    public int MaxHealth
    {
        get => _maxHealth;
        set
        {
            int newValue = MathHelper.Max(1, value);
            _health += MathHelper.Max(0, newValue - _maxHealth);
            _maxHealth = newValue;
        }
    }

    protected int _health = 100;
    /// <summary>
    /// Health of this sprite
    /// </summary>
    public int Health
    {
        get => _health;
        set
        {
            int nextHealth = MathHelper.Clamp(value, CanBeDamaged ? 0 : _health, _maxHealth);
            bool died = nextHealth == 0 && !Dead;
            _health = nextHealth;
            if (died) DeathEvent.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>
    /// Health percentage from 0-100
    /// </summary>
    public float HealthPercentage => Math.Clamp(100f * (float)_health / (float)_maxHealth, 0f, 100f);

    /// <summary>
    /// Whether the health is zero
    /// </summary>
    public bool Dead => Health == 0;

    /// <summary>
    /// Triggered when Health drops to 0
    /// </summary>
    public event EventHandler<EventArgs> DeathEvent;

    public Sprite()
    {
        
    }

    public Sprite(Resources resources)
    {
        Initialize(resources);
    }

    /// <summary>
    /// Points this sprite at a position
    /// </summary>
    /// <param name="position">Position to point at</param>
    public void PointAt(Vector2 position)
    {
        Angle = Position.AnglePointingAt(position);
    }

    /// <summary>
    /// Sets the velocity to be along the forward/backward axis
    /// </summary>
    public void ForceVelocityForwardBackward()
    {
        Velocity = Velocity.GetRotatedOntoLine(Angle);
    }

    /// <summary>
    /// Gets the first possible position to aim at
    /// </summary>
    /// <param name="sourcePos">Source position</param>
    /// <param name="bulletSpeed">Speed of the projectile</param>
    /// <param name="intersectionPoint">Will be set to the intersection point on success, or Vector2.NaN on fail</param>
    /// <returns>A boolean representing whether we can hit the target</returns>
    public bool GetTargetLeadPosition(Vector2 sourcePos, float bulletSpeed, out Vector2 intersectionPoint, float linearEpsilon = 1e-9f)
    {
        // https://gamedev.stackexchange.com/a/25292
        Vector2 sourceToTarget = Position - sourcePos;

        float a = Velocity.LengthSquared() - bulletSpeed*bulletSpeed;
        float b = 2f * Vector2.Dot(Velocity, sourceToTarget);
        float c = sourceToTarget.LengthSquared();

        float t = 0f;
        if (MathF.Abs(a) > linearEpsilon)
        {
            // Need to solve quadratic equation for t
            float det = (b*b) - 4*a*c;
            if (det < 0)
            {
                intersectionPoint = Vector2.NaN;
                return false;
            }
            float p = -b / (2f * a);
            float q = (float)MathF.Sqrt(det) / (2f * a);
            float t1 = p - q;
            float t2 = p + q;
            if ((t2 < 0 || t1 < t2) && t1 >= 0)
            {
                t = t1;
            }
            else if (t2 >= 0)
            {
                t = t2;
            }
            else
            {
                intersectionPoint = Vector2.NaN;
                return false;
            }
        }
        else
        {
            // Need to solve approximately linear equation for t
            if (MathF.Abs(b) < linearEpsilon)
            {
                intersectionPoint = Vector2.NaN;
                return false;
            }
            t = -c/b;
        }
        intersectionPoint = Position + Velocity*t;
        return true;
    }

    public virtual void Initialize(Resources resources)
    {
        _resources = resources;
        _isInitialized = true;
    }

    public virtual void Update(GameTime gt)
    {
        if (!_isInitialized) return;
        float dt = (float)gt.ElapsedGameTime.TotalSeconds;
        Position += Velocity * dt;
        Velocity += (Acceleration - Drag) * dt;
        Angle += AngularVelocity * dt;
        AngularVelocity += (AngularAcceleration - AngularDrag) * dt;
    }

    public virtual void Draw(GameTime gt, SpriteBatch sb)
    {
        if (!_isInitialized) return;
        throw new NotImplementedException();
    }

    bool ISprite.IsInitialized => _isInitialized;
    Vector2 ISprite.Position { get => Position; set => Position = value; }
    Vector2 ISprite.Velocity { get => Velocity; set => Velocity = value; }
    Vector2 ISprite.Acceleration { get => Acceleration; set => Acceleration = value; }
    float ISprite.Angle { get => Angle; set => Angle = value; }
    float ISprite.AngularVelocity { get => AngularVelocity; set => AngularVelocity = value; }
    float ISprite.AngularAcceleration { get => AngularAcceleration; set => AngularAcceleration = value; }
    bool ISprite.CanBeDamaged { get => CanBeDamaged; set => CanBeDamaged = value; }
    Team ISprite.Team { get => Team; set => Team = value; }
}
