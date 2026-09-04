using UnityEngine;

public class BallPhysicsController : MonoBehaviour
{
    [SerializeField] private Transform _ballVisual;

    [Header("Physics")]
    [SerializeField] private double _mass = 2;
    [SerializeField] private double _friction = 0.15;

    private BallPhysics _physics;

    public bool IsRolling => _physics != null && _physics.Velocity > 0;

    public double DistanceTravelled => _physics?.Position ?? 0;

    private void Awake()
    {
        _physics = new BallPhysics();
    }

    public void Launch(double energy)
    {
        double velocity = System.Math.Sqrt((2 * energy) / _mass); // using kinetic energy equation

        _physics.Launch(velocity, _friction);
    }

    private void Update()
    {
        if (_physics == null) return;

        _physics.Tick(Time.deltaTime);

        Vector3 pos = _ballVisual.position;

        pos.z = (float)_physics.Position;

        _ballVisual.position = pos;
    }
}
