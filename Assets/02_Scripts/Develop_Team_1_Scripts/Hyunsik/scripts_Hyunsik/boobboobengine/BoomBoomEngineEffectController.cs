using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 붕붕엔진 효과 관리 컨트롤러
/// </summary>
public class BoomBoomEngineEffectController : MonoBehaviour
{
    [SerializeField] private AuraRangeBoostEffect auraRangeEffect;

    private Dictionary<int, IBoomBoomEngineEffect> mEffectTable;

    private void Awake()
    {
        // SkillID 별 효과 매핑
        mEffectTable = new Dictionary<int, IBoomBoomEngineEffect>
        {
            { 303, auraRangeEffect }
            // 필요하면 추가 등록
        };
    }

    public void ActivateEffect(int skillID)
    {
        if (mEffectTable.TryGetValue(skillID, out var effect))
        {
            Debug.Log($"[엔진효과] SkillID {skillID} 효과 실행");
            effect.ExecuteEffect();
        }
        else
        {
            Debug.LogWarning($"[엔진효과] SkillID {skillID} 등록된 효과 없음");
        }
    }
}