using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class hands : MonoBehaviour
{
    // Player animator
    private Animator player_animator;

    // Current game object in hand
    private GameObject object_in_hand;

    // Acceleration value
    public float acceleration_value;

    // Max distance the gun can be away from player
    public float max_distance_threshold; 

    // Start is called before the first frame update
    void Start()
    {
        // Get player animator (Running / Walking / Idle / Bored walking thing)
        player_animator = gameObject.transform.parent.GetComponent<Animator>();

        // Set inital acceleration value
        acceleration_value = 10;

        // Finds object in hand
        object_in_hand = find_item_on_hand();
    }

    //Function which checks for item the player is currently holding(TRY NOT TO RUN THIS IN UPDATE FUNCTION)
    GameObject find_item_on_hand()
    {
        // If player is not holding anything, do not run this function
        if (gameObject.transform.childCount <= 0) return null;

        // Cycle through all childrens in current game object to find the item the player is holding
        foreach (Transform child in gameObject.transform)
        {
            // Find the object in hand which has the "Grabbable_Object" tag and returns the game object
            if (child.tag == "Grabbable_Object") return child.gameObject;
        }

        return null;
    }


    /* Function which disables player's hands when they are running */
    void disable_hands_when_running()
    {
        // If player is not holding anything, do not run this function
        if (transform.childCount <= 0) return;
        

        GameObject holding_object = gameObject.transform.GetChild(0).gameObject;

        if (gameObject.transform.parent.GetComponent<player_movement>().Stamina > 0  
            && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            && holding_object.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("idle")
            )
        {
            // Disable weapon on hands
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
        // Player can to shoot / aim when walking
        
        if(Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
        {
           
            // Enable weapon on hand
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
    
    void move_grabbable_object()
    {
        // Do not do anything if there is nothing on hand
        if (gameObject.transform.childCount <= 0)
        {
            return;
        }

        // Find hand position
        Vector2 hand_position = gameObject.transform.position;

        // Grabbed object position
        Vector2 grabbed_object_position = gameObject.transform.GetChild(0).position;
        Rigidbody2D grabbed_object_rb = gameObject.transform.GetChild(0).GetComponent<Rigidbody2D>();

        Vector2 direction = (hand_position - grabbed_object_position).normalized;

        // Get distance between hand and object
        float distance_hand_object = Vector3.Distance(hand_position, grabbed_object_position);

        // Rotate the object in hand
        gameObject.transform.GetChild(0).rotation = gameObject.transform.rotation;

        // If object in hand is moved too far away, teleport it towards the player's hand
        if (distance_hand_object < max_distance_threshold)
        {
            gameObject.transform.GetChild(0).position = hand_position;
        }

        // If object in hand is further than current hand position, do not apply velocity to the object in hand
        if (grabbed_object_position == hand_position || Vector2.Dot(grabbed_object_rb.velocity, direction) < 0) return;
        else
        {
            // Accelerate the object in hand towards hand position
            gameObject.transform.GetChild(0).position = Vector2.Lerp(grabbed_object_position, hand_position, acceleration_value);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.childCount > 0)
        {
            // Disables hands when player is running
            disable_hands_when_running();

            // Get current mouse position 
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Get direction of object towards cursor
            Vector3 direction = mousePos - transform.position;

            // Convert direction to angle
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Rotate object about z axis
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        }
    }

    private void FixedUpdate()
    {
        // Do not do anything if there is nothing on hand
        if (gameObject.transform.childCount > 0)
        {
            // Move object on hand
            move_grabbable_object();
        }
    }
}
