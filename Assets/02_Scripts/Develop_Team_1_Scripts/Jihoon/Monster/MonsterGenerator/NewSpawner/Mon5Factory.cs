using UnityEngine;

public class Mon5Factory : MonsterFactory
{
    private float genRadius = 20f; //3 times of wanted peak radius
    private float randDirAngle = 40f;

    #region 재정의 메서드

    public override MonsterController SpawnMonster(int allowedPattern)
    {
        int cost = GetCostFromRate(Random.Range(0, 100), allowedPattern);

        if (!TryConsumeDifficulty(cost)) return null;

        int spawnCount = cost switch
        {
            2 => 1,
            _ => 7
        };
        var group = new PatternGroup { Remaining = spawnCount, Cost = cost };
        ActiveGroups.Add(group);
        
        if (cost == 1)
        {
            group = new PatternGroup { Remaining = 1, Cost = cost };
            ActiveGroups.Add(group);

            var ctrl = DefaultSpawn(0);
            ctrl.InitEnemy(PlayerTransform);
            RegisterDestroyCallback(ctrl, group);

            ctrl.pattern = Patterns.Pattern1;
        }
        else if (cost == 3)
        {
            Vector3 pos = FindGenPosition();
            Quaternion quat = SetGenRotation(pos);
            
            var pattern = Instantiate(monsterPattern2, pos, quat);
            var controllers = pattern.GetComponentsInChildren<MonsterController>();

            group = new PatternGroup { Remaining = controllers.Length, Cost = cost };
            ActiveGroups.Add(group);
            
            foreach (var ctrl in controllers)
            {
                ctrl.pattern = Patterns.Pattern2;
                ctrl.InitEnemy(PlayerTransform);
                RegisterDestroyCallback(ctrl, group);
            }
        }
        else
        {
            Vector3 pos = FindGenPosition();
            Quaternion quat = SetGenRotation(pos);
            
            var pattern = Instantiate(monsterPattern3, pos, quat);
            var controllers = pattern.GetComponentsInChildren<MonsterController>();
            
            group = new PatternGroup { Remaining = controllers.Length, Cost = cost };
            ActiveGroups.Add(group);

            foreach (var ctrl in controllers)
            {
                ctrl.pattern = Patterns.Pattern3;
                ctrl.InitEnemy(PlayerTransform);
                RegisterDestroyCallback(ctrl, group);
            }
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
        //시드 생성
        Random.InitState(System.DateTime.Now.Millisecond);

        float rand = Random.Range(0f, 1f);
        float radius = (rand > 0.995f) ? genRadius : 0.4f * genRadius * Mathf.Sqrt(-Mathf.Log(1 - rand)) + 1.5f;
        float angle = 2f * Mathf.PI * Random.Range(0f, 1f);

        Vector3 pos = new Vector3(radius * Mathf.Cos(angle), 0, radius * Mathf.Sin(angle));
        pos += PlayerTransform.position;
        if (PlaySystemRefStorage.mapSetter.IsInMap(pos) == false)
        {
            pos = PlaySystemRefStorage.mapSetter.FindNearestPoint(pos);
        }

        pos.y = 1f;
        return pos;
    }

    #endregion
}