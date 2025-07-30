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
    //CsvHelper�� ������ ������Ƽ�� �ڵ� ������ �� �ִ�.
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
    [Header("��� ����")] 
    
    [LabelText("��� �̸�"), Tooltip("��� ���� �� UI�� ǥ���� ��� �̸�."), Required]
    public string gimmickName;

    [LabelText("��� ������"), Tooltip("��� ���� �� UI�� ǥ���� ������."), Required]
    public Sprite gimmickIcon;
    
    //ReadOnly
    [LabelText("��� ���"), Tooltip("����� ������ �� �������� �Ҵ�Ǵ� ����� ���"), ReadOnly]
    protected eGimmickGrade eGimmickGrade { get; set; } // �������� ������ ��� ���

    [LabelText("��� Ÿ��"), Tooltip("�طο� ���, �̷ο� ��� ����"), ReadOnly]
    public abstract eGimmickType eGimmickType { get;}
    
    [LabelText("��� ���� �ð�"), ReadOnly] 
    protected int bGimmickCost { get; set; }
    
    [LabelText("��� ���� �ð�"), ReadOnly] 
    protected float bGimmickDuration { get; set; }
    
    [LabelText("��� ȿ�� ��ġ 1"), ReadOnly]
    protected float bGimmickEffectValue1 { get; set; }
    
    [LabelText("��� ȿ�� ��ġ 2"), ReadOnly]
    protected float bGimmickEffectValue2 { get; set; }
    
    public abstract void ActivateGimmick(); //����
    public abstract void DeactivateGimmick(); //����

    //gimmickGrade�� �ڱ� �ڽ��� �ʱ�ȭ //��� ����, �غ� �ܰ�
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
