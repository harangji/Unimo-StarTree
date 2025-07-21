using System;
using Sirenix.OdinInspector;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;

public enum GimmickGrade
{
    Nomal,
    Enhanced,
    Elite,
    Legendary,
}

public class ToTsvGimmickData
{
    //CsvHelper�� ������ ������Ƽ�� �ڵ� ������ �� �ִ�.
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
    [Header("��� ����")] 
    
    [LabelText("��� �̸�"), Tooltip("��� ���� �� UI�� ǥ���� ��� �̸�."), Required]
    public string gimmickName;

    [LabelText("��� ������"), Tooltip("��� ���� �� UI�� ǥ���� ������."), Required]
    public Sprite gimmickIcon;
    
    //ReadOnly
    [LabelText("��� ���"), Tooltip("����� ������ �� �������� �Ҵ�Ǵ� ����� ���"), ReadOnly]
    protected GimmickGrade ebGimmickGrade { get; set; } // �������� ������ ��� ���
    
    [LabelText("��� ���� �ð�"), ReadOnly] 
    protected int bGimmickCost { get; set; }
    
    [LabelText("��� ���� �ð�"), ReadOnly] 
    protected int bGimmickDuration { get; set; }
    
    [LabelText("��� ȿ�� ��ġ 1"), ReadOnly]
    protected float bGimmickEffectValue1 { get; set; }
    
    [LabelText("��� ȿ�� ��ġ 2"), ReadOnly]
    protected float bGimmickEffectValue2 { get; set; }
    
    [LabelText("�ʱ�ȭ �Ϸ� ����"), ReadOnly] 
    protected bool mbGimmickInitialize { get; set; } = false; // ��� �ʱ�ȭ ����

    public abstract void Activate();

    public void InitializeGimmick(GimmickInitializer.GimmickInitializerData gimmickFactoryData, GimmickGrade gimmickGrade)
    {
        ebGimmickGrade = gimmickGrade;
        bGimmickCost = gimmickFactoryData.Costs[(int)gimmickGrade];
        bGimmickDuration = gimmickFactoryData.Durations_s[(int)gimmickGrade];
        bGimmickEffectValue1 = gimmickFactoryData.EffectValue1[(int)gimmickGrade];
        bGimmickEffectValue2 = gimmickFactoryData.EffectValue2[(int)gimmickGrade];
        mbGimmickInitialize = true;
    }
}
