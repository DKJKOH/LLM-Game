using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.Rendering.Universal;

public class Pistol : MonoBehaviour
{
    // To track and start pistol animations
    private Animator pistol_animator;

    // To display ammo for the gun
    private TextMesh ammo_count_text;

    // For ammo count purposes
    public int magazine_size, remaining_bullets_in_magazine, total_bullets;

    // To enable and disable muzzle flash
    private Light2D muzzle_flash_object;

    // Keep track of time player last fired weapon
    private float time_last_shot;

    // How long will the shooting delay be
    public float shoot_delay_time;

    void reload_pistol()
    {
        // Collate remaining bullet amount
        total_bullets = remaining_bullets_in_magazine + total_bullets;

        // If total bullets are not more than magazine capacity
        if (total_bullets <= magazine_size)
        {
            // Set remaining bullets in magazine
            remaining_bullets_in_magazine = total_bullets;

            // Ensure that there are no bullets for reload
            total_bullets = 0;
        }
        // If total bullets is more than magazine capacity
        else
        {
            // Refill magazine
            remaining_bullets_in_magazine = magazine_size;

            // Deduct the amount of ammunition from total
            total_bullets = total_bullets - magazine_size;
        }
    }

    void fire_pistol()
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

    void remove_muzzle_flash()
    {
        // Disable Muzzle flash
        muzzle_flash_object.enabled = false;
    }




    // Start is called before the first frame update
    void Start()
    {
        pistol_animator = gameObject.GetComponent<Animator>();

        ammo_count_text = gameObject.transform.GetChild(0).GetComponent<TextMesh>();

        muzzle_flash_object = gameObject.GetComponentInChildren<Light2D>();

        // Save start flash time as 0
        time_last_shot = 0f;    
    }

    // Update is called once per frame
    void Update()
    {
        float current_delay_time = Time.time - time_last_shot;

        // If weapon is being equipped by player
        if (gameObject.transform.parent != null)
        {
            ammo_count_text.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

            // Display ammo count
            ammo_count_text.text = "Ammo: " + remaining_bullets_in_magazine.ToString() + "/" + total_bullets.ToString();

            // Fire weapon normally
            if (Input.GetMouseButtonDown(0) && remaining_bullets_in_magazine > 1 && current_delay_time > shoot_delay_time)
            {
                // Save time where player last shot as now
                time_last_shot = Time.time;

                // Start pistol fire animation
                pistol_animator.SetTrigger("fire");
            }
            // If user is firing last bullet in magazine
            else if (Input.GetMouseButtonDown(0) && remaining_bullets_in_magazine == 1 && current_delay_time > shoot_delay_time)
            {
                // Save time where player last shot as now
                time_last_shot = Time.time;

                // Start fire last bullet pistol fire animation
                pistol_animator.SetTrigger("fire_last_bullet");
            }

            // Reload weapon
            else if (Input.GetKeyDown(KeyCode.R) && total_bullets > 0 && remaining_bullets_in_magazine < magazine_size)
            {
                pistol_animator.SetTrigger("reload");
            }
        }
        else
        {
            // Display ammo count
            ammo_count_text.text = "";
        }
       
    }
}
