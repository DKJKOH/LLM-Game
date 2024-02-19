using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class door_sounds : MonoBehaviour
{
    //  How much of the angle change per update (Delta Angle aka Angle over time) before the creaking sound will be played
    public float rotationThreshold = 0.2f;

    // Audio clip - Creaking 
    public AudioClip creakingSound, slamSound;

    // Audio Source component for the door object
    private AudioSource audioSource;

    private HingeJoint2D joint;

    private bool door_slam;


    // Saves the angle of the hinge in the last update
    float lastAngle;

    // Start is called before the first frame update
    void Start()
    {
        // Get audio source component for the door, to play sound clips
        audioSource = GetComponent<AudioSource>();

        // Loops audio source (For door creaking sound)
        audioSource.loop = true;

        joint = GetComponent<HingeJoint2D>();

        door_slam = false;
    }

    public void DoorSlamSound()
    {
        door_slam = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Get diff between current angle and angle of object in the last update
        float angle = transform.eulerAngles.z - lastAngle;

        // Save last angle as the current object angle
        lastAngle = transform.eulerAngles.z;

        // Make difference positive as angle goes from -180 to 180
        float rotationDelta = Mathf.Abs(angle) * 10;

        if (door_slam && joint.motor.motorSpeed == 0f)
        {
            door_slam = false;

            audioSource.loop = false;

            // Set volume and pitch for creaking sound
            audioSource.volume = 1f;
            audioSource.pitch = 1f;
            
            // Play door slam sound
            audioSource.PlayOneShot(slamSound);

        }

        // If Rate of change of angle is more than rotation threshold
        if (rotationDelta > rotationThreshold && joint.jointSpeed != 0 && !door_slam)
        {
            // Pitch of the creak depends on the door's angle rate of change

            // Loops audio source (For door creaking sound)
            audioSource.loop = true;

            // Adjust pitch of clip from 1f to 2f depending on rotation delta
            float pitch = Mathf.Lerp(.8f, 1.2f, rotationDelta);

            // Set volume and pitch for creaking sound
            audioSource.volume = Mathf.Lerp(0f, 1f, rotationDelta);
            audioSource.pitch = pitch;

            // If audio source is not currently playing, play creaking sound clip
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(creakingSound);
            }

            door_slam = false;
        }

        // Stop door creaking sound if door angle's rate of change is lesser than rotation threshold
        else if (audioSource.isPlaying && !door_slam && joint.motor.motorSpeed != 0f)
        {
            // Stop audio clip
            audioSource.Stop();
        }
    }
}
