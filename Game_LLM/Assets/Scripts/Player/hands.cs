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

    // Text Prompt for picking up / dropping weapon
    public TextMesh text_UI;

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
        if (transform.childCount <= 0) return null;

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
        if (transform.childCount <= 0 || object_in_hand == null) return;

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

    void pickup_grabbable_object(GameObject grabbable_object)
    {
        // If user has something on hand, cannot pick up item
        if (object_in_hand != null) return;

        // Set object in hand to be the grabbable object
        object_in_hand = grabbable_object;

        // Set parent of grabbable object to be the hand
        grabbable_object.transform.SetParent(gameObject.transform);

        grabbable_object.transform.position = gameObject.transform.position;

        grabbable_object.transform.rotation = gameObject.transform.rotation;
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

        // Get distance between hand and object
        float distance_hand_object = Vector3.Distance(hand_position, grabbed_object_position);

        // Rotate the object in hand
        object_in_hand.transform.rotation = gameObject.transform.rotation;

        // If object in hand is moved too far away, teleport it towards the player's hand
        if (distance_hand_object < max_distance_threshold)
        {
            object_in_hand.transform.position = hand_position;
        }

        // If object in hand is further than current hand position, do not apply velocity to the object in hand
        if (grabbed_object_position == hand_position || Vector2.Dot(grabbed_object_rb.velocity, direction) < 0) return;
        else
        {
            // Accelerate the object in hand towards hand position
            object_in_hand.transform.position = Vector2.Lerp(grabbed_object_position, hand_position, acceleration_value);
        }
    }

    IEnumerator eraseTextAfterSomeTime(float time)
    {
        yield return new WaitForSeconds(time);

        text_UI.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        // Ensures that the texts are facing the correct direction (upwards positive y direction)
        text_UI.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        // Disables hands when player is running
        disable_hands_when_running();

        // If user presses "Q"
        if (Input.GetKey("q") && object_in_hand != null)
        {
            // Tells user that object on hand has been dropped
            text_UI.text = object_in_hand.name + " Dropped!";

            // Erase text_UI after 1 second
            StartCoroutine(eraseTextAfterSomeTime(1));

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

    // If Hand is able to reach the grabbable object
    void OnTriggerStay2D(Collider2D collided_object)
    {
        

        if (collided_object.CompareTag("Grabbable_Object") && object_in_hand == null)
        {
            // Ensures that the texts are facing the correct direction (upwards positive y direction)
            text_UI.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

            text_UI.text = "Press 'E' to pickup " + collided_object.name;

            if (Input.GetKey("e"))
            {
                pickup_grabbable_object(collided_object.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        text_UI.text = "";
    }


    private void FixedUpdate()
    {
        // Move object on hand
        move_grabbable_object();
    }
}
