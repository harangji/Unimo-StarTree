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
    
    private bool isActiveArea = false;
    private float mbTimeElapsedActiveArea = 0f;
    private float mbActiveAreaDuration = 2f;
    
    [SerializeField] private GameObject activeArea;
    [SerializeField] private GameObject activeAreaRing;
    
    [SerializeField] private Color activeColor;
    
    [SerializeField] private Renderer activeRenderer;
    [SerializeField] private Renderer activeLineRenderer;
    private float mDistance;
    
    private Transform mUnimoTransform;
    
    private void OnEnable()
    {
        mbTimeElapsed = 0f; //�ð� �ʱ�ȭ
        isActiveArea = false;
        mbTimeElapsedActiveArea = 0f;
        activeArea.transform.localScale = new Vector3(0, 0, 0);
        activeArea.SetActive(true);

        mUnimoTransform = GimmickManager.Instance.UnimoPrefab.transform;
    }
    
    private void Update()
    {
        if (!isActiveArea)
        {
            mDistance = Vector3.Distance(transform.position, mUnimoTransform.position);
            
            if (mDistance <= EffectiveRange) //�÷��̾ �ݰ� �ȿ� ��ġ
            {
                mbTimeElapsedActiveArea += Time.deltaTime;
            
                float t = Mathf.Clamp01(mbTimeElapsedActiveArea / mbActiveAreaDuration);
            
                activeArea.transform.localScale = Vector3.one * Mathf.Lerp(0f, 4.1f, t);
                
                if (mbTimeElapsedActiveArea >= mbActiveAreaDuration) //Ȱ��ȭ �ð� ���� ä��
                {
                    isActiveArea = true;
                    activeArea.SetActive(false);
                    activeAreaRing.SetActive(true);
                
                    activeRenderer.material.color = activeColor;
                    activeLineRenderer.material.color = activeColor;
                }
            }
            
            return;
        }
        
        mbTimeElapsed += Time.deltaTime;

        if (mbTimeElapsed >= mGimmickDuration && !mbDeactivateStart) //���� �ð� ��� �� ��Ȱ�� && ���� ������ �� ����
        {
            mbDeactivateStart = true;
            DeactivateGimmick();
        }
    }

    private void FixedUpdate()
    {
        if (!mbReadyExecute || !isActiveArea) return;

        GlowFlowerWithDistance();
    }

    [SerializeField] private FlowerGenerator flowerGenerator;
    private List<FlowerController> mFlowerControllers;
    private float mFlowerDistance;
    private float mGrowthperSec = 12f; //�⺻ �� ���� ����
    
    private void GlowFlowerWithDistance()
    {
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
        mbDeactivateStart = true;
        
        await FadeAll(false);

        gameObject.SetActive(false);
    }
}