using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterFactory : MonoBehaviour
{
    protected class PatternGroup
    {
        public int Remaining;
        public int Cost;
    }
    protected List<PatternGroup> ActiveGroups = new List<PatternGroup>();

    [SerializeField] protected GameObject monsterPattern1;
    [SerializeField] protected GameObject monsterPattern2;
    [SerializeField] protected GameObject monsterPattern3;
    protected Transform PlayerTransform;

    private void Start()
    {
        PlayerMover mover = FindAnyObjectByType<PlayerMover>();
        if (mover != null)
        {
            PlayerTransform = mover.transform;
        }
    }

    #region 순수 가상 함수

    /// <summary>
    /// 확률 기반으로 몬스터를 소환합니다. 순수 가상 함수이므로 재정이가 필수로 필요합니다.
    /// </summary>
    /// <param name="allowedPattern">허용된 패턴 수 (패턴 그룹 확률 범위 결정)</param>
    /// <returns>소환된 주 몬스터 컨트롤러</returns>
    public abstract MonsterController SpawnMonster(int allowedPattern);

    /// <summary>
    /// 확률(rate)에 따라 필요한 코스트를 반환합니다. 순수 가상 함수이므로 재정이가 필수로 필요합니다.
    /// </summary>
    /// <param name="rate">난수로 정해진 rate 값</param>
    /// <param name="allowedPattern">허용된 패턴 수 (패턴 그룹 확률 범위 결정)</param>
    /// <returns>해당 rate에 따른 소모 코스트</returns>
    protected abstract int GetCostFromRate(int rate, int allowedPattern);

    #endregion

    #region 가상 함수    

    /// <summary>
    /// 몬스터를 생성할 위치를 계산합니다.
    /// </summary>
    /// <returns>생성 위치 벡터</returns>
    protected virtual Vector3 FindGenPosition()
    {
        return Vector3.zero;
    }

    /// <summary>
    /// 생성 위치에 맞는 회전 값을 계산합니다.
    /// </summary>
    /// <param name="genPos">생성 위치</param>
    /// <returns>계산된 회전 쿼터니언</returns>
    protected virtual Quaternion SetGenRotation(Vector3 genPos)
    {
        return Quaternion.identity;
    }

    #endregion

    #region 멤버 함수

    /// <summary>
    /// 몬스터의 첫 번째 패턴을 소환하는 기본 로직입니다.
    /// </summary>
    /// <param name="allowedPattern">허용된 패턴 수</param>
    /// <returns>소환된 주 몬스터 컨트롤러</returns>
    protected MonsterController DefaultSpawn(int allowedPattern)
    {
        Vector3 pos = FindGenPosition();
        Quaternion quat = SetGenRotation(pos);
        MonsterController controller = Instantiate(monsterPattern1, pos, quat).GetComponent<MonsterController>();
        controller.InitEnemy(PlayerTransform);

        return controller;
    }

    /// <summary>
    /// 몬스터가 파괴될 때 호출되는 콜백을 등록합니다.
    /// </summary>
    /// <param name="ctrl">대상 몬스터 컨트롤러</param>
    /// <param name="group">소속된 패턴 그룹</param>
    protected void RegisterDestroyCallback(MonsterController ctrl, PatternGroup group)
    {
        ctrl.OnDestroyed += (monster) =>
        {
            group.Remaining--;
            if (group.Remaining == 0)
            {
                PlaySystemRefStorage.stageManager.RestoreDifficulty(group.Cost);
                ActiveGroups.Remove(group);
            }
        };
    }
    
    /// <summary>
    /// 현재 StageManager에서 난이도 자원을 소비 시도합니다.
    /// </summary>
    /// <param name="cost">소모할 난이도 수치</param>
    /// <returns>성공 여부</returns>
    protected bool TryConsumeDifficulty(int cost)
    {
        var stageMgr = PlaySystemRefStorage.stageManager;
        if (stageMgr == null || !stageMgr.TryConsumeDifficulty(cost))
        {
            Debug.Log("할당할 난이도 수치가 부족합니다.");
            return false;
        }
        return true;
    }

    #endregion
}