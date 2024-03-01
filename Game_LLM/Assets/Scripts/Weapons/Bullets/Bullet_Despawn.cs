using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Despawn : MonoBehaviour
{
    
    // Amount of time until game object despawns by itself
    public float despawn_time;

    // Start is called before the first frame update
    void Start()
    {
        // Destroy itself after a period of time
        Destroy(gameObject, despawn_time);
    }
}
