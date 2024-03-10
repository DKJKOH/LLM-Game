using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        Debug.Log(collision.gameObject.name);

        // If bullet hits enemy
        if(collision.gameObject.CompareTag("Enemy"))
        {
            // Enemy dies
            collision.gameObject.GetComponent<enemy_controller>().enemyDie();
        }
        // If bullet hits grabbable object
        else if (collision.gameObject.CompareTag("Grabbable_Object"))
        {
            // Despawn Grabbable Object
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Grenade"))
        {
            // Make grenade explode
            collision.gameObject.GetComponent<grenade>().explode();
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            // Load lose screen
            SceneManager.LoadScene("Lose");
        }

        if (!collision.gameObject.CompareTag("Bullet"))
        {
            // Destroy bullet
            Destroy(gameObject);
        }


        
    }
}
