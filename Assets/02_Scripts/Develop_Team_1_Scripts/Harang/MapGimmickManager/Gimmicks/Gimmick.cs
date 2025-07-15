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
    [Header("��� ����")] 
    
    [LabelText("��� �̸�")] [Tooltip("��� ���� �� UI�� ǥ���� ��� �̸�.")] [Required]
    public string gimmickName;

    [LabelText("��� ������")] [Tooltip("��� ���� �� UI�� ǥ���� ������.")] [Required]
    public Sprite gimmickIcon;

    [LabelText("��� ���� �ð�")] [Tooltip("����� �����Ǵ� �ð��Դϴ�. Ȱ��ȭ �ð� �Ǵ� ���� �ð�")] [Required]
    public float gimmickDuration;

    [LabelText("��� ȿ�� ��ġ 1")]
    public float gimmickEffectValue1;
    
    [LabelText("��� ȿ�� ��ġ 2")]
    public float gimmickEffectValue2;
    
    [LabelText("��� ���")][Tooltip("��� ��� ���� �� ��� �Ҹ�. ���� ������� ������ �� �ִ� ����� ã�� �� �ȿ��� ����ġ�� ���� ����")] [Required]
    public int[] gimmickCosts;

    [LabelText("����ġ��")] [Tooltip("����� �������� ������ �� ���̴� ��")] [Required]
    public int[] gimmickWeights;

    //ReadOnly
    [LabelText("��� ���")][Tooltip("����� ������ �� �������� �Ҵ�Ǵ� ����� ���")][ReadOnly]
    public Grade gimmickGrade; // �������� ������ ��� ���
    
    [LabelText("�ʱ�ȭ �Ϸ� ����")][ReadOnly]
    protected bool GimmickInitialize = false; // �������� ������ ��� ���
    
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
    
    public void SetModeName(TextMeshProUGUI modeTxt) // Tmp�� �޾Ƽ� text�� ����̸����� ����
    {
        modeTxt.text = gimmickName;
    }
}
