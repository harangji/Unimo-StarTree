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
    protected eGimmickGrade ebGimmickGrade { get; set; } // �������� ������ ��� ���
    
    [LabelText("��� ���� �ð�"), ReadOnly] 
    protected int bGimmickCost { get; set; }
    
    [LabelText("��� ���� �ð�"), ReadOnly] 
    protected int bGimmickDuration { get; set; }
    
    [LabelText("��� ȿ�� ��ġ 1"), ReadOnly]
    protected float bGimmickEffectValue1 { get; set; }
    
    [LabelText("��� ȿ�� ��ġ 2"), ReadOnly]
    protected float bGimmickEffectValue2 { get; set; }
    
    [LabelText("�ʱ�ȭ �Ϸ� ����"), ReadOnly] 
    // protected bool mbGimmickInitialize { get; set; } = false; // ��� �ʱ�ȭ ����

    public abstract void ActivateGimmick(); //����
    public abstract void DeactivateGimmick(); //����

    //gimmickGrade�� �ڱ� �ڽ��� �ʱ�ȭ //��� ����, �غ� �ܰ�
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
