using UnityEngine;

[RequireComponent (typeof(Collider2D))]
public class BallArena : MonoBehaviour
{
    private Collider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    public bool Contains(CustomBallRigidbody body)
    {
        if (body == null)
        {
            return false;
        }

        return _collider.OverlapPoint(body.Center);
    }
}
