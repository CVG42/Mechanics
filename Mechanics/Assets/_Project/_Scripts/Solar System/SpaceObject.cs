using System;
using System.Collections.Generic;
using UnityEngine;

public class SpaceObject : GravitationalForces
{

    [SerializeField] private bool _onlyUseGravity = true;

    [SerializeField] private double _mass = 0;

    public bool OnlyUseGravity
    {
        get { return _onlyUseGravity; }
        set { _onlyUseGravity = value; }
    }

    public static readonly List<SpaceObject> All = new();

    private void OnEnable()
    {
        if (!All.Contains(this))
        { 
            All.Add(this); 
        }
    }

    private void OnDisable()
    {
        All.Remove(this);
    }

    private void FixedUpdate()
    {
        if (!_onlyUseGravity) return;

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
            Gizmos.color = Color.gray;

            foreach (var spaceObject in _spaceObjects)
            {
                Gizmos.DrawLine(transform.position, spaceObject.transform.position);
            }
        }
    }

    public double GetGravitationalPullForce(double distanceMetersFromObject)
    {
        return Constants.GRAVITATIONAL_CONSTANT * _mass / Math.Pow(distanceMetersFromObject, 2);
    }

    public double GetVelocityForCircularOrbit(double distanceMetersFromObject)
    {
        return Math.Sqrt(Constants.GRAVITATIONAL_CONSTANT * _mass / distanceMetersFromObject);
    }

    public double GetEscapeVelocity(double distanceMetersFromObject)
    {
        return Math.Sqrt(2 * Constants.GRAVITATIONAL_CONSTANT * _mass / distanceMetersFromObject);
    }
}
