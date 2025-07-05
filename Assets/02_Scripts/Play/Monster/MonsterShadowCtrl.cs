using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterShadowCtrl : MonoBehaviour
{
    [SerializeField] private GameObject shadowObj;

    // Update is called once per frame
    void Update()
    {
        if (PlaySystemRefStorage.mapSetter.IsInMap(transform.position) != shadowObj.activeSelf)
        {
            shadowObj.SetActive(PlaySystemRefStorage.mapSetter.IsInMap(transform.position));
        }
    }
}
