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
        mbTimeElapsed = 0f; //�ð� �ʱ�ȭ
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

        mFlowerControllers = new List<FlowerController>(flowerGenerator.AllFlowers); //�� ���� List������ ���� ���� ����
        
        //�Ÿ� ��
        foreach (FlowerController flower in mFlowerControllers)
        {
            if(!flower) continue; //����� ����Ʈ�� ������ �ö���� ���� �� ������ �˻�
            
            mFlowerDistance = Vector3.Distance(transform.position, flower.transform.position);
            if (mFlowerDistance <= EffectiveRange && flower)
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
        Vector2 randomPos = Random.insideUnitCircle * ( PlaySystemRefStorage.mapSetter.MaxRange - 2 );
        gameObject.transform.position = new Vector3(randomPos.x, 0, randomPos.y);
        
        gameObject.SetActive(true);
        await FadeAll(true);

        mbReadyExecute = true;
    }

    public override async void DeactivateGimmick()
    {
        mbReadyExecute = false;
        
        await FadeAll(false);

        gameObject.SetActive(false);
    }
}