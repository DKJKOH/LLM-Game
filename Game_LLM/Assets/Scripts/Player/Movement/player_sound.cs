using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_sound : MonoBehaviour
{
    // Sounds for footsteps
    [Header("Footsteps")]
    public List<AudioClip> walking_sounds;
    public List<AudioClip> running_sounds;
    public List<AudioClip> bored_idle_sounds;

    private AudioSource player_sound_source;

    // Start is called before the first frame update
    void Start()
    {
        // Retrieve footstep source
        player_sound_source = GetComponent<AudioSource>();
    }
    void Play_Footsteps()
    {
        // If a sound clip is currently playing, force it to stop
        if (player_sound_source.isPlaying) player_sound_source.Stop();

        // Initalize audioclip
        AudioClip audioClip = null;

        // Randomize volume and pitch of footsteps
        player_sound_source.volume = 1f;
        player_sound_source.pitch = 1f;

        // Running
        if (gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("running"))
        {
            // Randomly select a sound in the list of running sounds
            audioClip = running_sounds[Random.Range(0, running_sounds.Count)];
        }
        else if (gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("bored_idle"))
        {
            player_sound_source.volume = Random.Range(0.9f, 1f);

            // Randomly select a sound in the list of running sounds
            audioClip = bored_idle_sounds[Random.Range(0, bored_idle_sounds.Count)];
        }
        // Walking
        else
        {
            // Randomly select a sound in the list of walking sounds
            audioClip = walking_sounds[Random.Range(0, walking_sounds.Count)];
        }



        player_sound_source.clip = audioClip;

        // Play the footstep
        player_sound_source.Play();
    }



    // Update is called once per frame
    void Update()
    {
        if (gameObject.GetComponent<Rigidbody2D>().velocity.x == 0 && gameObject.GetComponent<Rigidbody2D>().velocity.y == 0 &! gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("bored_idle"))
        {
            player_sound_source.Stop();
        }
    }
}
