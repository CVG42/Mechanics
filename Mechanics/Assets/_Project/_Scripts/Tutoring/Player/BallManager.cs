using System;
using UnityEngine;

public class BallManager : Singleton<IBallSource>, IBallSource 
{
    public event Action<double> OnBallStopped;

    [SerializeField] private Transform _ballTransform;
    [SerializeField] private double _mass = 2.0;
    [SerializeField] private double _friction = 0.15;

    private BallPhysics _physics;

    private bool _rolling;

    public double DistanceTravelled => _physics?.Position ?? 0;

    protected override void Awake()
    {
        base.Awake();
        _physics = new BallPhysics();
    }

    private void Start()
    {
        ChargeManager.Source.OnChargeFinished += Launch;
    }

    private void OnDestroy()
    {
        ChargeManager.Source.OnChargeFinished -= Launch;
    }

    private void Update()
    {
        if (!_rolling) return;

        _physics.Tick(Time.deltaTime);

        Vector3 posicion = _ballTransform.position;

        posicion.z = (float)_physics.Position;

        _ballTransform.position = posicion;

        if (_physics.Velocity > 0) return;

        _rolling = false;

        OnBallStopped?.Invoke(_physics.Position);
    }

    private void Launch(double energy)
    {
        double velocity = Math.Sqrt((2 * energy) / _mass); // using kinetic energy equation

        _physics.Launch(velocity, _friction);

        _rolling = true;
    }
}

public interface IBallSource
{
    event Action<double> OnBallStopped;
    double DistanceTravelled { get; }
}
