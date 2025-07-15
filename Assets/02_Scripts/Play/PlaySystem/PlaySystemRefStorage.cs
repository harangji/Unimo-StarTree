using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySystemRefStorage : MonoBehaviour
{
    public static ScoreManager scoreManager;
    public static MapRangeSetter mapSetter;
    public static PlayTimeManager playTimeManager;
    public static PlayProcessController playProcessController;
    public static PlayerStatManager playerStatManager;
    public static HarvestLevelController harvestLvController;
    public static StageManager stageManager;
    // Start is called before the first frame update
    void Start()
    {
        // if (scoreManager == null) { Debug.Log("Score Manager not found."); }
        if (mapSetter == null) { Debug.Log("MapSetter not found."); }
        if (playTimeManager == null) { Debug.Log("Time Manage not found."); }
        if (playProcessController == null) { Debug.Log("process Ctrl not found."); }
        if (playerStatManager == null) { Debug.Log("player not found."); }
        if (harvestLvController == null) { Debug.Log("harvest Lv ctrl not found."); }
        if (scoreManager == null) { Debug.Log("ScoreManager not found."); }
    }
}
