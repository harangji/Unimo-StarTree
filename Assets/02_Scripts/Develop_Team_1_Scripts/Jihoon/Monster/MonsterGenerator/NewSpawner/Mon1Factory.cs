using System.Collections.Generic;
using UnityEngine;

public class Mon1Factory : MonsterFactory
{
    private float genRadius = 20f;
    private float randDirAngle = 40f;

    /// <summary>
    /// 확률 기반으로 몬스터를 소환합니다.
    /// </summary>
    /// <param name="allowedPattern">허용된 패턴 수</param>
    /// <returns>소환된 주 몬스터 컨트롤러</returns>
    public override MonsterController SpawnMonster(int allowedPattern)
    {
        var maxRate = 33 * allowedPattern;
        int rate = Random.Range(0, maxRate);

        int cost = GetCostFromRate(rate);
        if (!TryConsumeDifficulty(cost)) return null;

        int spawnCount = rate < 33 ? 1 : 8;
        var group = new PatternGroup { Remaining = spawnCount, Cost = cost };
        ActiveGroups.Add(group);

        var prefab = GetPrefabFromRate(rate);
        float offset = rate < 66 ? 6.5f : 7.5f;

        return spawnCount == 1
            ? SpawnSingle(prefab, group, out var single) ? single : null
            : SpawnGroup(prefab, offset, group);
    }

    /// <summary>
    /// 적절한 코스트를 rate 값으로부터 계산합니다.
    /// </summary>
    private int GetCostFromRate(int rate)
    {
        if (rate < 33) return 2;
        if (rate < 66) return 4;
        return 5;
    }

    /// <summary>
    /// 난이도 자원을 소비합니다. 실패 시 로그 출력.
    /// </summary>
    private bool TryConsumeDifficulty(int cost)
    {
        var stageMgr = PlaySystemRefStorage.stageManager;
        if (stageMgr == null || !stageMgr.TryConsumeDifficulty(cost))
        {
            Debug.Log("할당할 난이도 수치가 부족합니다.");
            return false;
        }
        return true;
    }

    /// <summary>
    /// 확률에 따라 사용할 몬스터 프리팹을 선택합니다.
    /// </summary>
    private GameObject GetPrefabFromRate(int rate)
    {
        return rate < 66 ? monsterPattern1 : monsterPattern3;
    }

    /// <summary>
    /// 단일 몬스터를 소환합니다.
    /// </summary>
    private bool SpawnSingle(GameObject prefab, PatternGroup group, out MonsterController ctrl)
    {
        ctrl = base.SpawnMonster(0);
        if (ctrl == null) return false;

        ctrl.InitEnemy(PlayerTransform);
        RegisterDestroyCallback(ctrl, group);
        return true;
    }

    /// <summary>
    /// 8마리의 몬스터를 원형으로 소환합니다.
    /// </summary>
    private MonsterController SpawnGroup(GameObject prefab, float offset, PatternGroup group)
    {
        Vector3 center = ClampToArena(PlayerTransform.position, PlaySystemRefStorage.mapSetter.MaxRange - 7.2f);
        MonsterController primary = null;

        for (int i = 0; i < 8; i++)
        {
            float angle = Mathf.PI / 4f * i;
            Vector3 pos = center + offset * new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
            Quaternion rot = Quaternion.LookRotation(center - pos, Vector3.up);

            var ctrl = Instantiate(prefab, pos, rot).GetComponent<MonsterController>();
            ctrl.InitEnemy(PlayerTransform);
            RegisterDestroyCallback(ctrl, group);

            if (primary == null) primary = ctrl;
        }

        return primary;
    }

    /// <summary>
    /// 맵 범위 내 좌표로 제한합니다.
    /// </summary>
    private Vector3 ClampToArena(Vector3 position, float maxRadius)
    {
        return position.magnitude > maxRadius ? position.normalized * maxRadius : position;
    }

    /// <summary>
    /// 소환 위치를 계산합니다.
    /// </summary>
    protected override Vector3 FindGenPosition()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        float rand = Random.Range(0f, 1f);
        float radius = (rand > 0.995f) 
            ? genRadius 
            : 0.4f * genRadius * Mathf.Sqrt(-Mathf.Log(1 - rand)) + 1.5f;

        float angle = 2f * Mathf.PI * Random.Range(0f, 1f);
        Vector3 pos = new Vector3(radius * Mathf.Cos(angle), 0f, radius * Mathf.Sin(angle)) + PlayerTransform.position;

        if (!PlaySystemRefStorage.mapSetter.IsInMap(pos))
        {
            pos = PlaySystemRefStorage.mapSetter.FindNearestPoint(pos);
        }

        return pos;
    }

    /// <summary>
    /// 랜덤 회전 각도를 포함한 몬스터 회전값을 설정합니다.
    /// </summary>
    protected override Quaternion SetGenRotation(Vector3 genPos)
    {
        Quaternion lookRot = Quaternion.LookRotation(PlayerTransform.position - genPos, Vector3.up);
        float randAngle = (Random.Range(0, 2) * 2 - 1) * Mathf.Pow(Random.Range(0f, 1f), 2f) * randDirAngle;

        if (randAngle < 0f) randAngle += 360f;

        return lookRot * Quaternion.Euler(0f, randAngle, 0f);
    }
}
