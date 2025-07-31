using System;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFactory : MonoBehaviour
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

    /// <summary>
    /// 지정된 패턴을 기준으로 몬스터를 생성합니다.
    /// </summary>
    /// <param name="allowedPattern">허용된 패턴 인덱스</param>
    /// <returns>생성된 몬스터 컨트롤러</returns>
    public virtual MonsterController SpawnMonster(int allowedPattern)
    {
        Vector3 pos = FindGenPosition();
        Quaternion quat = SetGenRotation(pos);
        MonsterController controller = Instantiate(monsterPattern1, pos, quat).GetComponent<MonsterController>();
        controller.InitEnemy(PlayerTransform);

        return controller;
    }

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
}