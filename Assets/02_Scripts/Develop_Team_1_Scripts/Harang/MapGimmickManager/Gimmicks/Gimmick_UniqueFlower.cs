using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Gimmick_UniqueFlower : Gimmick
{
    [Header("지역 활성화 설정")]
    
    public override eGimmickType eGimmickType => eGimmickType.Helpful;
    
    [SerializeField] private FlowerGenerator mFlowerGenerator;
    [SerializeField] private FlowerController_Unique mUniqueControllerPrefab;
    
    private void OnEnable()
    {
        if (GimmickManager.Instance != null)
        {
            if (GimmickManager.Instance.UnimoPrefab.TryGetComponent(out IDamageAble iDamageAble))
            {
                mbTimeElapsed = 0f; //시간 초기화
            }
            else
            {
                MyDebug.Log("There is no UnimoPrefab attached to this object.");
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    
    private void Start()
    {
        mUniqueControllerPrefab.InitFlower(mFlowerGenerator);
    }

    private void Update()
    {
        base.Update();
    }
    
    public override async void ActivateGimmick()
    {
        MyDebug.Log("Activate Gimmick");
        
        Vector2 randomPos = Random.insideUnitCircle * ( PlaySystemRefStorage.mapSetter.MaxRange - 2 );
        gameObject.transform.position = new Vector3(randomPos.x, 0, randomPos.y);
        
        gameObject.SetActive(true);
        await FadeAll(true);

        mbReadyExecute = true;
    }

    public override async void DeactivateGimmick()
    {
        MyDebug.Log("Deactivate Gimmick");
        
        mbReadyExecute = false;
        
        await FadeAll(false);

        gameObject.SetActive(false);
    }
}