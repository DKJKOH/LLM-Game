using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shotgun_sounds : MonoBehaviour
{
    // Gun sounds
    private AudioSource audioSource;

    // Audio Clips
    public AudioClip fire_sound, insert_bullet_sound, chamber_round_sound, shell_bounce_sound, dry_fire_sound;

    // Meant to check if dry fire sound should be played
    private int current_ammo_in_magazine;

    void Fire_sound()
    {
        audioSource.PlayOneShot(fire_sound);
    }

    void Insert_Bullet_Sound()
    {
        audioSource.PlayOneShot(insert_bullet_sound);
    }

    void Chamber_round_sound()
    {
        audioSource.PlayOneShot(chamber_round_sound);
    }

    void Shell_bounce_sound()
    {
        audioSource.PlayOneShot(shell_bounce_sound);
    }

    void Dry_fire_sound()
    {
        audioSource.PlayOneShot(dry_fire_sound);
    }

    // Update is called once per frame
    void Update()
    {
        // Constantly check for remaining bullets in magazine
        current_ammo_in_magazine = gameObject.GetComponent<shotgun>().remaining_bullets_in_magazine;

        // If weapon is picked up by user, get the audio source
        if (transform.parent != null)
        {
            // Retrieve audio source component
            audioSource = transform.parent.gameObject.GetComponent<AudioSource>();

            // If weapon has no bullets and player tries to fire, play dry fire sound
            if (current_ammo_in_magazine == 0 && Input.GetMouseButtonDown(0)) Dry_fire_sound();
        }
    }
}
