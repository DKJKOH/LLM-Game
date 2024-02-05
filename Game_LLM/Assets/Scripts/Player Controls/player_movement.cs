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
