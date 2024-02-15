using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class night_vision : MonoBehaviour
{
    public GameObject NightVisionGoggles;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            // Toggle Night Vision Goggles active state
            NightVisionGoggles.SetActive(!NightVisionGoggles.activeSelf);            
        }
    }
}
