using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_death : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Collision Enemy");
            Destroy(gameObject);
            ResetGameVariables();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void ResetGameVariables()
    {
        PlayerController.instance.gameRunning = false;
    }
}
