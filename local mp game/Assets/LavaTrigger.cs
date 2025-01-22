using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LavaTrigger : MonoBehaviour
{
    public Canvas gameOverCanvas;
    public Button restartBtn;
    public Button backtomenuBtn;
    public Button destopBtn;
    public float moveSpeed = 12f;
    public float timeDivider = 100;
    public float currentTime;

    private void Start()
    {
        gameOverCanvas.gameObject.SetActive(false);
        currentTime = 0;

        restartBtn.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });

        backtomenuBtn.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(1);
        });

        destopBtn.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            Application.Quit();
        });
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        transform.Translate(new Vector2(0, ((moveSpeed * (currentTime / timeDivider)) * Time.deltaTime)));
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player_1") ||
            collision.gameObject.CompareTag("Player_2"))
        {
            gameOverCanvas.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
