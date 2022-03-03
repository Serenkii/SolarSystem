using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : GravityObject
{
    [SerializeField]
    float maxFallingSpeed = 100f;
    [SerializeField]
    float rotationThreshold = 5;

    [Header("Camera Controlling")]
    [SerializeField]
    float mouseSensitivity = 1.0f;

    [Header("Player Controlling")]
    [SerializeField]
    float walkingSpeed = 10;

    Vector3 velocity = Vector3.zero;

    public override void FixedUpdate()
    {
        UpdateAccelarationToCelestialObjects();
        PlayerMovement();
    }

    private void Update()
    {
        HandlePlayerInput();
    }

    public void UpdateAccelarationToCelestialObjects()
    {
        Vector3 strongestGravitationalPull = Vector3.zero;
        foreach (CelestialBody celestialBody in CelestialBody.All)
        {
            var acceleration = CalculateAccelerationTowards(celestialBody);
            Rb.AddForce(acceleration, ForceMode.Acceleration);

            if (acceleration.sqrMagnitude > strongestGravitationalPull.sqrMagnitude)
                strongestGravitationalPull = acceleration;
        }

        if (maxFallingSpeed != -1 && Rb.velocity.sqrMagnitude > maxFallingSpeed * maxFallingSpeed)
            Rb.velocity = Rb.velocity.normalized * maxFallingSpeed;

        UpdateRotation(strongestGravitationalPull);
    }

    public void UpdateRotation(Vector3 strongestGravitationalPull)
    {
        if (strongestGravitationalPull.sqrMagnitude > rotationThreshold * rotationThreshold)
        {
            //Rb.rotation = Quaternion.FromToRotation(transform.up, -strongestGravitationalPull.normalized) * Rb.rotation;
            Rb.rotation = Quaternion.Slerp(
                Rb.rotation,
                Quaternion.FromToRotation(transform.up, -strongestGravitationalPull.normalized) * Rb.rotation,
                0.5f);
        }
    }


    public void HandlePlayerInput()
    {
        Vector3 movementInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        velocity = transform.TransformDirection(movementInput.normalized) * walkingSpeed;
    }

    private void PlayerMovement()
    {
        Rb.MovePosition(Rb.position + velocity * Time.fixedDeltaTime);
    }

}
