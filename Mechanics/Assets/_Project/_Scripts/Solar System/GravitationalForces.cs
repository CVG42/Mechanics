using System;
using System.Linq;
using UnityEngine;

public class GravitationalForces : MonoBehaviour
{
    /// <summary>
    /// Internal values for velocity type
    /// </summary>
    protected const int FREE_VELOCITY = 0;
    protected const int CIRCULAR_ORBIT_VELOCITY = 1;
    protected const int ESCAPE_ORBIT_VELOCITY = 2;

    protected enum InitialVelocity
    {
        Free = FREE_VELOCITY, // no constraint
        CircularOrbit = CIRCULAR_ORBIT_VELOCITY, // velocity to orbit around closest massive object
        EscapeOrbit = ESCAPE_ORBIT_VELOCITY // minimum velocity to escape completely from planet's gravity (leave orbit from closest massive object)
    }

    [SerializeField] protected InitialVelocity _velocityType; // Calculate velocity of the objet
    [SerializeField] protected double _initialVelocity = 0;
    [SerializeField] protected Vector3 _initialDirection = new Vector3();

    protected Rigidbody _rigidbody;
    protected GameObject[] _spaceObjects;

    protected bool isInitialized = false;

    protected void Initialize()
    {
        _rigidbody = GetComponent<Rigidbody>();

        _spaceObjects = SpaceObject.All.Where(o => o.gameObject != gameObject).Select(o => o.gameObject).ToArray(); // register to objects with space object script

        _initialVelocity = GetVelocity((int)_velocityType);
        _rigidbody.linearVelocity = _initialDirection *(float)_initialVelocity * Constants.METERS_CONVERTION * GameManager.Source.TimeScale / GameManager.Source.SpaceScaleMeters;

        isInitialized = true;
    }

    protected void ApplyGravity()
    {
        var gravityForces = new Vector3();

        foreach (var spaceObject in _spaceObjects)
        {
            gravityForces += GetGravity(spaceObject) * Time.fixedDeltaTime;
        }

        _rigidbody.linearVelocity += gravityForces;
    }

    protected Vector3 GetGravity(GameObject spaceObject)
    {
        var direction = spaceObject.transform.position - transform.position;
        var distance = direction.magnitude * GameManager.Source.SpaceScaleMeters;

        var gravity = spaceObject.GetComponent<SpaceObject>().GetGravitationalPullForce(distance);
        var gravityScaled = (float)(gravity * Math.Pow(GameManager.Source.TimeScale, 2) / GameManager.Source.SpaceScaleMeters);

        return direction.normalized * gravityScaled;
    }

    private double GetVelocity(int velocityType)
    {
        if (_spaceObjects.Length == 0)
        {
            Debug.Log("No spaceObjects found. Needed at least 1 to get velocity");
        }

        var target = _spaceObjects
          .OrderByDescending(o => o.GetComponent<SpaceObject>().GetGravitationalPullForce(
          (o.transform.position - transform.position).magnitude) * GameManager.Source.SpaceScaleMeters)
          .First();

        var distance = (target.transform.position - transform.position).magnitude * GameManager.Source.SpaceScaleMeters;
        var velocity = 0.0; // since it's double we need 0.0

        switch (velocityType)
        {
            case CIRCULAR_ORBIT_VELOCITY:
                velocity = target.GetComponent<SpaceObject>().GetVelocityForCircularOrbit(distance) / 1000 + target.GetComponent<Rigidbody>().linearVelocity.magnitude;
                break;
            case ESCAPE_ORBIT_VELOCITY:
                velocity = target.GetComponent<SpaceObject>().GetEscapeVelocity(distance) / 1000;
                break;
            default:
                Debug.Log("FREE_VELOCITY (" + _velocityType + ")");
                velocity = _initialVelocity;
                break;
        }

        return velocity;
    }
}
