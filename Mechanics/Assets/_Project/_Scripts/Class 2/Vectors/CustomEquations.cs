using UnityEngine;

public static class CustomEquations
{
    public static float DotProduct(Vector2 a, Vector2 b)
    {
        return a.x * b.x + a.y * b.y;
    }

    public static float VectorMagnitude(Vector2 vector)
    {
        return Mathf.Sqrt(vector.x * vector.x + vector.y * vector.y);
    }

    public static Vector2 Bounce(Vector2 velocity, Vector2 normal, float restitution = 1f)
    {
        float dot = DotProduct(velocity, normal);

        float scale = (1f + restitution) * dot;

        Vector2 result;

        result.x = velocity.x - scale * normal.x;
        result.y = velocity.y - scale * normal.y;

        return result;
    }

    public static Vector2 NormalizeVector(Vector2 vector)
    {
        float magnitude = VectorMagnitude(vector);

        if (magnitude == 0f) return Vector2.zero;

        Vector2 result;

        result.x = vector.x / magnitude;
        result.y = vector.y / magnitude;

        return result;
    }
}
