using System.Collections;
using UnityEngine;

public class Mon4Factory : MonsterFactory
{
    private float genRadius = 14f; //3 times of wanted peak radius
    private float randDirAngle = 30f;
    
    #region 재정의 메서드
    
    public override MonsterController SpawnMonster(int allowedPattern)
    {
        int cost = GetCostFromRate(Random.Range(0, 100), allowedPattern);

        if (!TryConsumeDifficulty(cost)) return null;

        int spawnCount = cost switch
        {
            3 => 5,
            _ => 1
        };
        var group = new PatternGroup { Remaining = spawnCount, Cost = cost };
        ActiveGroups.Add(group);

        if (cost == 2)
        {
            Debug.Log("[슬라임]: 패턴 1");
            var ctrl = DefaultSpawn(0);
            ctrl.InitEnemy(PlayerTransform);
            RegisterDestroyCallback(ctrl, group);
        }
        else if (cost == 3)
        {
            Debug.Log("[슬라임]: 패턴 2");
            StartCoroutine(StartPattern2(group));
        }
        else
        {
            Debug.Log("[슬라임]: 패턴 3");
            Vector3 pos = FindGenPosition();
            Quaternion quat = SetGenRotation(pos);
            var ctrl = Instantiate(monsterPattern3, pos, quat).GetComponent<MonsterController>();
            ctrl.InitEnemy(PlayerTransform);
            RegisterDestroyCallback(ctrl, group);
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
                    _ => 3
                };
            case 3:
                return rate switch
                {
                    < 33 => 2,
                    < 66 => 3,
                    _ => 5
                };
        }

        return 0;
    }
    
    protected override Vector3 FindGenPosition()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        float rand = Random.Range(0f, 1f);
        float radius = (rand > 0.995f) ? genRadius : 0.4f * genRadius * Mathf.Sqrt(-Mathf.Log(1 - rand)) + 1.5f;
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
        Quaternion quat = Quaternion.LookRotation(PlayerTransform.position - genPos, Vector3.up);
        float randAngle = (Random.Range(0, 2) * 2 - 1) * Mathf.Pow(Random.Range(0f, 1f), 2f);
        randAngle *= randDirAngle;
        if (randAngle < 0f)
        {
            randAngle += 360f;
        }

        quat *= Quaternion.Euler(0f, randAngle, 0f);
        return quat;
    }
    
    #endregion

    #region 내부 정의 메서드

    /// <summary>
    /// 패턴 2: 플레이어 주변에서 5마리의 몬스터가 순차적으로 생성됩니다.
    /// </summary>
    /// <param name="group">소환된 몬스터가 속할 패턴 그룹</param>
    /// <returns></returns>
    private IEnumerator StartPattern2(PatternGroup group)
    {
        float angleOffset = Random.Range(0f, 2f * Mathf.PI);
        
        for (int i = 0; i < 5; i++)
        {
            var angle = 1f / 6.5f * Mathf.PI * i + angleOffset;
            Vector3 center = PlayerTransform.position;
            if (center.magnitude > PlaySystemRefStorage.mapSetter.MaxRange - 6f)
            {
                center *= (PlaySystemRefStorage.mapSetter.MaxRange - 6f) / center.magnitude;
            }

            Vector3 pos = center + 5f * new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
            Quaternion quat = Quaternion.LookRotation(PlayerTransform.position - pos, Vector3.up);
            var ctrl = Instantiate(monsterPattern1, pos, quat).GetComponent<MonsterController>();
            ctrl.InitEnemy(PlayerTransform);
            RegisterDestroyCallback(ctrl, group);
            yield return new WaitForSeconds(0.33f);
        }
    }

    #endregion
    
}
