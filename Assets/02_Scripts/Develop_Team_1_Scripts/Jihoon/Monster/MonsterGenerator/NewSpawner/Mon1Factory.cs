using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Mon1Factory : MonsterFactory
{
    private float genRadius = 20f;
    private float randDirAngle = 40f;

    #region 재정의 메서드

    public override MonsterController SpawnMonster(int allowedPattern)
    {
        int cost = GetCostFromRate(Random.Range(0, 100), allowedPattern);
        
        if (!TryConsumeDifficulty(cost)) return null;

        int spawnCount = cost < 5 ? 1 : 8;
        var group = new PatternGroup { Remaining = spawnCount, Cost = cost };
        ActiveGroups.Add(group);

        var prefab = GetPrefabFromRate(cost);
        float offset = cost <= 6 ? 6.5f : 7.5f;

        return spawnCount == 1
            ? SpawnSingle(prefab, group, out var single) ? single : null
            : SpawnGroup(prefab, offset, group);
    }
    
    protected override int GetCostFromRate(int rate, int allowedPattern)
    {
        switch (allowedPattern)
        {
            case 1:
                return 3;
            case 2:
                return rate switch
                {
                    < 70 => 3,
                    _ => 5
                };
            case 3:
                return rate switch
                {
                    < 60 => 3,
                    < 85 => 5,
                    _ => 6
                };
        }
        
        return 0;
    }
    
    protected override Vector3 FindGenPosition()
    {
        Random.InitState(DateTime.Now.Millisecond);
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

    protected override Quaternion SetGenRotation(Vector3 genPos)
    {
        Quaternion lookRot = Quaternion.LookRotation(PlayerTransform.position - genPos, Vector3.up);
        float randAngle = (Random.Range(0, 2) * 2 - 1) * Mathf.Pow(Random.Range(0f, 1f), 2f) * randDirAngle;

        if (randAngle < 0f) randAngle += 360f;

        return lookRot * Quaternion.Euler(0f, randAngle, 0f);
    }

    #endregion

    #region 내부 정의 메서드
    
    /// <summary>
    /// rate 값을 기반으로 사용할 프리팹을 선택합니다.
    /// </summary>
    /// <param name="cost">랜덤으로 생성된 cost 값</param>
    /// <returns>선택된 몬스터 프리팹</returns>
    private GameObject GetPrefabFromRate(int cost)
    {
        return cost <= 5 ? monsterPattern1 : monsterPattern3;
    }

    /// <summary>
    /// 단일 몬스터를 소환하고 그룹에 등록합니다.
    /// </summary>
    /// <param name="prefab">소환할 프리팹</param>
    /// <param name="group">소환된 패턴 그룹 정보</param>
    /// <param name="ctrl">소환된 몬스터 컨트롤러 (out)</param>
    /// <returns>소환 성공 여부</returns>
    private bool SpawnSingle(GameObject prefab, PatternGroup group, out MonsterController ctrl)
    {
        ctrl = DefaultSpawn(0);
        if (ctrl == null) return false;

        ctrl.InitEnemy(PlayerTransform);
        RegisterDestroyCallback(ctrl, group);
        return true;
    }

    /// <summary>
    /// 8마리의 몬스터를 중심 위치를 기준으로 원형으로 소환합니다.
    /// </summary>
    /// <param name="prefab">소환할 몬스터 프리팹</param>
    /// <param name="offset">중심으로부터의 거리</param>
    /// <param name="group">소환된 패턴 그룹 정보</param>
    /// <returns>첫 번째로 생성된 몬스터 컨트롤러</returns>
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
    /// 맵 범위 밖으로 벗어날 경우 중심에서 maxRadius 이내로 위치를 제한합니다.
    /// </summary>
    /// <param name="position">플레이어 기준 위치</param>
    /// <param name="maxRadius">맵의 최대 반지름</param>
    /// <returns>제한된 위치</returns>
    private Vector3 ClampToArena(Vector3 position, float maxRadius)
    {
        return position.magnitude > maxRadius ? position.normalized * maxRadius : position;
    }

    #endregion
}
