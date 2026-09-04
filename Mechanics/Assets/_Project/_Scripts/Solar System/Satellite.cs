using UnityEngine;

public class Satellite : GravitationalForces
{
    private void FixedUpdate()
    {
        if (!isInitialized)
        {
            Initialize();
        }

        ApplyGravity();
    }

    private void OnDrawGizmosSelected()
    {
        if (_spaceObjects != null)
        {
            Gizmos.color = Color.white;

            foreach (var spaceObject in _spaceObjects)
            {
                Gizmos.DrawLine(transform.position, spaceObject.transform.position);
            }
        }
    }
}
