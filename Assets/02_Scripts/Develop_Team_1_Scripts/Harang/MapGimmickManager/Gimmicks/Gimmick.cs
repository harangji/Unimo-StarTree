using System;
using Sirenix.OdinInspector;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;

public enum eGimmickGrade
{
    Nomal,
    Enhanced,
    Elite,
    Legendary,
}

public enum eGimmickType
{
    BlackHole,
    RedZone
}

public class ToTsvGimmickData
{
    //CsvHelper는 정적인 프로퍼티만 자동 매핑할 수 있다.
    public string GimmickName { get; set; }
    public string GimmickID { get; set; }
    public int[] Costs { get; set; }
    public int[] Weights { get; set; }
    public int[] Durations_s { get; set; }
    public float[] EffectValue1 { get; set; }
    public float[] EffectValue2 { get; set; }
}

public abstract class Gimmick : MonoBehaviour
{
    [Header("기믹 세팅")] 
    
    [LabelText("기믹 이름"), Tooltip("기믹 출현 시 UI에 표현할 기믹 이름."), Required]
    public string gimmickName;

    [LabelText("기믹 아이콘"), Tooltip("기믹 출현 시 UI에 표현할 아이콘."), Required]
    public Sprite gimmickIcon;
    
    //ReadOnly
    [LabelText("기믹 등급"), Tooltip("기믹을 설정할 때 동적으로 할당되는 기믹의 등급"), ReadOnly]
    protected eGimmickGrade ebGimmickGrade { get; set; } // 동적으로 설정할 기믹 등급
    
    [LabelText("기믹 지속 시간"), ReadOnly] 
    protected int bGimmickCost { get; set; }
    
    [LabelText("기믹 지속 시간"), ReadOnly] 
    protected int bGimmickDuration { get; set; }
    
    [LabelText("기믹 효과 수치 1"), ReadOnly]
    protected float bGimmickEffectValue1 { get; set; }
    
    [LabelText("기믹 효과 수치 2"), ReadOnly]
    protected float bGimmickEffectValue2 { get; set; }
    
    [LabelText("초기화 완료 여부"), ReadOnly] 
    // protected bool mbGimmickInitialize { get; set; } = false; // 기믹 초기화 여부

    public abstract void ActivateGimmick(); //실행
    public abstract void DeactivateGimmick(); //정지

    //gimmickGrade로 자기 자신을 초기화 //기믹 생성, 준비 단계
    public void InitializeGimmick(GimmickInitializer gimmickInitializer, eGimmickGrade gimmickGrade)
    {
        ebGimmickGrade = gimmickGrade;
        bGimmickCost = gimmickInitializer.Costs[(int)gimmickGrade];
        bGimmickDuration = gimmickInitializer.Durations_s[(int)gimmickGrade];
        bGimmickEffectValue1 = gimmickInitializer.EffectValue1[(int)gimmickGrade];
        bGimmickEffectValue2 = gimmickInitializer.EffectValue2[(int)gimmickGrade];
        // mbGimmickInitialize = true;
    }
}
