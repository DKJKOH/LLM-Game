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
    public float acceleration_value = 10;
    public float maxDistance = 5;

    // Start is called before the first frame update
    void Start()
    {
        // Get player animator (Running / Walking / Idle / Bored walking thing)
        player_animator = gameObject.transform.parent.GetComponent<Animator>();

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
        
        // Get the held object
        GameObject holding_object = gameObject.transform.GetChild(0).gameObject;

        // If user has stamina
        if (gameObject.transform.parent.GetComponent<player_movement>().Stamina > 0  
            // If user presses shift
            && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            // If held object is currently not performing any actions
            && holding_object.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("idle")
            )
        {
            // Disable weapon on hands when running
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
        if (gameObject.transform.childCount <= 0) return;

        // Find hand position
        Vector2 hand_position = gameObject.transform.position;

        // Grabbed object position
        Vector2 grabbed_object_position = gameObject.transform.GetChild(0).position;

        // Grabbed object rigidbody
        Rigidbody2D grabbed_object_rb = gameObject.transform.GetChild(0).GetComponent<Rigidbody2D>();

        // If held object is too far from hand
        if (Vector2.Distance(hand_position, grabbed_object_position) > maxDistance)
        {
            // Move the held object towards the hand position
            gameObject.transform.GetChild(0).position = hand_position;
        }

        // Rotate the object in hand
        gameObject.transform.GetChild(0).rotation = gameObject.transform.rotation;

        // Accelerate the object in hand towards hand position
        grabbed_object_rb.velocity += (hand_position - grabbed_object_position) * acceleration_value * Time.deltaTime;
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
