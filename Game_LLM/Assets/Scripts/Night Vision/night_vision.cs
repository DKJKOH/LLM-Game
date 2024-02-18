using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class night_vision : MonoBehaviour
{
    private Light2D NightVisionGoggles;

    public AudioClip nvg_on_sound, nvg_off_sound;

    // Start is called before the first frame update
    void Start()
    {
        NightVisionGoggles = gameObject.GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            // Get sound source component for this object and set its volume to max
            AudioSource sound_source = gameObject.GetComponent<AudioSource>();
            sound_source.volume = 1f;

            // Toggle Night Vision Goggles active state
            NightVisionGoggles.enabled = !NightVisionGoggles.enabled; 
            
            // If nvg is being turned on
            if (!NightVisionGoggles.enabled)
            {
                sound_source.PlayOneShot(nvg_off_sound);
            }
            // If nvg is off, play turn on nvg goggles sound
            else
            {
                sound_source.PlayOneShot(nvg_on_sound);
            }
        }
    }
}
