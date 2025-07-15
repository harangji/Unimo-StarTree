using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonBehaviour<GameManager>
{
    public int ChaIdx { get; set; } = 1;
    public int EqIdx { get; set; } = 1;
    
    public int SelectedUnimoID { get; set; } = 10101;

    new private void Awake()
    {
        base.Awake();
        if (IsNotWantedObj) { return; }
        Application.targetFrameRate = 60;
    }
}
