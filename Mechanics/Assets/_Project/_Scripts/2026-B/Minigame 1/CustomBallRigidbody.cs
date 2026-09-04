using UnityEngine;

[RequireComponent (typeof(CircleCollider2D))]
public class CustomBallRigidbody : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField, Min(0f)] private float _acceleration = 15f;
    [SerializeField, Min(0f)] private float _deceleration = 8f;
    [SerializeField, Min(0f)] private float _maxMoveSpeed = 6f;
    [SerializeField, Min(0f)] private float _maxTotalSpeed = 15f;

    [Header("Physics")]
    [SerializeField, Min(0.01f)] private float _mass = 1f;
    [SerializeField, Range(0f, 1f)] private float _restitution = 0.8f;
    [SerializeField, Min(0f)] private float _knockbackMultiplier = 1.25f;

    private CircleCollider2D _collider;
    private Vector2 _velocity;
    private Vector2 _moveInput;
    private bool _isEliminated;

    public Vector2 Velocity => _velocity;
    public float Mass => _mass;
    public float InverseMass => 1f / _mass;
    public float Restitution => _restitution;
    public float KnockbackMultiplier => _knockbackMultiplier;
    public CircleCollider2D Collider => _collider;
    public bool IsEliminated => _isEliminated;

    public Vector2 Position
    {
        get => transform.position;
        set
        {
            transform.position = new Vector3(value.x, value.y, transform.position.z);
        }
    }

    public Vector2 Center
    {
        get
        {
            if (_collider == null)
            {
                return transform.position;
            }

            return transform.TransformPoint(_collider.offset);
        }
    }

    private void Awake()
    {
        _collider = GetComponent<CircleCollider2D>();
    }

    private void OnEnable()
    {
        Minigame1PhysicsManager.Source?.Register(this);
    }

    private void OnDisable()
    {
        Minigame1PhysicsManager.Source?.Unregister(this);
    }

    public void SetMoveInput(Vector2 input)
    {
        if (_isEliminated)
        {
            _moveInput = Vector2.zero;
            return;
        }

        _moveInput = Vector2.ClampMagnitude(input, 1f);
    }

    public void SimulateVelocity(float deltaTime)
    {
        if (_isEliminated) return;

        Vector2 targetVelocity = _moveInput * _maxMoveSpeed;
        float acceleration;

        if (_moveInput.sqrMagnitude > 0.001f)
        {
            acceleration = _acceleration;
        }
        else
        {
            acceleration = _deceleration;
        }

        _velocity = Vector2.MoveTowards(_velocity, targetVelocity, acceleration * deltaTime);
    }

    public void Integrate(float deltaTime)
    {
        if (_isEliminated) return;

        Position += _velocity * deltaTime;
    }

    public void ApplyImpulse(Vector2 impulse)
    {
        if (_isEliminated) return;

        _velocity += impulse * InverseMass;
        _velocity = Vector2.ClampMagnitude(_velocity, _maxTotalSpeed);
    }

    public void Translate(Vector2 displacement)
    {
        if (_isEliminated)
        {
            return;
        }

        Position += displacement;
    }

    public void SetVelocity(Vector2 velocity)
    {
        _velocity = Vector2.ClampMagnitude(velocity, _maxTotalSpeed);
    }

    public void Eliminate()
    {
        if (_isEliminated) return;

        _isEliminated = true;

        _moveInput = Vector2.zero;
        _velocity = Vector2.zero;

        _collider.enabled = false;
    }
}
