using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public Button loadButton;
    public TMP_Text loadButtonText;
    public TextMeshProUGUI scoreLabel;
    public TextMeshProUGUI mapNameLabel;
    public string visibleName;
    public string sceneName;

    private void OnEnable()
    {
        LoadScoreAndMapName();
    }

    private void LoadScoreAndMapName()
    {
        int target_id = int.Parse(sceneName.Split("_")[1]);
        float score = ScoreSaver.GetTime(target_id);

        if (score == -1)
        {
            score = 0;
        }


        scoreLabel.text = "Score: " + score.ToString("F2");
        mapNameLabel.text = "Map Name: " + visibleName;

        if (!ScoreSaver.HasUnlockedLevel(target_id))
        {
            loadButton.interactable = false;
            loadButtonText.text = "Locked";
        }
    }

    public void LoadScene()
    {
        Debug.Log("Loading scene: " + sceneName);

        SceneManager.LoadScene(sceneName);
    }
}
