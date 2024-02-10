using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_sound : MonoBehaviour
{
    [Header("Footsteps")]
    public List<AudioClip> walking_sounds;
    public List<AudioClip> running_sounds;

    private AudioSource footstep_source;

    // Start is called before the first frame update
    void Start()
    {
        // Retrieve footstep source
        footstep_source = GetComponent<AudioSource>();
    }
    void Play_Footsteps()
    {
        
        // If a sound clip is currently playing, force it to stop
        if (footstep_source.isPlaying) footstep_source.Stop();

        // Initalize audioclip
        AudioClip audioClip = null;

        // Running
        if (gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("running"))
        {
            Debug.Log("Running sound is playing");

            // Randomly select a sound in the list of running sounds
            audioClip = running_sounds[Random.Range(0, running_sounds.Count)];
        }
        // Walking
        else
        {
            // Randomly select a sound in the list of walking sounds
            audioClip = walking_sounds[Random.Range(0, walking_sounds.Count)];
        }



        footstep_source.clip = audioClip;

        // Randomize volume and pitch of footsteps
        footstep_source.volume = Random.Range(0.2f, 0.8f);
        footstep_source.pitch = Random.Range(0.5f, 1f);

        // Play the footstep
        footstep_source.Play();
    }



    // Update is called once per frame
    void Update()
    {
        if (gameObject.GetComponent<Rigidbody2D>().velocity.x == 0 && gameObject.GetComponent<Rigidbody2D>().velocity.y == 0)
        {
            footstep_source.Stop();
        }
    }
}
