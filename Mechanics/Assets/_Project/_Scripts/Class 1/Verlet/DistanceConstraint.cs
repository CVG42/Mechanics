using UnityEngine;

namespace VerletIntegration
{
    public class DistanceConstraint
    {
        public VerletPoint pointA;
        public VerletPoint pointB;

        public float resetLenght;

        public DistanceConstraint(VerletPoint a, VerletPoint b, float lenght)
        {
            pointA = a;
            pointB = b;
            resetLenght = lenght;
        }

        public void Solve()
        {
            Vector2 delta = pointB.position - pointA.position;
            float distance = delta.magnitude;

            if (distance == 0) return;

            float difference = (distance - resetLenght) / distance;

            if (!pointA.locked)
            {
                pointA.position += delta * 0.5f * difference;
            }

            if (!pointB.locked)
            {
                pointB.position -= delta * 0.5f * difference;
            }
        }
    }
}