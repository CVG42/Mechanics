using VerletIntegration;

public interface ICollider
{
    bool ResolveCollision(VerletPoint point, float softness, float friction);
}
