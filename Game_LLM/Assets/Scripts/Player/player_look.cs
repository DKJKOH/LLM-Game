using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class player_look : MonoBehaviour
{
    public GameObject GPT_UI, Shop_UI, Pause_UI;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("GameManager").GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("idle") && !GPT_UI.activeSelf && !Shop_UI.activeSelf && !Pause_UI.activeSelf)
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
}
