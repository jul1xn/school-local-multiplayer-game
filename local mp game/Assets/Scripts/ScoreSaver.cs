using Unity.VisualScripting;
using UnityEngine;

public static class ScoreSaver
{
    public static void SaveTime(int level_id, float time)
    {
        PlayerPrefs.SetFloat($"level_{level_id}_time", time);
        Debug.Log("Saved time for level " + level_id);
    }
    public static void SetUnlockedLevel(int level_id, bool unlocked)
    {
        PlayerPrefs.SetString($"level_{level_id}_unlocked", unlocked.ToString());
    }

    public static float GetTime(int level_id)
    {
        return PlayerPrefs.GetFloat($"level_{level_id}_time", -1);
    }

    public static bool HasUnlockedLevel(int level_id)
    {
        return bool.Parse(PlayerPrefs.GetString($"level_{level_id}_unlocked", "false"));
    }
}