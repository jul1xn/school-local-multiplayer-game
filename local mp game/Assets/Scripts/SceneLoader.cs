using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public TextMeshProUGUI scoreLabel;
    public TextMeshProUGUI mapNameLabel;
    public string sceneName;

    private void OnEnable()
    {
        LoadScoreAndMapName();
    }

    private void LoadScoreAndMapName()
    {
        float score = ScoreSaver.GetTime(1);

        if (score == -1)
        {
            score = 0;
        }


        scoreLabel.text = "Score: " + score.ToString("F2");
        mapNameLabel.text = "Map Name: " + sceneName;
    }

    public void LoadScene()
    {
        Debug.Log("Loading scene: " + sceneName);

        SceneManager.LoadScene(sceneName);
    }
}
