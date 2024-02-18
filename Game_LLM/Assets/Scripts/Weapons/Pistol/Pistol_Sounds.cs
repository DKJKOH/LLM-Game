using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pistol_Sounds : MonoBehaviour
{

    // Gun sounds
    private AudioSource audioSource;
    public AudioClip fire_sound, release_slide_sound, load_magazine_sound, unload_magazine_sound, drop_magazine_sound, shell_eject_sound, dry_fire_sound;
    public List<AudioClip> shell_bounce_sound;
    
    // Meant to check if dry fire sound should be played
    private int current_ammo_in_magazine;

    void Start()
    {
    }

    // This functions plays sounds
    void Fire_sound()
    {
        audioSource.PlayOneShot(fire_sound);
    }
    void Release_slide_sound()
    {
        audioSource.PlayOneShot(release_slide_sound);
    }

    void Load_magazine_sound()
    {
        audioSource.PlayOneShot(load_magazine_sound);
    }

    void Unload_magazine_sound()
    {
        audioSource.PlayOneShot(unload_magazine_sound);
    }

    void Drop_magazine_sound()
    {
        audioSource.PlayOneShot(drop_magazine_sound);
    }

    void Shell_eject_sound()
    {
        audioSource.PlayOneShot(shell_eject_sound);
    }

    void Shell_bounce_sound()
    {
        // Play a random sound from the list of shell bounce sound
        audioSource.PlayOneShot(shell_bounce_sound[Random.Range(0, shell_bounce_sound.Count)]);
    }

    void Dry_fire_sound()
    {
        audioSource.PlayOneShot(dry_fire_sound);
    }
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {

        // Thre reason for placing the audio source on the hands instead of the gun object is because the gun will get disabled, stopping any sounds
        if (transform.parent != null) audioSource = transform.parent.gameObject.GetComponent<AudioSource>();

        current_ammo_in_magazine = gameObject.GetComponent<Pistol>().remaining_bullets_in_magazine;

        // If user tries to fire weapon with no bullets in magazine, play dry fire sound
        if (current_ammo_in_magazine == 0 && Input.GetMouseButtonDown(0) && transform.parent.gameObject != null) Dry_fire_sound();

    }
}
