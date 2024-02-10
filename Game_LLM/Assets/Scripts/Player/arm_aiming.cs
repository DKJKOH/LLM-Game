using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arm_aiming : MonoBehaviour
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

        // Get direction of object towards cursor
        Vector3 direction = mousePos - transform.position;

        // Convert direction to angle
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        // Rotate object about z axis
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
