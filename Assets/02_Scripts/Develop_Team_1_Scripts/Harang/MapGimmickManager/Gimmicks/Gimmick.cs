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
    // [field: SerializeField] public string GimmickID { get; set; } //??????????필요?
    [Header("기믹 세팅")] [LabelText("기믹 이름")] [Tooltip("기믹 출현 시 UI에 표현할 기믹 이름.")] [Required]
    public string gimmickName;

    [LabelText("기믹 아이콘")] [Tooltip("기믹 출현 시 UI에 표현할 아이콘.")] [Required]
    public Sprite gimmickIcon;

    [LabelText("기믹 유지 시간")] [Tooltip("기믹이 유지되는 시간입니다. 활성화 시간 또는 버프 시간")] [Required]
    public float gimmickDuration;

    [LabelText("기믹 효과(버프) 지속 시간")] [Tooltip("기믹이 부여하는 버프의 지속 시간")] [Required]
    public float gimmickEffectDuration;
    
    [LabelText("기믹 비용")][Tooltip("기믹 사용 결정 시 비용 소모. 남은 비용으로 시전할 수 있는 기믹을 찾고 그 안에서 가중치로 랜덤 선택")] [Required]
    public int gimmickCost;

    [LabelText("가중치")] [Tooltip("기믹을 랜덤으로 결정할 때 쓰이는 값")] [Required]
    public string gimmickWeight;

    [ReadOnly] public Grade gimmickGrade; // 동적으로 설정할 기믹 등급

    private void Awake()
    {
        InitializeGimmick();
    }

    protected virtual void InitializeGimmick()
    {
        SetGrade();
    }
    
    public abstract void ExcuteGimmick();

    public void SetGrade()
    {
        
    }
    
    public void SetModeName(TextMeshProUGUI modeTxt) // Tmp를 받아서 text를 기믹이름으로 설정
    {
        modeTxt.text = gimmickName;
    }
}
