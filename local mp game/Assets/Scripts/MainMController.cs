using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMController : MonoBehaviour
{
    private void Start()
    {
        if (!ScoreSaver.HasUnlockedLevel(0))
        {
            ScoreSaver.SetUnlockedLevel(0, true);
        }
    }

    public void ResetScores()
    {
        ScoreSaver.SetUnlockedLevel(0, true);
        ScoreSaver.SetUnlockedLevel(1, false);
        ScoreSaver.SetUnlockedLevel(2, false);
        ScoreSaver.SetUnlockedLevel(3, false);
        
        ScoreSaver.SaveTime(0, 0);
        ScoreSaver.SaveTime(1, 0);
        ScoreSaver.SaveTime(2, 0);
        ScoreSaver.SaveTime(3, 0);
    }
}
