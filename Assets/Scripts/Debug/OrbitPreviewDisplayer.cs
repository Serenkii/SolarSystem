using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class OrbitPreviewDisplayer : MonoBehaviour
{

    [SerializeField] int simulationSteps = 3000;
    [SerializeField] float simulationTimeStep = 0.1f;

    [SerializeField] bool relativeLines = false;
    [SerializeField] CelestialBody relativeCenter = null;

    private void Update()
    {
        if (Application.isPlaying)
            return;


        DrawLines();
        //Debug.Log("Update was called!");
    }

    private void DrawLines()
    {
        CelestialBody[] celestialBodies = FindObjectsOfType<CelestialBody>();
        VirtualBody[] bodies = new VirtualBody[celestialBodies.Length];

        for (int i = 0; i < bodies.Length; i++)
        {
            bodies[i] = new VirtualBody(celestialBodies[i]);
        }

        for (int step = 0; step < simulationSteps; step++)
        {
            // Update velocities
            for (int i = 0; i < bodies.Length; i++)
            {
                bodies[i].velocity += bodies[i].CalculateTotalAccelearation(bodies) * simulationTimeStep;
            }
            // Update positions and draw lines
            for (int i = 0; i < bodies.Length; i++)
            {
                Vector3 newPos = bodies[i].position + (bodies[i].velocity * simulationTimeStep);
                Debug.DrawLine(bodies[i].position, newPos, Color.red);
                bodies[i].position = newPos;
            }
        }
    }

    private Vector3 CalculateAccelaration(int indexAttracted, VirtualBody[] bodies)
    {

        throw new NotImplementedException();
        //return Vector3.zero;
    }

    public class VirtualBody
    {
        public Vector3 position;
        public Vector3 velocity;
        public float mass;

        public VirtualBody(CelestialBody celestialBody)
        {
            this.position = celestialBody.transform.position;
            this.velocity = celestialBody.InitialVelocity;
            this.mass = celestialBody.Mass;
        }

        public Vector3 CalculateTotalAccelearation(VirtualBody[] bodies)
        {
            Vector3 force = Vector3.zero;
            foreach (VirtualBody virtualBody in bodies)
            {
                force = force + CalculateAccelerationTowards(virtualBody);
            }
            return force;
        }

        public Vector3 CalculateAccelerationTowards(VirtualBody attractor)
        {
            if (this == attractor)
                return Vector3.zero;

            Vector3 vectorToAttractor = attractor.position - this.position;
            float distanceSqr = vectorToAttractor.sqrMagnitude;
            vectorToAttractor.Normalize();

            float strength = Universe.G * (attractor.mass / distanceSqr);

            return vectorToAttractor * strength;
        }
    }
}
