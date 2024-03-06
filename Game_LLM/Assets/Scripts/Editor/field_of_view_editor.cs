using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// Set custom editor for field_of_view script
[CustomEditor (typeof (field_of_view))]
public class field_of_view_editor : Editor
{
    private void OnSceneGUI()
    {
        field_of_view fov = (field_of_view)target;

        // Draw the radius of fov
        Handles.color = Color.yellow;

        // Draws circle
        Handles.DrawWireArc(fov.transform.position, Vector3.forward, Vector3.up, 360, fov.radius);


        // Draws the vision cone lines
        Vector3 viewCone1 = fov.DirectionFromAngle(-fov.angle / 2, false);
        Vector3 viewCone2 = fov.DirectionFromAngle(fov.angle / 2, false);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewCone1 * fov.radius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewCone2 * fov.radius);

        // Change color line
        Handles.color = Color.red;
        // Iterate through the current targets seen by the object
        foreach (Transform seen_target in fov.targetsVisible)
        {
            // Draw line from object to target
            Handles.DrawLine(fov.transform.position, seen_target.position);
        }

    }
}
