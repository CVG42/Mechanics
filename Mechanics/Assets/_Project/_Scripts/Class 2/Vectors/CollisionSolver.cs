using System.Linq;
using UnityEngine;
using VerletIntegration;

public class CollisionSolver : MonoBehaviour
{
    public float softness = 1.0f;
    public float friction = 0f;

    private ICollider[] _obstacles;
    private CircleObject _circle;

    private void Start()
    {
        _circle = GetComponent<CircleObject>();
        _obstacles = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<ICollider>().ToArray();
    }

    private void Update()
    {
        VerletPoint point = _circle.GetPoint();

        Vector2 circleNormal = Vector2.zero;
        bool collided = false;

        for(int i = 0; i < 4;  i++)
        {
            foreach (var obstacle in _obstacles)
            {
                if (obstacle.ResolveCollision(point, softness, friction))
                {
                    collided = true;
                    Vector2 normal = GetCollisionNormal(obstacle);

                    circleNormal.x += normal.x;
                    circleNormal.y += normal.y;
                }
            }
        }

        if (collided)
        {
            circleNormal = CustomEquations.NormalizeVector(circleNormal);
            Vector2 velocity = point.GetVelocity();
            float dot = CustomEquations.DotProduct(velocity, circleNormal);

            if (dot < 0)
            {
                GetComponent<CustomBounce>()?.ApplyBounce(circleNormal);
            }
        }
    }

    public Vector2 GetCollisionNormal(ICollider obstacle)
    {
        if (obstacle is BoxObstacle box) return box.LastNormal;

        return Vector2.up;
    }
}
