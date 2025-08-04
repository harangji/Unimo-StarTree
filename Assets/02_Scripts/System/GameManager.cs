using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonBehaviour<GameManager>
{
    public int ChaIdx { get; set; } = 1;
    public int EqIdx { get; set; } = 1;

    public int SelectedUnimoID { get; set; } = 10101;
    public int SelectedEngineID { get; set; } = 20101;

    new private void Awake()
    {
        base.Awake();
        if (SelectedEngineID <= 0)
            SelectedEngineID = 20101; // 기본 엔진 설정
        
        if (IsNotWantedObj) { return; }
        Application.targetFrameRate = 60;
    }
}
