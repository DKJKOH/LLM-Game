using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickup_drop : MonoBehaviour
{
    // Current game object in hand
    private GameObject object_in_hand;

    // Text Prompt for picking up / dropping weapon
    public TextMesh text_UI;

    private GameObject current_main_hand;

    public GameObject left_hand, right_hand;

    // Start is called before the first frame update
    void Start()
    {
        // Finds object in hand
        current_main_hand = right_hand;
    }

    // Function which checks for item the player is currently holding (TRY NOT TO RUN THIS IN UPDATE FUNCTION)
    GameObject find_hand_with_object()
    {
        //// If player is not holding anything, do not run this function
        //if (hands.transform.childCount <= 0) return null;

        //// Cycle through all childrens in current game object to find the item the player is holding
        //foreach (Transform child in hands.transform)
        //{
        //    // Find the object in hand which has the "Grabbable_Object" tag and returns the game object
        //    if (child.tag == "Grabbable_Object") return child.gameObject;
        //}

        //return null;

        if (left_hand.transform.childCount > 0)
        {
            object_in_hand = left_hand.transform.GetChild(0).gameObject;

            return left_hand;
        }

        else if (right_hand.transform.childCount > 0)
        {
            object_in_hand = right_hand.transform.GetChild(0).gameObject;

            return right_hand;
        }

        return current_main_hand;
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

        // Play pickup sound
        gameObject.GetComponent<pickup_drop_sound>().Pickup_sound();

        // Set object in hand to be the grabbable object
        object_in_hand = grabbable_object;

        // Set parent of grabbable object to be the hand
        //grabbable_object.transform.SetParent(hands.transform);

        //grabbable_object.transform.position = hands.transform.position;

        //grabbable_object.transform.rotation = hands.transform.rotation;

        object_in_hand.transform.SetParent(current_main_hand.transform);

        object_in_hand.transform.position = current_main_hand.transform.position;
        object_in_hand.transform.rotation = current_main_hand.transform.rotation;
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

        // Finds object in hand
        current_main_hand = find_hand_with_object();


        // If user presses "Q"
        if (Input.GetKey("q") && object_in_hand != null)
        {


            if (object_in_hand.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("idle") || object_in_hand.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("empty_magazine"))
            {
                // Tells user that object on hand has been dropped
                text_UI.text = object_in_hand.name + " Dropped!";

                // Play the drop weapon sound
                gameObject.GetComponentInParent<pickup_drop_sound>().Drop_sound();

                // Drop object on hand
                drop_grabbable_object(object_in_hand);

            }
            else
            {
                // Tells user that object on hand has been dropped
                text_UI.text = "Object still in use! Cannot Drop!";
            }

            // Erase text_UI after 1 second
            StartCoroutine(eraseTextAfterSomeTime(1));

        }
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
}