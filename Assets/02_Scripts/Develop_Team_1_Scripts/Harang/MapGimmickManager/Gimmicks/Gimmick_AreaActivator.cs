using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class Gimmick_AreaActivator : Gimmick
{
    [Header("���� Ȱ��ȭ ����")]
    
    public override eGimmickType eGimmickType => eGimmickType.Helpful;
    
    [field: SerializeField, LabelText("���� Ȱ��ȭ ũ��"), Required, Space]
    private float EffectiveRange { get; set; } = 6.7f;
    
    [SerializeField] private FlowerGenerator flowerGenerator;
    private List<FlowerController> mFlowerControllers;
    
    private void OnEnable()
    {
        if (GimmickManager.Instance != null)
        {
            if (GimmickManager.Instance.UnimoPrefab.TryGetComponent(out IDamageAble iDamageAble))
            {
                mbTimeElapsed = 0f; //�ð� �ʱ�ȭ
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

    //cash
    private float mFlowerDistance;
    private float mGrowthperSec = 12f; //�⺻ �� ���� ����
    
    private void Update()
    {
        base.Update();
    }

    private void FixedUpdate()
    {
        if (!mbReadyExecute) return;

        mFlowerControllers = flowerGenerator.AllFlowers; //�� ���� List������ ���� ���� ����
        
        //�Ÿ� ��
        foreach (FlowerController flower in mFlowerControllers)
        {
            mFlowerDistance = Vector3.Distance(transform.position, flower.transform.position);
            if (mFlowerDistance <= EffectiveRange)
            {
                flower.AuraAffectFlower(mGrowthperSec * Time.fixedDeltaTime);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, EffectiveRange);
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