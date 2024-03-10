using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{

    // Slider game object
    public Slider volumeSlider;

    void Start()
    {
        // If user touches the volume slider and changes value
        volumeSlider.onValueChanged.AddListener(delegate 
        { 
            // Check how much volume has changed
            OnVolumeChanged(); 
        });
    }

    void OnVolumeChanged()
    {
        // Get the value of the slider ranging from 0 to 1
        float volume = volumeSlider.value;

        // Retrieve all audio sources in scene
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();

        // Iteration through all audio sources
        foreach (AudioSource audioSource in audioSources)
        {
            // Adjust volume for all audio sources
            audioSource.volume = volume;
        }
    }
}
