using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hands : MonoBehaviour
{
    // Player animator
    Animator player_animator;

    // Start is called before the first frame update
    void Start()
    {
        // Get player animator (Running / Walking / Idle / Bored walking thing)
        player_animator = gameObject.transform.parent.GetComponent<Animator>();
    }

    /* Function which disables player's hands when they are running */
    void disable_hands_when_running()
    {
        // If player is not holding anything, do not run this function
        if (transform.childCount <= 0) return;

        // Player is unable to shoot / aim when running
        if (player_animator.GetBool("Running"))
        {
            // Disable weapon on hand
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
        // Player can to shoot / aim when walking
        else
        {
            // Enable weapon on hand
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Disables hands when player is running
        disable_hands_when_running();

        // Get current mouse position 
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Get direction of object towards cursor
        Vector3 direction = mousePos - transform.position;

        // Convert direction to angle
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        // Rotate object about z axis
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
