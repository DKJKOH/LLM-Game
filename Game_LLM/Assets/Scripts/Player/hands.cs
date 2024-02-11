using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
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

        // Finds object in hand
        object_in_hand = find_item_on_hand();

        // Set inital acceleration value
        acceleration_value = 10;
    }

    // Function which checks for item the player is currently holding (TRY NOT TO RUN THIS IN UPDATE FUNCTION)
    GameObject find_item_on_hand()
    {
        // If player is not holding anything, do not run this function
        if (transform.childCount <= 1) return null;

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
        if (transform.childCount <= 1 || object_in_hand == null) return;

        // Player is unable to shoot / aim when running
        if (player_animator.GetBool("Running"))
        {
            // Disable weapon on hand
            object_in_hand.SetActive(false);
        }
        // Player can to shoot / aim when walking
        else
        {
            // Enable weapon on hand
            object_in_hand.SetActive(true);
        }
    }

    // This function removes the parent 
    void drop_grabbable_object(GameObject grabbable_object)
    {
        // If nothing is on hand, do not do anything
        if (grabbable_object == null) return;

        // Remove parent of object
        grabbable_object.transform.SetParent(null);

        // Set remove current grabbable_object
        object_in_hand = null;
    }
    
    void move_grabbable_object()
    {
        // Do not do anything if there is nothing on hand
        if (object_in_hand == null) return;

        // Find hand position
        Vector2 hand_position = gameObject.transform.position;

        // Grabbed object position
        Vector2 grabbed_object_position = object_in_hand.transform.position;
        Rigidbody2D grabbed_object_rb = object_in_hand.GetComponent<Rigidbody2D>();

        Vector2 direction = (hand_position - grabbed_object_position).normalized;

        float distance_hand_object = Vector3.Distance(hand_position, grabbed_object_position);

        object_in_hand.transform.rotation = gameObject.transform.rotation;


        // If object in hand is further than current hand position, do not apply velocity to the object in hand
        //if (Vector2.Dot(grabbed_object_rb.velocity, direction) < 0) return;

        if (distance_hand_object < max_distance_threshold)
        {
            object_in_hand.transform.position = hand_position;
        }

        if (grabbed_object_position == hand_position || Vector2.Dot(grabbed_object_rb.velocity, direction) < 0) return;
        else
        {
            // Accelerate the object in hand towards hand position
            //grabbed_object_rb.AddForce(grabbed_object_rb.mass * (acceleration_value * direction));

            object_in_hand.transform.position = Vector2.Lerp(grabbed_object_position, hand_position, acceleration_value);
        }


        // Find player object and get its velocity
        //Vector2 player_velocity = GameObject.Find("Player").GetComponent<Rigidbody2D>().velocity;

        // Get current velocity and give it to the dynamic weapon object
        //object_in_hand.GetComponent<Rigidbody2D>().velocity = player_velocity;
    }

    // Update is called once per frame
    void Update()
    {
        // Disables hands when player is running
        disable_hands_when_running();

        // If user presses "Q"
        if (Input.GetKeyDown("q"))
        {
            // Drop object on hand
            drop_grabbable_object(find_item_on_hand());
        }

        // Get current mouse position 
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Get direction of object towards cursor
        Vector3 direction = mousePos - transform.position;

        // Convert direction to angle
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        // Rotate object about z axis
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));


    }

    private void FixedUpdate()
    {
        // Move object on hand
        move_grabbable_object();
    }
}
