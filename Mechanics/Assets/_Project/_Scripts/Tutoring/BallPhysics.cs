using UnityEngine;

public class BallPhysics
{
    private const double GRAVITY = 9.81;
    
    public double Position { get; private set; }
    public double Velocity { get; private set; }

    private double _friction;

    public void Launch(double initialVelocity, double friction)
    {
        Velocity = initialVelocity;
        _friction = friction;
        Position = 0;
    }

    public void Tick(float deltaTime)
    {
        if (Velocity <= 0) return;

        double acceleration = -_friction * GRAVITY;

        Velocity += acceleration * deltaTime;

        if (Velocity < 0)
        {
            Velocity = 0;
        }

        Position += Velocity * deltaTime;
    }
}
