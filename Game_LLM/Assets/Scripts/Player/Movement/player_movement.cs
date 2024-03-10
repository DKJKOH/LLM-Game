using System;
// Used for coroutine functionality
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor;
using UnityEngine;

// For stamina bar UI
using UnityEngine.UI;

public class player_movement : MonoBehaviour
{
    // Stamina Bar
    public Image Stamina_Bar;

    // Maximum stamina and current stamina values
    public float Max_Stamina, Stamina;

    // What is the cost of running?
    public float Run_Cost;

    // Coroutine to recharge stamina over time
    private Coroutine Recharge_Stamina;

    // How fast does the stamina recharge when not running
    public float Recharge_Stamina_Rate;

    public GameObject GPT_UI, Shop_UI, Pause_UI;

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
        bool messageClosed = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("idle");

        // If there are no popups or chat box open or shop open or pause menue is open
        if (messageClosed && !GPT_UI.activeSelf && !Shop_UI.activeSelf && !Pause_UI.activeSelf)
        {
            // Retrieve horizontal and vertical inputs
            HorizontalInput = Input.GetAxisRaw("Horizontal");
            VerticalInput = Input.GetAxisRaw("Vertical");
        }
        else
        {
            // Retrieve horizontal and vertical inputs
            HorizontalInput = 0f;
            VerticalInput = 0f;
        }
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
        if (gameObject.GetComponent<Rigidbody2D>().velocity != Vector2.zero && playerMovement != Vector2.zero)
        {
            // Set Walking to true to activate walking animation
            gameObject.GetComponent<Animator>().SetBool("Walking", true);

            // If user is pressing shift
            if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Stamina > 0)
            {
                // Set Running to true to start running animation
                gameObject.GetComponent<Animator>().SetBool("Running", true);

                // Reduce current amount of stamina
                Stamina -= Run_Cost * Time.deltaTime;

                // Ensure that stanima is not lesser than 0
                if (Stamina < 0) Stamina = 0;

                // fill Amount controls the size of the current stamina left
                Stamina_Bar.fillAmount = Stamina / Max_Stamina;

                // If there is a coroutine happening
                if (Recharge_Stamina != null) StopCoroutine(Recharge_Stamina);

                // Start coroutine 
                Recharge_Stamina = StartCoroutine(Charge_Stamina());
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

    private IEnumerator Charge_Stamina()
    {
        yield return new WaitForSeconds(1f);

        // If stamina bar is not full
        while (Stamina < Max_Stamina)
        {
            Stamina += Recharge_Stamina_Rate / 10f;

            // Ensures that stamina does not exceed maximum stamina
            if (Stamina > Max_Stamina)
            {
                Stamina = Max_Stamina;
            }

            // Resize stamina bar
            Stamina_Bar.fillAmount = Stamina / Max_Stamina;

            // Wait for 1/10th of a second, before the while loop starts again.
            yield return new WaitForSeconds(.1f);
        }
    }

    private void FixedUpdate()
    {
        // If User presses Shift key
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Stamina > 0)
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
