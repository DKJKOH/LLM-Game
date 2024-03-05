using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bullet_Spawn : MonoBehaviour
{
    // What object will be spawned
    public GameObject bullet;

    // Location where the bullet will spawn
    public GameObject spawnPosition;

    // Set bullet speed here
    public float bullet_speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    void spawnBullet()
    {
        // Create bullet
        GameObject newBullet = Instantiate(bullet, spawnPosition.transform);

        // Set the bullet's velocity
        newBullet.GetComponent<Rigidbody2D>().velocity = newBullet.transform.right * bullet_speed;

        // Ensure that bullet does not have any parent
        newBullet.transform.parent = null;
    }
}
