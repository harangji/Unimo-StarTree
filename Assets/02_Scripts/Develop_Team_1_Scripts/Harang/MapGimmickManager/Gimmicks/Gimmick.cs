using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.ResourceManagement.AsyncOperations;

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
    [Header("��� ����")]
    
    [LabelText("��� �̸�")]
    [Tooltip("��� ���� �� UI�� ǥ���� ��� �̸�.")][Required]
    [field: SerializeField] public string GimmickName { get; set; }
    
    [LabelText("��� ������")]
    [Tooltip("��� ���� �� UI�� ǥ���� ������.")][Required]
    [field: SerializeField] public Sprite GimmickIcon { get; set; }
    
    [LabelText("��� ���� �ð�")]
    [Tooltip("����� �����Ǵ� �ð��Դϴ�. Ȱ��ȭ �ð� �Ǵ� ���� �ð�")][Required]
    [field: SerializeField] public float GimmickDuration { get; set; }
    
    [LabelText("��� ȿ��(����) ���� �ð�")]
    [Tooltip("����� �ο��ϴ� ������ ���� �ð�")][Required]
    [field: SerializeField] public float GimmickEffectDuration { get; set; }
    
    [LabelText("��� ���")]
    [Tooltip("��� ��� ���� �� ��� �Ҹ�. ���� ������� ������ �� �ִ� ����� ã�� �� �ȿ��� ����ġ�� ���� ����")][Required]
    [field: SerializeField] public int Cost { get; set; }
    
    [LabelText("����ġ")]
    [Tooltip("����� �������� ������ �� ���̴� ��")][Required]
    [field: SerializeField] public string Weight { get; set; }
    
    [ReadOnly] public Grade GimmickGrade { get; set; } //�������� ������ ��� ���

    public abstract void InitializeGimmick();
    public abstract void ExcuteGimmick();

    // public abstract void ExcuteGimmick();
    
    public void SetModeName(TextMeshProUGUI modeTxt)
    {
        modeTxt.text = GimmickName;
    }

    public void SetGrade(int currentStage)
    {
        
    }
}
