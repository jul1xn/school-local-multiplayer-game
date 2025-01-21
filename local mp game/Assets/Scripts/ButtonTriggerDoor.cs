using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTriggerDoor : MonoBehaviour
{
    public void TriggerDoor()
    {
        Debug.Log("Has been triggered!");
        Destroy(gameObject);
    }
}
