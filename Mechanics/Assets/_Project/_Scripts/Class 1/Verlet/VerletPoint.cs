using Unity.Android.Gradle.Manifest;
using UnityEngine;

public class VerletPoint
{
    public Vector2 position;
    public bool locked;

    private Vector2 _previousPosition;
    
    public VerletPoint(Vector2 startPos, bool locked)
    {
        this.position = startPos;
        this._previousPosition = startPos;
        this.locked = locked;
    }

    public void Integrate(Vector2 gravity, float dt, float damping)
    {
        if (locked) return;

        Vector2 velocity = (position - _previousPosition) * damping;
        float maxSpeed = 6f;
        velocity = Vector2.ClampMagnitude(velocity, maxSpeed);

        _previousPosition = position;
        position += velocity + gravity * dt * dt;
    }

    public Vector2 GetVelocity()
    {
        return position - _previousPosition;
    }

    public void SetVelocity(Vector2 newVelocity)
    {
        _previousPosition = position - newVelocity;
    }
}
