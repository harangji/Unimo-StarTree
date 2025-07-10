using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySceneStatMultiplier : MonoBehaviour
{
    
    // 해당 스크립트에서 아우라 넓이 관리. 정현식
    //Class will be used initailize player stats using multiplier
    [SerializeField] private float speedMultiplier = 8.5f;
    [SerializeField] private float areaMultiplier = 9f;
    // Start is called before the first frame update
    void Awake()
    {
        FindAnyObjectByType<PlayerMover>().SetSpeed(speedMultiplier);
        FindAnyObjectByType<AuraController>().InitAura(areaMultiplier);
    }
}
