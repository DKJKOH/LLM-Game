using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class field_of_view : MonoBehaviour
{
    // Set radius and angle of what the enemy is able to see
    public float radius, angle;

    // Game object that the enemy will be searching for
    private GameObject playerGameObject;

    // Target Mask - Objects to be found is in here
    // Obstruction Mask - Objects which blocks the line of sight
    public LayerMask targetMask, obstructionMask;

    // True if enemy spots player
    public bool playerSeen;

    // This is a list which stores if the player is seen by the enemy   
    [HideInInspector]
    public List<Transform> targetsVisible = new List<Transform>();

    private Transform transform_target;

    private void Start()
    {
        // Find player gameobject with its tag
        playerGameObject = GameObject.FindGameObjectWithTag("Player");

        // Start coroutine to find player
        StartCoroutine(FOVRoutine());
    }

    private void Update()
    {
        // Enemy sees the target object
        if (targetsVisible.Count > 0)
        {
            for (int i = 0; i < targetsVisible.Count; i++)
            {
                // Get target transform
                transform_target = targetsVisible[i];

                // Move enemy towards target
                gameObject.GetComponent<enemy_controller>().moveEnemy(transform_target);

                // Rotate enemy towards target
                gameObject.GetComponent<enemy_controller>().rotateEnemy(transform_target);

                // Enemy fires towards target
                gameObject.GetComponent<enemy_controller>().fireAtTarget(transform_target);

            }
        }
    }

    // Coroutine
    private IEnumerator FOVRoutine()
    {
        // Code which waits (Aka delay) and then checks for player
        WaitForSeconds wait = new WaitForSeconds(0.25f);

        // Search for player
        while (true)
        {

            // Wait for 0.25f
            yield return wait;

            // Tries to find the player game object
            checkFOV();
        }
    }

    // Function which tries to find player game object
    private void checkFOV()
    {
        // CLears list before checking for targets
        targetsVisible.Clear();

        // Check if player is within the vision circle
        Collider2D[] targetsWithinVision = Physics2D.OverlapCircleAll(transform.position, radius, targetMask);

        // Iterations through the list of objects seen within the target mask
        for (int i = 0; i < targetsWithinVision.Length; ++i)
        {
            // Get the details of the target
            Transform target = targetsWithinVision[i].transform;

            // Get the target direction
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            // If target is within the vision cone
            if (Vector3.Angle(transform.up, directionToTarget) < angle / 2)
            {
                // Get the distance from enemy to the target object
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                // Checks if there are any objects blocking the target object by sending a line
                if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    // Add the current target into the list
                    targetsVisible.Add(target);
                }
            }
        }
    }

    public Vector3 DirectionFromAngle(float angleDegrees, bool isAngleGlobal)
    {
        // Convert angles to local angles instead
        if (!isAngleGlobal)
        {
            // This portion rotates the arc lines towards the mouse position
            angleDegrees -= transform.eulerAngles.z;
        }

        return new Vector3(Mathf.Sin(angleDegrees * Mathf.Deg2Rad), Mathf.Cos(angleDegrees * Mathf.Deg2Rad), 0);
    }
}
