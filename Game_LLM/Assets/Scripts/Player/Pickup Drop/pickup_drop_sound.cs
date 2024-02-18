using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickup_drop_sound : MonoBehaviour
{

    // Sounds for Picking up and Dropping weapon
    public AudioClip pickup_sound, drop_sound;

    private AudioSource sound_source;

    // Start is called before the first frame update
    void Start()
    {
        sound_source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Plays weapon pickup sound
    public void Pickup_sound()
    {
        Debug.Log("Pickup Sound");
        sound_source.PlayOneShot(pickup_sound);
    }

    // Plays weapon drop sound
    public void Drop_sound()
    {
        Debug.Log("Drop Sound");
        sound_source.PlayOneShot(drop_sound);
    }
}
