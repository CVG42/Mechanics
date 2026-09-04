using System.Collections.Generic;
using UnityEngine;

public class Minigame1PhysicsManager : Singleton<IMinigame1Source>, IMinigame1Source
{
    [Header("Simulation")]
    [SerializeField, Range(1, 8)] private int _substeps = 2;
    [SerializeField, Range(1, 8)] private int _solverIterations = 2;

    [Header("Collision")]
    [SerializeField, Range(0f, 1f)] private float _positionCorrection = 0.8f;
    [SerializeField, Min(0f)] private float _penetrationSlop = 0.001f;

    [Header("Arena")]
    [SerializeField] private BallArena _arena;

    private readonly List<CustomBallRigidbody> _bodies = new();

    protected override void Awake()
    {
        CustomBallRigidbody[] bodies = FindObjectsByType<CustomBallRigidbody>(FindObjectsSortMode.None);

        foreach (CustomBallRigidbody body in bodies)
        {
            Register(body);
        }

        base.Awake();
    }

    private void FixedUpdate()
    {
        float deltaTime = Time.fixedDeltaTime / _substeps;

        for (int step = 0; step < _substeps; step++)
        {
            SimulateStep(deltaTime);
        }
    }

    private void SimulateStep(float deltaTime)
    {
        SimulateVelocities(deltaTime);
        IntegrateBodies(deltaTime);

        Physics2D.SyncTransforms();

        for (int iteration = 0; iteration < _solverIterations; iteration++)
        {
            ResolveBodyCollisions();
            Physics2D.SyncTransforms();
        }

        CheckArena();
    }

    private void SimulateVelocities(float deltaTime)
    {
        foreach (CustomBallRigidbody body in _bodies)
        {
            if (body == null || body.IsEliminated)
            {
                continue;
            }

            body.SimulateVelocity(deltaTime);
        }
    }

    private void IntegrateBodies(float deltaTime)
    {
        foreach (CustomBallRigidbody body in _bodies)
        {
            if (body == null || body.IsEliminated)
            {
                continue;
            }

            body.Integrate(deltaTime);
        }
    }

    private void ResolveBodyCollisions()
    {
        for (int i = 0; i < _bodies.Count; i++)
        {
            CustomBallRigidbody ball = _bodies[i];

            if (ball == null || ball.IsEliminated || !ball.Collider.enabled)
            {
                continue;
            }

            for (int j = i + 1; j < _bodies.Count; j++)
            {
                CustomBallRigidbody ball2 = _bodies[j];

                if (ball2 == null || ball2.IsEliminated || !ball2.Collider.enabled)
                {
                    continue;
                }

                ResolveCollision(ball, ball2);
            }
        }
    }

    private void ResolveCollision(CustomBallRigidbody ballA, CustomBallRigidbody ballB)
    {
        ColliderDistance2D distance = Physics2D.Distance(ballA.Collider, ballB.Collider);

        if (!distance.isValid || !distance.isOverlapped) return;

        Vector2 delta = ballB.Center - ballA.Center;
        Vector2 normal;

        if (delta.sqrMagnitude > 0.000001f)
        {
            normal = delta.normalized;
        }
        else
        {
            normal = Vector2.right;
        }

        ResolveOverlap(ballA, ballB, normal, distance);
        ResolveImpulse(ballA, ballB, normal);
    }

    private void ResolveOverlap(CustomBallRigidbody ballA, CustomBallRigidbody ballB, Vector2 normal, ColliderDistance2D distance)
    {
        float overlap = Mathf.Max(0f, -distance.distance);
        overlap = Mathf.Max(0f, overlap - _penetrationSlop);

        if (overlap <= 0f)
        {
            return;
        }

        float inverseMassA = ballA.InverseMass;
        float inverseMassB = ballB.InverseMass;
        float totalInverseMass = inverseMassA + inverseMassB;

        if (totalInverseMass <= 0f)
        {
            return;
        }

        Vector2 correction = normal * (overlap * _positionCorrection / totalInverseMass);
        ballA.Translate(-correction * inverseMassA);
        ballB.Translate(correction * inverseMassB);
    }

    private void ResolveImpulse(CustomBallRigidbody ballA, CustomBallRigidbody ballB, Vector2 normal)
    {
        Vector2 relativeVelocity = ballB.Velocity - ballA.Velocity;

        float velocityAlongNormal = Vector2.Dot(relativeVelocity, normal);

        if (velocityAlongNormal >= 0f) return;

        float restitution = Mathf.Min(ballA.Restitution, ballB.Restitution);
        float inverseMassA = ballA.InverseMass;
        float inverseMassB = ballB.InverseMass;
        float impulseMagnitude = -(1f + restitution) * velocityAlongNormal;

        impulseMagnitude /= inverseMassA + inverseMassB;

        float knockback = GetKnockbackMultiplier(ballA, ballB, normal);

        impulseMagnitude *= knockback;

        Vector2 impulse = impulseMagnitude * normal;

        ballA.ApplyImpulse(-impulse);
        ballB.ApplyImpulse(impulse);
    }

    private float GetKnockbackMultiplier(CustomBallRigidbody ballA, CustomBallRigidbody ballB, Vector2 normal)
    {
        float attackA = Mathf.Max(0f, Vector2.Dot(ballA.Velocity, normal));
        float attackB = Mathf.Max(0f, Vector2.Dot(ballB.Velocity, -normal));

        if (attackA >= attackB)
        {
            return ballA.KnockbackMultiplier;
        }

        return ballB.KnockbackMultiplier;
    }

    private void CheckArena()
    {
        if (_arena == null) return;

        foreach (CustomBallRigidbody body in _bodies)
        {
            if (body == null || body.IsEliminated)
            {
                continue;
            }

            if (!_arena.Contains(body))
            {
                body.Eliminate();
            }
        }
    }

    public void Register(CustomBallRigidbody body)
    {
        if (body == null || _bodies.Contains(body)) return;

        _bodies.Add(body);
    }

    public void Unregister(CustomBallRigidbody body)
    {
        _bodies.Remove(body);
    }
}

public interface IMinigame1Source
{
    void Register(CustomBallRigidbody body);
    void Unregister(CustomBallRigidbody body);
}
