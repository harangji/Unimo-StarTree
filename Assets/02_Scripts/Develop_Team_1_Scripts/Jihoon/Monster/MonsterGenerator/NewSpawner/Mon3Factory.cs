using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mon3Factory : MonsterFactory
{
    private float innerRadius = 2.5f;
    private float outerRadius = 8f;
    
    #region 재정의 메서드

    public override MonsterController SpawnMonster(int allowedPattern)
    {
        int cost = GetCostFromRate(Random.Range(0, 100), allowedPattern);

        if (!TryConsumeDifficulty(cost)) return null;

        int spawnCount = cost switch
        {
            2 => 1,
            4 => 5,
            _ => 8
        };
        var group = new PatternGroup { Remaining = spawnCount, Cost = cost };
        ActiveGroups.Add(group);

        if (cost == 2)
        {
            var ctrl = DefaultSpawn(0);
            ctrl.InitEnemy(PlayerTransform);
            RegisterDestroyCallback(ctrl, group);
        }
        else if (cost == 4)
        {
            StartPattern2(group);
        }
        else
        {
            StartPattern3(group);
        }

        return null;
    }

    protected override int GetCostFromRate(int rate, int allowedPattern)
    {
        switch (allowedPattern)
        {
            case 1:
                return 2;
            case 2:
                return rate switch
                {
                    < 70 => 2,
                    _ => 4
                };
            case 3:
                return rate switch
                {
                    < 60 => 2,
                    < 85 => 4,
                    _ => 7
                };
        }

        return 0;
    }

    protected override Vector3 FindGenPosition()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        float rand = Random.Range(0f, 1f);
        rand *= rand;
        float radius = innerRadius + rand * (outerRadius - innerRadius);
        float angle = 2f * Mathf.PI * Random.Range(0f, 1f);

        Vector3 pos = new Vector3(radius * Mathf.Cos(angle), 0f, radius * Mathf.Sin(angle));
        pos += PlayerTransform.position;
        if (PlaySystemRefStorage.mapSetter.IsInMap(pos) == false)
        {
            pos = PlaySystemRefStorage.mapSetter.FindNearestPoint(pos);
        }

        return pos;
    }

    protected override Quaternion SetGenRotation(Vector3 genPos)
    {
        float rand = Random.Range(-20f, 20f);
        Quaternion quat = Quaternion.Euler(0f, 180f + rand, 0f);
        return quat;
    }
    
    #endregion

    #region 내부 정의 메서드

    /// <summary>
    /// 패턴 2: 중심을 기준으로 5마리의 몬스터를 오각형 형태로 생성합니다.
    /// </summary>
    /// <param name="group">소환된 몬스터가 속할 패턴 그룹</param>
    private void StartPattern2(PatternGroup group)
    {
        float angle;
        Vector3 center = PlayerTransform.position;
        if (center.magnitude > PlaySystemRefStorage.mapSetter.MaxRange - 6f)
        {
            center *= (PlaySystemRefStorage.mapSetter.MaxRange - 6f) / center.magnitude;
        }

        for (int i = 0; i < 5; i++)
        {
            angle = 0.4f * Mathf.PI * i;
            Vector3 pos = center + 4.8f * new Vector3(Mathf.Sin(angle), 0f, Mathf.Cos(angle));
            MonsterController controller = Instantiate(monsterPattern1, pos, Quaternion.identity)
                .GetComponent<MonsterController>();
            controller.InitEnemy(PlayerTransform);
            RegisterDestroyCallback(controller, group);
        }
    }

    /// <summary>
    /// 패턴 3: 8방향에 몬스터를 소환하고, 일부 몬스터의 폭발을 딜레이시켜 연출 효과를 부여합니다.
    /// </summary>
    /// <param name="group">소환된 몬스터가 속할 패턴 그룹</param>
    private void StartPattern3(PatternGroup group)
    {
        int firstBombCount = 0;
        Vector3 center = PlayerTransform.position;
        Vector3[] spawnOffsets = new Vector3[]
        {
            new Vector3(-3f, 0, 6f), // 앞왼
            new Vector3(6f, 0, 3f), // 오앞
            new Vector3(3f, 0, -6f), // 뒤오
            new Vector3(-6f, 0, -3f), // 왼뒤

            new Vector3(3f, 0, 6f), // 앞오
            new Vector3(6f, 0, -3f), // 오뒤
            new Vector3(-3f, 0, -6f), // 뒤왼
            new Vector3(-6f, 0, 3f) // 왼앞
        };

        var actions = new List<Mon003State_Action_C>();

        foreach (var spawnOffset in spawnOffsets)
        {
            var obj = Instantiate(monsterPattern1, center + spawnOffset, Quaternion.identity);
            MonsterController controller = obj.GetComponent<MonsterController>();
            controller.InitEnemy(PlayerTransform);
            RegisterDestroyCallback(controller, group);

            if (firstBombCount < 4)
            {
                firstBombCount++;

                var component = obj.GetComponentInChildren<Mon003State_Action_C>();
                component.canBomb = false;

                actions.Add(component);
            }
        }

        StartCoroutine(DelayBomb(actions));
    }

    /// <summary>
    /// 지정된 Mon003State_Action_C 리스트에 대해 일정 시간 후 폭발 동작을 트리거합니다.
    /// </summary>
    /// <param name="actions">지연 후 폭발시킬 액션 컴포넌트 리스트</param>
    /// <returns>코루틴 IEnumerator</returns>
    private IEnumerator DelayBomb(List<Mon003State_Action_C> actions)
    {
        yield return new WaitForSeconds(2.75f);

        foreach (var action in actions)
        {
            action.canBomb = true;
            action.OnTriggerAction();
        }
    }

    #endregion
}