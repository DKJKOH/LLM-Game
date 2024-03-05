using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class doors : MonoBehaviour
{
    // Get hingejoint component of object
    private HingeJoint2D door_joint;

    private Vector2 closed_pos;
    private Quaternion closed_rot;

    // Text UI is to display instructions to player to close door
    public TextMesh text_UI;

    public int openDoorForce;

    private bool withinDoorRange;


    Vector3 playerPosition;
    // Start is called before the first frame update
    void Start()
    {
        // Get hingejoint component
        door_joint = GetComponent<HingeJoint2D>();

        // Save the closed position and rotational value of closed door position
        closed_pos = gameObject.transform.position;
        closed_rot = gameObject.transform.rotation;


        withinDoorRange = false;
    }

    // Update is called once per frame
    void Update()
    {
        // If door joint angle is close to +/- 6 degree or door is opening
        if (-6f < door_joint.jointAngle && door_joint.jointAngle < 6f)
        {
            // no motor foce
            door_joint.motor = new JointMotor2D { motorSpeed = 0 * 1f, maxMotorTorque = 1000f };

            // Rotates and transforms the door into the closed door position (To make it seem like the door is closed to players)
            gameObject.transform.rotation = closed_rot;
            gameObject.transform.position = closed_pos;
        }

        if (withinDoorRange)
        {
            // Ensures that the texts are facing the correct direction (upwards positive y direction)
            text_UI.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

            // If door is not fully closed
            if (door_joint.jointAngle > 5f || door_joint.jointAngle < -5)
            {
                // Tells user that 'E' closes the door
                text_UI.text = "'E' - Close Door";

                // If user presses "e" and the door is NOT currently closed
                if (Input.GetKeyDown("e"))
                {
                    // Close Door
                    CloseDoor();
                }
            }
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

        // Start door slam
        gameObject.GetComponent<door_sounds>().DoorSlamSound();
    }


    // Checks if player touches door 
    void OnTriggerStay2D(Collider2D collided_object)
    {
        if (collided_object.CompareTag("Player"))
        {
            withinDoorRange = true;

            playerPosition = collided_object.transform.position;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        withinDoorRange = false;

        // Remove open / close door text
        text_UI.text = "";
    }
}
