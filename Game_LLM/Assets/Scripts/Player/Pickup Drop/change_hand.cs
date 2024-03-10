using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class change_hand : MonoBehaviour
{
    public Transform leftHand;
    public Transform rightHand;

    public Transform currentHand;
    private Transform heldItem;

    // Audio clip to play change main hand carrying weapon sound.
    public AudioClip swapWeaponHandSound;
    public AudioSource swapWeaponSource;

    void Start()
    {
        // Start with the left hand
        currentHand = rightHand;
    }

    void Update()
    {
        if (leftHand.childCount > 0) heldItem = leftHand.GetChild(0);
        else if (rightHand.childCount > 0) heldItem = rightHand.GetChild(0);

        // If player presses tab to change hands, and game is not paused
        if (Input.GetKeyDown(KeyCode.Tab) && Time.timeScale > 0f)
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
            heldItem.position = currentHand.position;

            // Play swap weapon hand sound
            swapWeaponSource.PlayOneShot(swapWeaponHandSound);
        }

    }
}
