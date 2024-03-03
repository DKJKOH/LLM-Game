using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class change_hand : MonoBehaviour
{
    public Transform leftHand;
    public Transform rightHand;

    public Transform currentHand;
    private Transform heldItem;

    void Start()
    {
        // Start with the left hand
        currentHand = rightHand;
    }

    void Update()
    {
        if (leftHand.childCount > 0) heldItem = leftHand.GetChild(0);
        else if (rightHand.childCount > 0) heldItem = rightHand.GetChild(0);



        // Example: Switch hands on a key press (e.g., 'Q' key)
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchHands();
        }
    }

    void SwitchHands()
    {
        if (heldItem != null)
        {
            // Detach item from the current hand
            heldItem.SetParent(null);

            // Switch to the other hand
            currentHand = (currentHand == leftHand) ? rightHand : leftHand;

            // Attach item to the new hand
            heldItem.SetParent(currentHand);
            heldItem.position = currentHand.position; // Adjust as needed
        }

    }
}
