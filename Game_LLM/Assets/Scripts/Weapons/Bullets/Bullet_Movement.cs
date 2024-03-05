using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Movement : MonoBehaviour
{
    // Bullet's Rigid body
    private Rigidbody2D rb;

    // How fast the bullet will move
    public float bullet_speed;

    // Start is called before the first frame update
    void Start()
    {
        // calling rigidbody2d
        //rb = GetComponent<Rigidbody2D>();

        // Simulates "Explosion" of bullet using Impulse
        //rb.AddForce(transform.right * bullet_speed * Time.deltaTime, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
