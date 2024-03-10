using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class shotgun : MonoBehaviour
{
    // Control Shotgun animations
    private Animator weapon_animator;

    // Display ammo count for player to see
    private TextMesh ammo_count_text;

    // Ammo management purposes
    public int magazine_size, remaining_bullets_in_magazine, total_bullets;

    // Enable and disable muzzle flash when shooting
    private Light2D muzzle_flash_object;

    // Keep track of time player last fired weapon
    private float time_last_shot;

    // How long will the shooting delay be
    public float shoot_delay_time;

    private bool chamber_weapon;

    // Fire weapon
    void fire_shotgun()
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

    void insert_bullet()
    {
        if (total_bullets <= 0 || remaining_bullets_in_magazine > magazine_size - 1)
        {
            if (chamber_weapon)
            {
                // Stop Reload loop
                weapon_animator.SetTrigger("chamber_round");

                // Set chamber weapon to false
                chamber_weapon = false;
            }
            else
            {
                // Stop Reload loop
                weapon_animator.SetTrigger("stop_reload");
            }
        }

        // If bullets left is lesser than magazine size
        if (total_bullets > 0 && remaining_bullets_in_magazine < magazine_size)
        {
            // Increment bullets in magazine
            remaining_bullets_in_magazine++;

            // Decrement total bullets left
            total_bullets--;
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
        // Update firing delay time
        float current_delay_time = Time.time - time_last_shot;

        // Ensures that muzzle flash is not active if not being shot
        if (weapon_animator.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            // Disable muzzle flash
            remove_muzzle_flash();
        }

        // If weapon is picked up and time is not paused
        if (gameObject.transform.parent != null 
            && GameObject.FindGameObjectWithTag("GameManager").GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("idle")
            && Time.timeScale > 0f)
        {

            // Rotate ammo count text to be facing upwards all the time 
            ammo_count_text.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

            // Display ammo count (Ammo: remaining bullets in magazine / total bullets left)
            ammo_count_text.text = "Ammo: " + remaining_bullets_in_magazine.ToString() + "/" + total_bullets.ToString();

            // If player clicks fire button
            if (Input.GetMouseButton(0)
                && current_delay_time >= shoot_delay_time
                && remaining_bullets_in_magazine > 0
                && weapon_animator.GetCurrentAnimatorStateInfo(0).IsName("idle"))
            {
                // Fire weapon

                weapon_animator.SetTrigger("Fire");

                // Save the last shot time
                time_last_shot = Time.time;
            }

            // If user presses 'R' to reload weapon
            if (Input.GetKeyDown(KeyCode.R) && remaining_bullets_in_magazine < magazine_size && total_bullets > 0)
            {
                // Ensures that chamber weapon happens
                if (remaining_bullets_in_magazine == 0) chamber_weapon = true;

                // Start Reload loop
                weapon_animator.SetTrigger("reload");
            }

            // Conditions for user to stop reload loop (If user fires weapon or runs)
            if (weapon_animator.GetCurrentAnimatorStateInfo(0).IsName("insert_bullet") && (Input.GetMouseButton(0) || Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)))
            {
                // Chamber round
                if (chamber_weapon) weapon_animator.SetTrigger("chamber_round");

                // Stop reload loop
                else weapon_animator.SetTrigger("stop_reload");
            }

            // If there are no bullets in magazine
            if (remaining_bullets_in_magazine <= 0)
            {
                // This ensures that weapon is cocked
                chamber_weapon = true;
            }

        }
        else
        {
            // Remove ammo count text
            ammo_count_text.text = "";

        }
    }
}
