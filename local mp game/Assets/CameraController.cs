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
        Vector3 targetPosition = (player1.position + player2.position) / 2;
        if (targetPosition.y < minCameraHeight) { targetPosition = new Vector3(targetPosition.x, minCameraHeight, targetPosition.z); }

        Vector3 lerpPosition = new Vector3(transform.position.x, targetPosition.y, transform.position.z);

        transform.position = Vector3.Lerp(transform.position, lerpPosition, lerpTime * Time.deltaTime);
    }
}
