using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CelestialBody : GravityObject
{
    private static List<CelestialBody> celestialBodies = new List<CelestialBody>();

    private Vector3 velocity;
    [SerializeField] private Vector3 initialVelocity;

    private void Start()
    {
        velocity = initialVelocity;
    }

    public override void FixedUpdate()
    {
        UpdateVelocity();
        UpdatePosition();
    }

    public void UpdateVelocity()
    {
        //velocity = velocity + ((base.CalculateTotalForce() / Mass) * Time.fixedDeltaTime);
        velocity = velocity + (base.CalculateTotalAccelearation() * Time.fixedDeltaTime);
    }

    public void UpdatePosition()
    {
        Rb.MovePosition(Rb.position + velocity * Time.fixedDeltaTime);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Rb.isKinematic = true;

        celestialBodies.Add(this);        
    }

    private void OnDisable()
    {
        celestialBodies.Remove(this);
    }


    public static List<CelestialBody> All { get { return celestialBodies; } }
}
