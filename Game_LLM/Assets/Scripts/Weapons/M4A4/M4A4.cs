using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class M4A4 : MonoBehaviour
{
    // Control M4 animations
    private Animator weapon_animator;

    // Display gun ammo count
    private TextMesh ammo_count_text;

    // For ammo count purposes
    public int magazine_size, remaining_bullets_in_magazine, total_bullets;

    // To enable and disable muzzle flash
    private Light2D muzzle_flash_object;

    // Keep track of time player last fired weapon
    private float time_last_shot;

    // How long will the shooting delay be
    public float shoot_delay_time;

    // Fire weapon
    void fire_M4A4()
    {
        // If there are bullets left in the weapon
        if (remaining_bullets_in_magazine > 0)
        {
            // Enable Muzzle flash
            muzzle_flash_object.enabled = true;

            // Deduct magazine by 1 bullet
            remaining_bullets_in_magazine--;
        }
    }

    // Function which removes muzzle flash (Light)
    void remove_muzzle_flash()
    {
        // Disable Muzzle flash
        muzzle_flash_object.enabled = false;

    }

    // Refills ammunititon in magazine (if there is enough ammo)
    void reload_M4A4()
    {
        // Collate total amount of bullets
        total_bullets = total_bullets + remaining_bullets_in_magazine;

        // If bullets left is lesser than magazine size
        if (total_bullets <= magazine_size)
        {
            // Set remaining bullets as remaining bullets in magazine
            remaining_bullets_in_magazine = total_bullets;

            // Make sure that bullets left is 0, cannot reload anymore
            total_bullets = 0;
        }
        // Reload as per normal
        else
        {
            // Refill magazine
            remaining_bullets_in_magazine = magazine_size;

            // Reduce amount of bullets remaining to be used
            total_bullets = total_bullets - magazine_size;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        // Retrieve Animator component in game object
        weapon_animator = GetComponent<Animator>();

        // Setup ammo count text
        ammo_count_text = gameObject.transform.GetChild(0).GetComponent<TextMesh>();

        // When script starts, initalize time last shot as 0f
        time_last_shot = Time.time;

        // Get light component in children
        muzzle_flash_object = gameObject.GetComponentInChildren<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get total time elapsed between current time and the time where the previous shot happened
        float current_delay_time = Time.time - time_last_shot;




        // If weapon is picked up
        if (gameObject.transform.parent != null)
        {
            // Rotate ammo count text to be facing upwards all the time 
            ammo_count_text.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

            // Display ammo count (Ammo: remaining bullets in magazine / total bullets left)
            ammo_count_text.text = "Ammo: " + remaining_bullets_in_magazine.ToString() + "/" + total_bullets.ToString();

            // If user presses fire button and total number of bullets left in magazine is more than 1
            if (Input.GetMouseButton(0) && current_delay_time >= shoot_delay_time && remaining_bullets_in_magazine > 1)
            {
                // Fire weapon
                weapon_animator.SetBool("Fire", true);

                // Save the last shot time
                time_last_shot = Time.time;
            }
            else
            {
                // Stop firing weapon
                weapon_animator.SetBool("Fire", false);
            }

            // If magazine has its last bullet
            if (Input.GetMouseButton(0) && current_delay_time >= shoot_delay_time && remaining_bullets_in_magazine == 1)
            {
                // fire last shot
                weapon_animator.SetBool("fire_last_bullet", true);

                // Save the last shot time
                time_last_shot = Time.time;
            }
            else
            { 
                    // fire last shot
                    weapon_animator.SetBool("fire_last_bullet", false);
            }


            // If user presses reload and there are bullets available
            if (remaining_bullets_in_magazine < magazine_size && Input.GetKeyDown(KeyCode.R) && total_bullets > 0)
            {
                // Trigger reload animation
                weapon_animator.SetTrigger("reload");
            }

        }
        // If m4 is not being held by player
        else
        {
            // Hide ammo count
            ammo_count_text.text = "";
        }



    }
}
