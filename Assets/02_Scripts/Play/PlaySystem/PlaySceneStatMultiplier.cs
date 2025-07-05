using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySceneStatMultiplier : MonoBehaviour
{
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
