using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_controller : MonoBehaviour
{
    // Enemy movement speed, default is 1
    public float movementSpeed = 1f;

    GameObject gun;


    public AudioClip deathSound;
    private AudioSource source;

    public void DeathSound()
    {
        source.PlayOneShot(deathSound);
    }

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {

    }
    // This function rotates the enemy towards the target object
    public void rotateEnemy(Transform target_transform)
    {
        // Normalize the vector for rotation purposes
        Vector3 normalized_direction = (target_transform.position - gameObject.transform.position).normalized;

        // Calculate angle which enemy needs to rotate
        float rotationAngle = Mathf.Atan2(normalized_direction.y, normalized_direction.x) * Mathf.Rad2Deg - 90f;

        // Rotate the enemy
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, rotationAngle));
    }

    // This function moves the enemy towards the target object
    public void moveEnemy(Transform target_transform)
    {
        // Normalize the vector for rotation purposes
        Vector3 normalized_direction = (target_transform.position - gameObject.transform.position).normalized;

        // Enemy will move towards the target position
        transform.position += normalized_direction * movementSpeed * Time.deltaTime;
    }

    public void enemyDie()
    {
        
        // If enemy's gun is not destroyed yet
        if (gameObject.transform.childCount > 0)
        {
            // Get gun game object
            gun = gameObject.transform.GetChild(0).gameObject;

            // Enemy "Drops" weapon
            gun.transform.parent = null;
        }

        // Start death animation
        gameObject.GetComponent<Animator>().SetTrigger("die");

    }

    void disableEnemy()
    {
        // Remove box collider
        gameObject.GetComponent<BoxCollider2D>().enabled = false;

        // Remove this script
        gameObject.GetComponent<field_of_view>().enabled = false;

        
    }

    // This function allows the enemy to fire at target object
    public void fireAtTarget(Transform target_transform)
    {
        // Normalize the vector for rotation purposes
        Vector3 normalized_direction = (target_transform.position - gameObject.transform.position).normalized;

        // Calculate angle which enemy needs to rotate
        float rotationAngle = Mathf.Atan2(normalized_direction.y, normalized_direction.x) * Mathf.Rad2Deg;

        // If enemy's gun is not destroyed
        if (gameObject.transform.childCount > 0)
        {
            gun = gameObject.transform.GetChild(0).gameObject;

            // Aim gun towards player
            gun.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, rotationAngle));

            // Fire pistol
            gun.GetComponent<Pistol>().fire_weapon();
        }
    }

}
