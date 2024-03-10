using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Explosion : MonoBehaviour
{
    public AudioClip explosionSound;

    private ParticleSystem particleSys;

    void Start()
    {
        // Play explosion clip at that particular point
        AudioSource.PlayClipAtPoint(explosionSound, Camera.main.gameObject.transform.position);

        // Get particle system to detect objects
        particleSys = GetComponent<ParticleSystem>();
    }

    // If particles collide with other objects
    private void OnParticleCollision(GameObject other)
    {
        // If explosion hits player or enemy or door
        if (other.CompareTag("Enemy"))
        {
            // Enemy dies
            other.gameObject.GetComponent<enemy_controller>().enemyDie();
        }
        else if (other.CompareTag("Player"))
        {
            // Load lose screen
            SceneManager.LoadScene("Lose");
        }

        if (other.CompareTag("Grenade"))
        {
            // Set grenade to explode
            other.GetComponent<grenade>().explode();
        }
    }

}

