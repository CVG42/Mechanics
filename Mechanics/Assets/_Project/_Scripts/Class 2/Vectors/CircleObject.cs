using UnityEngine;

public class CircleObject : MonoBehaviour
{
    public Vector2 initialVelocity = new Vector2(5f, 2f);
    public Vector2 gravity = Vector2.zero;
    public float damping = 0.999f;

    private VerletPoint _point;

    private void Start()
    {
        _point = new VerletPoint(transform.position, false);
        _point.SetVelocity(initialVelocity);
    }

    private void Update()
    {
        float dt = Time.deltaTime;
        _point.Integrate(gravity, dt, damping);

        transform.position = _point.position;
        DrawVector();
    }

    public VerletPoint GetPoint()
    {
        return _point;
    }

    private void DrawVector()
    {
        Vector2 velocity = _point.GetVelocity();
        Debug.DrawLine(transform.position, (Vector2)transform.position + velocity, Color.yellow);
    }
}
