using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doors : MonoBehaviour
{
    // Get hingejoint component of object
    private HingeJoint2D door_joint;

    private Vector2 closed_pos;
    private Quaternion closed_rot;

    // Start is called before the first frame update
    void Start()
    {
        // Get hingejoint component
        door_joint = GetComponent<HingeJoint2D>();

        closed_pos = gameObject.transform.position;
        closed_rot = gameObject.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(hingeJoint.jointAngle);

        if (-1f < door_joint.jointAngle && door_joint.jointAngle < 1f)
        {
            Debug.Log("Appleds");

            // Apply torque to rotate the door
            door_joint.motor = new JointMotor2D { motorSpeed = 0 * 1f, maxMotorTorque = 1000f };

            gameObject.transform.rotation = closed_rot;
            gameObject.transform.position = closed_pos; 
            
        }
    }

    private void CloseDoor()
    {
        // Set the target angle to 0
        float targetAngle = 0f;

        // Get the current angle of the door
        float currentAngle = door_joint.jointAngle;

        // Calculate the rotation needed to reach angle 0
        float rotationNeeded = targetAngle - currentAngle;

        // Apply torque to rotate the door
        door_joint.motor = new JointMotor2D { motorSpeed = rotationNeeded * 3.5f, maxMotorTorque = 1000f };
    }

    // Checks if player touches door 
    void OnTriggerStay2D(Collider2D collided_object)
    {
        if (collided_object.CompareTag("Player"))
        {
            // Ensures that the texts are facing the correct direction (upwards positive y direction)
            //text_UI.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

            //text_UI.text = "Press 'E' to pickup " + collided_object.name;

            // If user presses "e" and the door is NOT currently closed
            if (Input.GetKey("e"))
            {
                // Close Door
                CloseDoor();
            }
        }
    }
}
