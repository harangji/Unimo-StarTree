using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class Gimmick_AreaActivator : Gimmick
{
    [Header("지역 활성화 설정")]
    public override eGimmickType eGimmickType => eGimmickType.Helpful;
    
    [field: SerializeField, LabelText("지역 활성화 크기"), Required, Space]
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
        mbTimeElapsed = 0f; //시간 초기화
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
            
            if (mDistance <= EffectiveRange) //플레이어가 반경 안에 위치
            {
                mbTimeElapsedActiveArea += Time.deltaTime;
            
                float t = Mathf.Clamp01(mbTimeElapsedActiveArea / mbActiveAreaDuration);
            
                activeArea.transform.localScale = Vector3.one * Mathf.Lerp(0f, 4.1f, t);
                
                if (mbTimeElapsedActiveArea >= mbActiveAreaDuration) //활성화 시간 전부 채움
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

        if (mbTimeElapsed >= mGimmickDuration && !mbDeactivateStart) //지속 시간 경과 시 비활성 && 조건 만족시 한 번만
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
    private float mGrowthperSec = 12f; //기본 꽃 성장 배율
    
    private void GlowFlowerWithDistance()
    {
        mFlowerControllers = new List<FlowerController>(flowerGenerator.AllFlowers); //비교 도중 List변경을 막기 위해 복사
        
        //거리 비교
        foreach (FlowerController flower in mFlowerControllers)
        {
            if(!flower) continue; //복사된 리스트에 실제로 플라워가 없을 수 있으니 검사
            
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