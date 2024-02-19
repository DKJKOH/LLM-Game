using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door_sounds : MonoBehaviour
{
    //  How much of the angle change per update (Delta Angle aka Angle over time) before the creaking sound will be played
    public float rotationThreshold = 0.1f;

    // Audio clip - Creaking 
    public AudioClip creakingSound;

    // Audio Source component for the door object
    private AudioSource audioSource;

    // Saves the angle of the hinge in the last update
    float lastAngle;

    // Start is called before the first frame update
    void Start()
    {
        // Get audio source component for the door, to play sound clips
        audioSource = GetComponent<AudioSource>();

        // Loops audio source (For door creaking sound)
        audioSource.loop = true; 
    }

    // Update is called once per frame
    void Update()
    {
        // Get diff between current angle and angle of object in the last update
        float angle = transform.eulerAngles.z - lastAngle;

        // Save last angle as the current object angle
        lastAngle = transform.eulerAngles.z;

        // Make difference positive as angle goes from -180 to 180
        float rotationDelta = Mathf.Abs(angle);

        // If Rate of change of angle is more than rotation threshold
        if (rotationDelta > rotationThreshold)
        {
            // Volume and pitch of the creak depends on the door's angle rate of change
            
            // Scales volume from 0 to 1 depending on rotationDelta
            float volume = Mathf.Clamp01(rotationDelta);

            // Adjust pitch of clip from 1f to 2f depending on rotation delta
            float pitch = Mathf.Lerp(1f, 2f, rotationDelta);

            // Set volume and pitch for creaking sound
            audioSource.volume = volume;
            audioSource.pitch = pitch;

            // If audio source is not currently playing, play creaking sound clip
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(creakingSound);
            }
        }
        // Stop door creaking sound if door angle's rate of change is lesser than rotation threshold
        else if (audioSource.isPlaying)
        {
            // Stop audio clip
            audioSource.Stop();
        }
    }
}
