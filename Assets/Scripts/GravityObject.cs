using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class GravityObject : MonoBehaviour
{
    [SerializeField]
    private float mass;
    private Rigidbody rb;
    public float Mass { get { return mass; } private set { this.mass = value; } }
    protected Rigidbody Rb { get { return rb; } }

    protected virtual void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = mass;
        rb.useGravity = false;
    }

    public abstract void FixedUpdate();

    protected virtual Vector3 CalculateTotalForce()
    {
        Vector3 force = Vector3.zero;
        foreach (CelestialBody celestialBody in CelestialBody.All)
        {
            force = force + CalculateForceTowards(celestialBody);
        }
        return force;
    }

    protected virtual Vector3 CalculateForceTowards(CelestialBody attractor)
    {
        if (this == attractor)
            return Vector3.zero;

        Vector3 vectorToAttractor = attractor.transform.position - this.transform.position;
        float distanceSqr = vectorToAttractor.sqrMagnitude;
        vectorToAttractor.Normalize();

        float strength = Universe.G * ((this.Mass * attractor.Mass) / distanceSqr);

        return vectorToAttractor * strength;
    }


    protected virtual Vector3 CalculateTotalAccelearation()
    {
        Vector3 force = Vector3.zero;
        foreach (CelestialBody celestialBody in CelestialBody.All)
        {
            force = force + CalculateAccelerationTowards(celestialBody);
        }
        return force;
    }

    protected virtual Vector3 CalculateAccelerationTowards(CelestialBody attractor)
    {
        if (this == attractor)
            return Vector3.zero;

        Vector3 vectorToAttractor = attractor.transform.position - this.transform.position;
        float distanceSqr = vectorToAttractor.sqrMagnitude;
        vectorToAttractor.Normalize();

        float strength = Universe.G * (attractor.Mass / distanceSqr);

        return vectorToAttractor * strength;
    }

}
