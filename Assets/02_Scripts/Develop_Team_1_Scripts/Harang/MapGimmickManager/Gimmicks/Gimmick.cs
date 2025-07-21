using System;
using Sirenix.OdinInspector;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;

public enum Grade
{
    Nomal,
    Enhanced,
    Elite,
    Legendary,
}

public abstract class Gimmick : MonoBehaviour
{
    [Header("기믹 세팅")] 
    
    [LabelText("기믹 이름")] [Tooltip("기믹 출현 시 UI에 표현할 기믹 이름.")] [Required]
    public string gimmickName;

    [LabelText("기믹 아이콘")] [Tooltip("기믹 출현 시 UI에 표현할 아이콘.")] [Required]
    public Sprite gimmickIcon;

    [LabelText("기믹 유지 시간")] [Tooltip("기믹이 유지되는 시간입니다. 활성화 시간 또는 버프 시간")] [Required]
    public float gimmickDuration;

    [LabelText("기믹 효과 수치 1")]
    public float gimmickEffectValue1;
    
    [LabelText("기믹 효과 수치 2")]
    public float gimmickEffectValue2;
    
    [LabelText("기믹 비용")][Tooltip("기믹 사용 결정 시 비용 소모. 남은 비용으로 시전할 수 있는 기믹을 찾고 그 안에서 가중치로 랜덤 선택")] [Required]
    public int[] gimmickCosts;

    [LabelText("가중치들")] [Tooltip("기믹을 랜덤으로 결정할 때 쓰이는 값")] [Required]
    public int[] gimmickWeights;

    //ReadOnly
    [LabelText("기믹 등급")][Tooltip("기믹을 설정할 때 동적으로 할당되는 기믹의 등급")][ReadOnly]
    public Grade gimmickGrade; // 동적으로 설정할 기믹 등급
    
    [LabelText("초기화 완료 여부")][ReadOnly]
    protected bool GimmickInitialize = false; // 동적으로 설정할 기믹 등급
    
    private void Start()
    {
        InitializeGimmick();
        GimmickInitialize = true;
    }

    protected abstract void InitializeGimmick();
    
    public abstract void ExcuteGimmick();
    
    public void SetGrade(Grade grade)
    {
        gimmickGrade = grade;
    }
    
    public void SetModeName(TextMeshProUGUI modeTxt) // Tmp를 받아서 text를 기믹이름으로 설정
    {
        modeTxt.text = gimmickName;
    }
}
