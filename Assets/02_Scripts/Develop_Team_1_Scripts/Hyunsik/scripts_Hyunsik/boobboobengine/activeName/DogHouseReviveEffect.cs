using UnityEngine;

[DisallowMultipleComponent] // 중복 AddComponent 방지
public class DogHouseReviveEffect : MonoBehaviour, IBoomBoomEngineEffect
{
    [SerializeField][Range(0.01f, 1f)] private float revivePercent = 0.05f;
    private bool bReviveUsed = false;
    public bool IsReviveUsed => bReviveUsed; // 읽기전용
    private PlayerStatManager mStatManager;
    
    void Start() {
        var arr = GetComponents<DogHouseReviveEffect>();
        Debug.Log($"[DogHouseReviveEffect] Player에 붙은 개수: {arr.Length}");
    }
    
    private void Awake()
    {
        // 반드시 플레이어에 붙어야 함
        mStatManager = GetComponent<PlayerStatManager>();
        if (mStatManager == null)
        {
            Debug.LogError("[도베르만] PlayerStatManager 없음! DogHouseReviveEffect는 플레이어에만 붙여야 함");
        }
    }

    public void Init(int engineID, int level)
    {
        var data = BoomBoomEngineDatabase.GetEngineData(engineID);
        if (data?.GrowthTable != null && data.GrowthTable.Length > level)
        {
            revivePercent = data.GrowthTable[level];
            Debug.Log($"[도베르만] Init 완료 ▶ RevivePercent = {revivePercent:P} (레벨 {level})");
        }
        else
        {
            revivePercent = 0.05f;
            Debug.LogWarning("[도베르만] GrowthTable 정보 없음 ▶ 기본값 5% 사용");
        }

        bReviveUsed = false; // 초기화 시 항상 리셋
    }
    
    public void ExecuteEffect()
    {
        // 스테이지 시작 등에서 호출해 리셋
        bReviveUsed = false;
        Debug.Log("[도베르만] ExecuteEffect()로 bReviveUsed를 false로 리셋");
    }

    public bool TryRevive(PlayerStatManager statManager)
    {
        Debug.Log($"[도베르만] TryRevive 호출! bReviveUsed={bReviveUsed}");
        if (!IsReviveUsed)
        {
            
            float maxHp = statManager.GetStat().BaseStat.Health;
            float reviveHp = Mathf.Max(1f, maxHp * revivePercent);
            statManager.ForceRevive(reviveHp);
            bReviveUsed = true;   // **값 세팅은 무조건 private 필드로**
            Debug.Log($"[도베르만] 부활 성공, bReviveUsed={bReviveUsed}");
            return true;
        }
        Debug.Log("[도베르만] 이미 부활됨, 무시!");
        return false;
    }
    
}