using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Camera will focus on this object / target
    [SerializeField] Transform camera_target;

    // Distance between target and camera
    private float distance = 10;

    // How fast the camera follows the player model
    public float cameraSpeed = 0.1f;

    // Where the camera will move towards
    private Vector3 cameraTargetPosition;

    private void Update()
    {
        // Distance value between camera and object
        distance = Vector3.Distance(transform.position, camera_target.position);
    }

    private void LateUpdate()
    {
        // Calculate the desired camera position
        cameraTargetPosition = camera_target.position - transform.forward * distance;

        // Calculate next camera position - Move the camera towards the target object smoothly using Lerp function
        Vector3 smoothedCameraPosition = Vector3.Lerp(transform.position, cameraTargetPosition, cameraSpeed * Time.deltaTime);

        // Move the camera towards the target object smoothly using Lerp function
        transform.position = smoothedCameraPosition;
    }

}
