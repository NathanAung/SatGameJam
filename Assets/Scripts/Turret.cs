using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] GameObject target;
    private Transform targetPos;
    [SerializeField] Transform baseT;
    [SerializeField] Transform barrelT;
    [SerializeField] private float rotationSpeed = 5f;


    // Start is called before the first frame update
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        if (targetPos != null)
        {
            LookAtTarget();
        }
    }


    private void LookAtTarget()
    {
        // ROTATE BASE (Y-Axis)
        Vector3 baseDirection = targetPos.position - baseT.position;
        baseDirection.y = 0f; // ignore vertical component for base rotation
        if (baseDirection != Vector3.zero)
        {
            Quaternion baseTargetRotation = Quaternion.LookRotation(baseDirection);
            baseT.rotation = Quaternion.Slerp(baseT.rotation, baseTargetRotation, rotationSpeed * Time.deltaTime);
        }

        // ROTATE BARREL (X-Axis)
        Vector3 barrelDirection = targetPos.position - barrelT.position;
        Quaternion barrelTargetRotation = Quaternion.LookRotation(barrelDirection);
        // Only rotate around the local X-axis (pitch)
        Vector3 localBarrelEuler = barrelT.localEulerAngles;
        float pitch = barrelTargetRotation.eulerAngles.x;
        // Normalize pitch to avoid gimbal lock issues
        if (pitch > 180) pitch -= 360;
        localBarrelEuler.x = Mathf.LerpAngle(localBarrelEuler.x, pitch, rotationSpeed * Time.deltaTime);
        barrelT.localEulerAngles = localBarrelEuler;
    }
}
