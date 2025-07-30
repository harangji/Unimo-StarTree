using System;
using CsvHelper.Configuration.Attributes;
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

public enum eGimmicks
{
    BlackHole,
    RedZone
}

public enum eGimmickType
{
    Dangerous,
    Helpful
}

[Serializable]
public class ToTsvGimmickData : IKeyedData
{
    //CsvHelper는 정적인 프로퍼티만 자동 매핑할 수 있다.
    [field: SerializeField, ReadOnly] public string Key { get; set;}
    [field: SerializeField, ReadOnly] public string GimmickName { get; set; }
    
    [field: SerializeField, TypeConverter(typeof(IntArrayConverter)), ReadOnly] public int[] Costs { get; set; }
    [field: SerializeField, TypeConverter(typeof(IntArrayConverter)), ReadOnly] public int[] Weights { get; set; }
    [field: SerializeField, TypeConverter(typeof(FloatArrayConverter)), ReadOnly] public float[] Durations_s { get; set; }
    [field: SerializeField, TypeConverter(typeof(FloatArrayConverter)), ReadOnly] public float[] EffectValue1 { get; set; }
    [field: SerializeField, TypeConverter(typeof(FloatArrayConverter)), ReadOnly] public float[] EffectValue2 { get; set; }
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
    protected eGimmickGrade eGimmickGrade { get; set; } // 동적으로 설정할 기믹 등급

    [LabelText("기믹 타입"), Tooltip("해로운 기믹, 이로운 기믹 설정"), ReadOnly]
    public abstract eGimmickType eGimmickType { get;}
    
    [LabelText("기믹 지속 시간"), ReadOnly] 
    protected int bGimmickCost { get; set; }
    
    [LabelText("기믹 지속 시간"), ReadOnly] 
    protected float bGimmickDuration { get; set; }
    
    [LabelText("기믹 효과 수치 1"), ReadOnly]
    protected float bGimmickEffectValue1 { get; set; }
    
    [LabelText("기믹 효과 수치 2"), ReadOnly]
    protected float bGimmickEffectValue2 { get; set; }
    
    public abstract void ActivateGimmick(); //실행
    public abstract void DeactivateGimmick(); //정지

    //gimmickGrade로 자기 자신을 초기화 //기믹 생성, 준비 단계
    public void InitializeGimmick(GimmickInitializer gimmickInitializer, eGimmickGrade gimmickGrade)
    {
        eGimmickGrade = gimmickGrade;
        bGimmickCost = gimmickInitializer.Costs[(int)gimmickGrade];
        bGimmickDuration = gimmickInitializer.Durations_s[(int)gimmickGrade];
        bGimmickEffectValue1 = gimmickInitializer.EffectValue1[(int)gimmickGrade];
        bGimmickEffectValue2 = gimmickInitializer.EffectValue2[(int)gimmickGrade];
    }
    
    public void SetModeName(TextMeshProUGUI modeTxt)
    {
        modeTxt.text = gimmickName;
    }
}
