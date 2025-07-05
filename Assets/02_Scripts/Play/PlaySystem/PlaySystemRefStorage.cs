using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySystemRefStorage : MonoBehaviour
{
    static public ScoreManager scoreManager;
    static public MapRangeSetter mapSetter;
    static public PlayTimeManager playTimeManager;
    static public PlayProcessController playProcessController;
    static public PlayerStatManager playerStatManager;
    static public HarvestLevelController harvestLvController;
    // Start is called before the first frame update
    void Start()
    {
        if (scoreManager == null) { Debug.Log("Score Manager not found."); }
        if (mapSetter == null) { Debug.Log("MapSetter not found."); }
        if (playTimeManager == null) { Debug.Log("Time Manage not found."); }
        if (playProcessController == null) { Debug.Log("process Ctrl not found."); }
        if (playerStatManager == null) { Debug.Log("player not found."); }
        if (harvestLvController == null) { Debug.Log("harvest Lv ctrl not found."); }
    }
}
