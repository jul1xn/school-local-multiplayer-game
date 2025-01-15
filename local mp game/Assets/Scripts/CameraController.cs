using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float minCameraHeight;
    public float lerpTime;
    public Transform player1;
    public Transform player2;

    private void Update()
    {
        // Get the average of the 2 players positions
        Vector3 targetPosition = (player1.position + player2.position) / 2;

        // If the y position is lower than the minimal camera height, set it to that value
        if (targetPosition.y < minCameraHeight) { targetPosition = new Vector3(targetPosition.x, minCameraHeight, targetPosition.z); }

        // Lerp the camera position to the new position
        Vector3 lerpPosition = new Vector3(transform.position.x, targetPosition.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, lerpPosition, lerpTime * Time.deltaTime);
    }
}
