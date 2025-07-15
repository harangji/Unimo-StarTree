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
    // [field: SerializeField] public string GimmickID { get; set; } //??????????�ʿ�?
    [Header("��� ����")] [LabelText("��� �̸�")] [Tooltip("��� ���� �� UI�� ǥ���� ��� �̸�.")] [Required]
    public string gimmickName;

    [LabelText("��� ������")] [Tooltip("��� ���� �� UI�� ǥ���� ������.")] [Required]
    public Sprite gimmickIcon;

    [LabelText("��� ���� �ð�")] [Tooltip("����� �����Ǵ� �ð��Դϴ�. Ȱ��ȭ �ð� �Ǵ� ���� �ð�")] [Required]
    public float gimmickDuration;

    [LabelText("��� ȿ��(����) ���� �ð�")] [Tooltip("����� �ο��ϴ� ������ ���� �ð�")] [Required]
    public float gimmickEffectDuration;
    
    [LabelText("��� ���")][Tooltip("��� ��� ���� �� ��� �Ҹ�. ���� ������� ������ �� �ִ� ����� ã�� �� �ȿ��� ����ġ�� ���� ����")] [Required]
    public int gimmickCost;

    [LabelText("����ġ")] [Tooltip("����� �������� ������ �� ���̴� ��")] [Required]
    public string gimmickWeight;

    [ReadOnly] public Grade gimmickGrade; // �������� ������ ��� ���

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
    
    public void SetModeName(TextMeshProUGUI modeTxt) // Tmp�� �޾Ƽ� text�� ����̸����� ����
    {
        modeTxt.text = gimmickName;
    }
}
