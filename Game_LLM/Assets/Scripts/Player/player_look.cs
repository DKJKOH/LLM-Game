using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class player_look : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Get current mouse position 
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Get direction from player to mouse positon
        Vector3 direction = mousePos - transform.position;

        // Convert vector direction into angle to rotate the player
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        // Actually Rotate player
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
