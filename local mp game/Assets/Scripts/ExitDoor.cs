using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ExitDoor : MonoBehaviour
{
    public AudioSource exitSource;
    public Canvas endCanvas;
    public TMP_Text percentageText;
    public Slider Slider;
    public float playerDistance;
    public float heightPercentage;
    public bool player1Entered;
    public bool player2Entered;
    private float exitHeight;
    private float startHeight;

    private void Start()
    {
        endCanvas.gameObject.SetActive(false);
        exitHeight = transform.position.y;
        startHeight = (PlayerController.instance.player1Collider.transform.position.y + PlayerController.instance.player2Collider.transform.position.y) / 2;
    }

    public void NextLevel()
    {
        int level_id = int.Parse(SceneManager.GetActiveScene().name.Split("_")[1]) + 1;
        SceneManager.LoadScene($"Level_{level_id}");
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void Update()
    {
        float x = (PlayerController.instance.player1Collider.transform.position.y + PlayerController.instance.player2Collider.transform.position.y) / 2;
        playerDistance = exitHeight - x;
        heightPercentage = Mathf.Clamp(((x - startHeight) / (exitHeight - startHeight)) * 100f, 0, 100);
        Slider.value = heightPercentage;
        percentageText.text = $"{Mathf.Round(heightPercentage)}%";
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player_1"))
        {
            player1Entered = true;
        }

        if (collision.gameObject.CompareTag("Player_2"))
        {
            player2Entered = true;
        }

        if (player1Entered && player2Entered)
        {
            endCanvas.gameObject.SetActive(true);
            exitSource.Play();
            PlayerController.instance.gameRunning = false;
            int level_id = int.Parse(SceneManager.GetActiveScene().name.Split("_")[1]);
            ScoreSaver.SaveTime(level_id, PlayerController.instance.currentTime);
            ScoreSaver.SetUnlockedLevel(level_id + 1, true);
        }
    }
}
