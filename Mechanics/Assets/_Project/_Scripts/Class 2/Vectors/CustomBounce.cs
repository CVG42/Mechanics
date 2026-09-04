using UnityEngine;

public class CustomBounce : MonoBehaviour
{
    [SerializeField] private float _restitution = 1f;

    private CircleObject _circle;

    private void Start()
    {
        _circle = GetComponent<CircleObject>();
    }

    public void ApplyBounce(Vector2 normal)
    {
        VerletPoint point = _circle.GetPoint();
        Vector2 velocity = point.GetVelocity();

        normal = CustomEquations.NormalizeVector(normal);

        velocity = CustomEquations.Bounce(velocity, normal, _restitution);

        point.SetVelocity(velocity);
    }
}
