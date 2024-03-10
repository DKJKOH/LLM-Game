using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkPaused : MonoBehaviour
{
    public GameObject bargainMenu, shopMenu, PauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // If pause menu is active
        if (bargainMenu.activeSelf || shopMenu.activeSelf || PauseMenu.activeSelf)
        {

            Debug.Log("Apple");

            // Continue time
            Time.timeScale = 0f;
        }
        else
        {
            // Stop Time
            Time.timeScale = 1f;

        }


    }
}
