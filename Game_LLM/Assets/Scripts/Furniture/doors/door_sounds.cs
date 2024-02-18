using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door_sounds : MonoBehaviour
{
    public float rotationThreshold = 0.1f;
    public AudioClip creakingSound;


    private AudioSource audioSource;

    float lastAngle;

    // Get hingejoint component of object
    private HingeJoint2D door_joint;

    // Start is called before the first frame update
    void Start()
    {
        // Get hingejoint component
        door_joint = GetComponent<HingeJoint2D>();

        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true; // Set to loop the sound
    }

    // Update is called once per frame
    void Update()
    {
        float angle = transform.eulerAngles.z - lastAngle;

        lastAngle = transform.eulerAngles.z;

        float rotationDelta = Mathf.Abs(angle);

        if (rotationDelta > rotationThreshold)
        {
            float volume = Mathf.Clamp01(rotationDelta); // Scale volume from 0 to 1
            float pitch = Mathf.Lerp(1f, 2f, rotationDelta); // Adjust pitch based on rotation speed

            audioSource.volume = volume;
            audioSource.pitch = pitch;

            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(creakingSound);
            }
        }
        else if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
