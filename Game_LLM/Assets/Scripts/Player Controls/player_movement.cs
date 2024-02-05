using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class player_movement : MonoBehaviour
{
    // Values to store horizontal and vertical movement
    float HorizontalInput;
    float VerticalInput;

    // Movement speed (Walking and Running)
    [SerializeField] float walk_speed = 2f;
    [SerializeField] float run_speed = 4f;
    Vector2 playerMovement;

    // Get last mouse position
    Vector3 lastMousePosition;
    float time_of_last_movement;
    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        // Retrieve horizontal and vertical inputs
        HorizontalInput = Input.GetAxisRaw("Horizontal");
        VerticalInput = Input.GetAxisRaw("Vertical");

        // Calculating the direction of input
        playerMovement = new Vector2(HorizontalInput, VerticalInput);

        // Normalize vector to ensure vertical and horizontal inputs are the same
        playerMovement.Normalize();

        // If player moves / rotates
        if (Vector3.Magnitude(Input.mousePosition - lastMousePosition) > 0 || gameObject.GetComponent<Rigidbody2D>().velocity != Vector2.zero)
        {
            // Update time of last movement as current time.
            time_of_last_movement = Time.time;
        }

        // Save current mouse position to last mouse position to compare if the user is afk
        lastMousePosition = Input.mousePosition;

        // If player is afk for a set amount of time
        if ((Mathf.Abs(Time.time - time_of_last_movement)) > 2f)
        {
            // Set bored to true to activate idle animation
            gameObject.GetComponent<Animator>().SetBool("Bored", true);
        }
        else
        {
            // Set bored to false to get out of bored idle animation
            gameObject.GetComponent<Animator>().SetBool("Bored", false);
        }


        // Walking animation stuff

        // If player is moving
        if (gameObject.GetComponent<Rigidbody2D>().velocity != Vector2.zero)
        {
            // Set Walking to true to activate walking animation
            gameObject.GetComponent<Animator>().SetBool("Walking", true);

            // If user is pressing shift
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                // Set Running to true to start running animation
                gameObject.GetComponent<Animator>().SetBool("Running", true);
            }
            else
            {
                // Set Running to false to start walking animation
                gameObject.GetComponent<Animator>().SetBool("Running", false);
            }
        }
        // Player is not moving
        else
        {
            // Stop walking and running animation
            gameObject.GetComponent<Animator>().SetBool("Walking", false);
            gameObject.GetComponent<Animator>().SetBool("Running", false);
        }
    }

    private void FixedUpdate()
    {
        // If User presses Shift key
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            // Character runs
            gameObject.GetComponent<Rigidbody2D>().velocity = playerMovement * run_speed;
        }
        else
        {
            // Character walks
            gameObject.GetComponent<Rigidbody2D>().velocity = playerMovement * walk_speed;
        }


    }
}
