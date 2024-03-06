using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet_Collision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // If bullet hits another object with a rigid body 2D
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If bullet hit wall or door (tagged as wall)
        if (collision.gameObject.CompareTag("Wall"))
        {
            // Despawn bullet
            Destroy(gameObject);
        }

        // If bullet hits enemy
        else if(collision.gameObject.CompareTag("Enemy"))
        {
            // Enemy dies
            collision.gameObject.GetComponent<enemy_controller>().enemyDie();

            // Despawn bullet
            Destroy(gameObject);
        }
        // If bullet hits grabbable object
        else if (collision.gameObject.CompareTag("Grabbable_Object"))
        {
            // Despawn Grabbable Object
            Destroy(collision.gameObject);

            // Despawn bullet
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Grenade"))
        {
            // Make grenade explode
            collision.gameObject.GetComponent<grenade>().explode();
        }
        
    }
}
