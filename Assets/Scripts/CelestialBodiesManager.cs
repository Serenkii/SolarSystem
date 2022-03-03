using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialBodiesManager : MonoBehaviour
{
    private void FixedUpdate()
    {
        foreach (CelestialBody celestialBody in CelestialBody.All) {
            celestialBody.UpdateVelocity();
        }
        foreach (CelestialBody celestialBody in CelestialBody.All)
        {
            celestialBody.UpdatePosition();
        }
    }
}
