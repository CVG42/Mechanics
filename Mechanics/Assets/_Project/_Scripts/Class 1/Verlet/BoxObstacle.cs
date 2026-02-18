using UnityEngine;

namespace VerletIntegration
{
    public class BoxObstacle : MonoBehaviour, ICollider
    {
        public Vector2 size = Vector2.one;

        public bool ResolveCollision(VerletPoint p, float softness, float friction)
        {
            Vector2 half = size * 0.5f;
            Vector2 local = p.position - (Vector2)transform.position;

            if (Mathf.Abs(local.x) > half.x || Mathf.Abs(local.y) > half.y) return false;

            float dx = half.x - Mathf.Abs(local.x);
            float dy = half.y - Mathf.Abs(local.y);

            Vector2 normal;

            if (dx < dy)
            {
                normal = new Vector2(Mathf.Sign(local.x), 0);
            }
            else
            {
                normal = new Vector2(0, Mathf.Sign(local.y));
            }

            Vector2 target = p.position + normal * Mathf.Min(dx, dy);

            p.position = Vector2.Lerp(p.position, target, softness);

            ApplyFriction(p, normal, friction);

            return true;
        }

        private void ApplyFriction(VerletPoint p, Vector2 normal, float friction)
        {
            Vector2 velocity = p.GetVelocity();

            Vector2 tangent = new Vector2(-normal.y, normal.x);

            float tangentialSpeed = Vector2.Dot(velocity, tangent);
            tangentialSpeed *= (1f - friction);

            Vector2 newVelocity = tangent * tangentialSpeed + normal * Mathf.Min(Vector2.Dot(velocity, normal), 0);

            p.SetVelocity(newVelocity);
        }
    }
}