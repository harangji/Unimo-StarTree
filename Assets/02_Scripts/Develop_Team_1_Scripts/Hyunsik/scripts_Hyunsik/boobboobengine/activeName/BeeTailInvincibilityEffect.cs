using System.Collections;
using UnityEngine;

public class BeeTailInvincibilityEffect : MonoBehaviour, IBoomBoomEngineEffect
{
    public int EngineID { get; set; }
    public int EngineLevel { get; set; }

    private float duration; // GrowthTable 기반 무적 시간

    private PlayerStatManager mStatManager;
    private bool bIsBuffActive = false;

    /// <summary>
    /// 필요한 경우 외부에서 무적 시간 초기화 (GrowthTable 기반)
    /// </summary>
    public void Init(int engineID, int level)
    {
        EngineID = engineID;
        EngineLevel = level;

        var data = BoomBoomEngineDatabase.GetEngineData(engineID);
        if (data == null || data.GrowthTable == null || data.GrowthTable.Length == 0)
        {
            Debug.LogError($"[BeeTail] EngineData 오류 - ID: {engineID}");
            duration = 0f;
            return;
        }

        duration = data.GrowthTable[Mathf.Clamp(level - 1, 0, data.GrowthTable.Length - 1)];
        Debug.Log($"[BeeTail] Init 완료 - ID: {engineID}, Level: {level}, Duration: {duration}");
    }

    /// <summary>
    /// 필요할 때마다 최신 PlayerStatManager를 가져옴
    /// </summary>
    private PlayerStatManager StatManager
    {
        get
        {
            if (mStatManager == null)
            {
                mStatManager = PlaySystemRefStorage.playerStatManager;
            }
            return mStatManager;
        }
    }

    public void ExecuteEffect()
    {
        if (bIsBuffActive)
            return;

        if (duration <= 0f)
        {
            Debug.LogWarning($"[BeeTail] duration이 0 이하임. Init 호출 여부 확인 필요");
            return;
        }

        StartCoroutine(ApplyInvincibilityBuff());
    }

    private IEnumerator ApplyInvincibilityBuff()
    {
        bIsBuffActive = true;
        StatManager.SetTemporaryInvincibility(true);

        yield return new WaitForSeconds(duration);

        StatManager.SetTemporaryInvincibility(false);
        bIsBuffActive = false;

        Debug.Log($"[BeeTail] 무적 해제 완료 ({duration}초)");
    }
}