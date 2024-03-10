using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class grenade : MonoBehaviour
{
    // Shows the line from object to cursor
    private LineRenderer lineRenderer;
    private Vector3 mousePosition;
    private Vector3 objectPosition;

    public float maximumThrowDistance;

    // Distance between object and cursor
    private float distance;

    // Sets how fast the grenade can be thrown
    public float throwSpeed;

    // Boolean which tells the timer to start
    private bool isThrown = false;

    // Location of the target which the grenade will be thrown to
    private Vector2 throwTarget;

    // Countdown timer for explosion
    public float explosionCountdownTime = 3;

    // Keeps track on how much time is left before grenade explodes
    private float timeRemaining;

    // Text which shows time remaining
    private TextMesh timeRemainingText;

    // If true, countdown timer will start
    private bool explosionStart = false;

    // Explosion particle game object which will instantiate after grenade timer hits 0
    public GameObject explosionParticles;

    // For grenade sounds
    private AudioSource audio_controller;

    public AudioClip pullPinSound, throwGrenadeSound;

    // Start is called before the first frame update
    void Start()
    {
        // Get line renderer component
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.positionCount = 2;

        // Set time remaining to be the explosion countdown time
        timeRemaining = explosionCountdownTime;

        // Get time remaining text component
        timeRemainingText = gameObject.transform.GetChild(0).GetComponent<TextMesh>();

        // Get audio source component from current object, to play sounds and stuff
        audio_controller = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            // Stop Grenade from moving
            isThrown = false;
        }
    }

    public void explode()
    {
        // Set explosion to be true
        explosionStart = true;

        // Set time remaining to 0
        timeRemaining = 0f;
    }

    void countdownTimer()
    {
        if (explosionStart)
        {
            // Set countdown time to be upright all the time
            timeRemainingText.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

            // Deduct the time remaining
            timeRemaining -= Time.deltaTime;


            // If no time left
            if (timeRemaining <= 0)
            {
                // Kaboom!
                Instantiate(explosionParticles, transform.position, transform.rotation);

                // Destroy grenade
                Destroy(gameObject);
            }
            else
            {
                // Show remaining time in the game (2 Decimal places - "0.00")
                timeRemainingText.text = timeRemaining.ToString("0.00");
            }
        }

    }

    // Function which 'moves' the grenade after player indicated where they throw
    void throwGrenade()
    {
        // If player throws grenade
        if (isThrown)
        {
            // Calculate fixed speed
            float step = throwSpeed * Time.deltaTime;

            // move sprite towards the target location
            transform.position = Vector2.MoveTowards(transform.position, throwTarget, step);

            // Start grenade explosion
            explosionStart = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Start countdown if explosion starts
        countdownTimer();

        // Function which 'moves' grenade after player indicated where they throw
        throwGrenade();


        // Get new mouse position
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Current object's position
        objectPosition = transform.position;

        // Get mouse direction
        mousePosition.z = -1f;
        objectPosition.z = -1f;

        // Distance from object position to cursor
        distance = (mousePosition - objectPosition).magnitude;

        if (distance > maximumThrowDistance)
        {
            // Set line to red
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.red;
        }
        else
        {
            // Set line to green

            lineRenderer.startColor = Color.green;
            lineRenderer.endColor = Color.green;
        }

        // If grenade is in idle state and game is not paused
        if (GameObject.FindGameObjectWithTag("GameManager").GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("idle")
            && Time.timeScale > 0f)
        {
            // If user presses button down
            if (Input.GetMouseButton(0) && gameObject.transform.parent != null)
            {
                lineRenderer.enabled = true;

                lineRenderer.SetPosition(0, objectPosition);

                lineRenderer.SetPosition(1, mousePosition);
            }
            else
            {
                lineRenderer.enabled = false;
            }

            // If user clicks fire button
            if (Input.GetMouseButtonDown(0) && gameObject.transform.parent != null)
            {
                // Play pull grenade pin sound 
                audio_controller.PlayOneShot(pullPinSound);
            }

            if (Input.GetMouseButtonUp(0) && gameObject.transform.parent != null && distance < maximumThrowDistance)
            {
                // Set is thrown to be true
                isThrown = true;

                // Play throw grenade sound
                audio_controller.PlayOneShot(throwGrenadeSound);

                // Set throw target
                throwTarget = mousePosition;

                // Remove object from hand
                gameObject.transform.SetParent(null);
            }

        }
    }
}
