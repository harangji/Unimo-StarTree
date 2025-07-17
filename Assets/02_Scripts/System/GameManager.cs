using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonBehaviour<GameManager>
{
    public int ChaIdx { get; set; } = 1;
    public int EqIdx { get; set; } = 1;
    public GameObject unimoPrefab;
    
    public int SelectedUnimoID { get; set; } = 10101;
    public int SelectedEngineID { get; set; } = 20101;  // ¶óºñ

    new private void Awake()
    {
        base.Awake();
        if (IsNotWantedObj) { return; }
        Application.targetFrameRate = 60;
    }
}
