using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �غؿ��� ȿ�� ���� ��Ʈ�ѷ�
/// </summary>
public class BoomBoomEngineEffectController : MonoBehaviour
{
    [SerializeField] private AuraRangeBoostEffect auraRangeEffect;

    private Dictionary<int, IBoomBoomEngineEffect> mEffectTable;

    private void Awake()
    {
        // SkillID �� ȿ�� ����
        mEffectTable = new Dictionary<int, IBoomBoomEngineEffect>
        {
            { 303, auraRangeEffect }
            // �ʿ��ϸ� �߰� ���
        };
    }

    public void ActivateEffect(int skillID)
    {
        if (mEffectTable.TryGetValue(skillID, out var effect))
        {
            Debug.Log($"[����ȿ��] SkillID {skillID} ȿ�� ����");
            effect.ExecuteEffect();
        }
        else
        {
            Debug.LogWarning($"[����ȿ��] SkillID {skillID} ��ϵ� ȿ�� ����");
        }
    }
}