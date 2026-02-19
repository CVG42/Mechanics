using UnityEngine;

namespace VerletIntegration
{
    public class CircleObstacle : MonoBehaviour, ICollider
    {
        public float radius = 1f;

        public Vector2 Position => transform.position;

        public bool ResolveCollision(VerletPoint p, float softness, float friction)
        {
            Vector2 delta = p.position - Position;
            float distance = delta.magnitude;

            if (distance >= radius) return false;

            Vector2 normal = delta / distance;
            Vector2 target = Position + normal * radius;

            p.position = Vector2.Lerp(p.position, target, softness);

            ApplyFriction(p, normal, friction);

            return true;
        }

        private void ApplyFriction(VerletPoint p, Vector2 normal, float friction)
        {
            Vector2 velocity = p.GetVelocity();

            Vector2 tangent = new Vector2(-normal.y, normal.x);

            float tangentialSpeed = Vector2.Dot(velocity, tangent);
            tangentialSpeed *= 1f - friction;

            Vector2 newVelocity = tangent * tangentialSpeed + normal * Mathf.Min(Vector2.Dot(velocity, normal), 0);

            p.SetVelocity(newVelocity);
        }
    }
}