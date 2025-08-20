using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackWheele : MonoBehaviour
{
    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentbreakForce;
    private bool isBreaking;
    public bool preserveYRotation;
    // Settings
    [SerializeField] private float motorForce, breakForce, maxSteerAngle;

    // Wheel Colliders
    [SerializeField] private WheelCollider WheelCollider;
    [SerializeField] private WheelCollider WheelCollider2;

    // Wheels
    [SerializeField] private Transform WheelTransform;
    [SerializeField] private Transform WheelTransform2;

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    private void GetInput()
    {
        // Steering Input
        horizontalInput = Input.GetAxis("Horizontal");

        // Acceleration Input
        verticalInput = Input.GetAxis("Vertical");

        // Breaking Input
        isBreaking = Input.GetKey(KeyCode.Space);
    }

    private void HandleMotor()
    {
        WheelCollider.motorTorque = verticalInput * motorForce;
        WheelCollider2.motorTorque = verticalInput * motorForce;
        currentbreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();
    }

    private void ApplyBreaking()
    {
        WheelCollider.brakeTorque = currentbreakForce;
        WheelCollider2.brakeTorque = currentbreakForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        WheelCollider.steerAngle = currentSteerAngle;
        WheelCollider2.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(WheelCollider, WheelTransform);
        UpdateSingleWheel(WheelCollider2, WheelTransform2);
    }
    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;

        // WheelCollider'dan pozisyon ve rotasyon bilgilerini al
        wheelCollider.GetWorldPose(out pos, out rot);

        // Eğer preserveYRotation true ise Y rotasyonunu koru
        if (preserveYRotation)
        {
            rot = Quaternion.Euler(rot.eulerAngles.x, wheelTransform.rotation.eulerAngles.y, rot.eulerAngles.z);
        }

        // Pozisyon ve rotasyonu uygula
        //wheelTransform.rotation = rot;
    }

}
