using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPT_UI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void exit_GPT_UI()
    {

        // Resume movement
        Time.timeScale = 1f;

        // Set ui as false
        gameObject.SetActive(false);
    }
}
