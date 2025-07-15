using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mon005Generator : MonsterGenerator
{
    private class PatternGroup { public int Remaining; public int Cost; }
    private List<PatternGroup> _activeGroups = new List<PatternGroup>();
    
    private float genRadius = 20f; //3 times of wanted peak radius
    private float randDirAngle = 40f;

    new void OnEnable() { base.OnEnable(); }
    new void Update()   { base.Update();   }

    protected override Vector3 findGenPosition()
    {
        //시드 생성
        Random.InitState(System.DateTime.Now.Millisecond);

        float rand = Random.Range(0f, 1f);
        float radius = (rand > 0.995f) ? genRadius : 0.4f * genRadius * Mathf.Sqrt(-Mathf.Log(1 - rand)) + 1.5f;
        float angle = 2f * Mathf.PI * Random.Range(0f, 1f);

        Vector3 pos = new Vector3(radius * Mathf.Cos(angle), 0, radius * Mathf.Sin(angle));
        pos += playerTransform.position;
        if (PlaySystemRefStorage.mapSetter.IsInMap(pos) == false)
        {
            pos = PlaySystemRefStorage.mapSetter.FindNearestPoint(pos);
        }

        pos.y = 1f;
        return pos;
    }

    protected override MonsterController generateEnemy()
    {
        //todo 난이도 점유 기능 추가해야 함 -> 나중에 게임 매니저 만들어지면 하면 됨
        
        int cost;
        var rate = Random.Range(0, 100);
        if (rate < 70) cost = 1;
        else if (rate < 90) cost = 3;
        else cost = 5;
        
        var stageMgr = PlaySystemRefStorage.stageManager;
        if (stageMgr == null || !stageMgr.TryConsumeDifficulty(cost))
        {
            Debug.Log("할당할 난이도 수치가 부족합니다.");
            return null;
        }
        
        PatternGroup group;
        MonsterController primary = null;

        Vector3 pos = findGenPosition();
        Quaternion quat = setGenRotation(pos);

        if (rate < 70)
        {
            group = new PatternGroup { Remaining = 1, Cost = cost };
            _activeGroups.Add(group);
            
            var ctrl = Instantiate(monsterPattern1, pos, quat).GetComponent<MonsterController>();
            ctrl.InitEnemy(playerTransform);
            RegisterDestroyCallback(ctrl, group);

            ctrl.pattern = Patterns.Pattern1;
            primary = ctrl;
        }
        else if (rate < 90)
        {
            Debug.Log("패턴 2");
            
            var pattern = Instantiate(monsterPattern2, pos, quat);
            var controllers = pattern.GetComponentsInChildren<MonsterController>();

            group = new PatternGroup { Remaining = controllers.Length, Cost = cost };
            _activeGroups.Add(group);
            
            foreach (var ctrl in controllers)
            {
                ctrl.pattern = Patterns.Pattern2;
                ctrl.InitEnemy(playerTransform);
                RegisterDestroyCallback(ctrl, group);
            }
        }
        else
        {
            Debug.Log("패턴 3");
            
            var pattern = Instantiate(monsterPattern3, pos, quat);
            var controllers = pattern.GetComponentsInChildren<MonsterController>();
            
            group = new PatternGroup { Remaining = controllers.Length, Cost = cost };
            _activeGroups.Add(group);

            foreach (var ctrl in controllers)
            {
                ctrl.pattern = Patterns.Pattern3;
                ctrl.InitEnemy(playerTransform);
                RegisterDestroyCallback(ctrl, group);
            }
        }

        return primary;
    }
    
    private void RegisterDestroyCallback(MonsterController ctrl, PatternGroup group)
    {
        ctrl.OnDestroyed += (monster) =>
        {
            group.Remaining--;
            if (group.Remaining == 0)
            {
                PlaySystemRefStorage.stageManager.RestoreDifficulty(group.Cost);
                _activeGroups.Remove(group);
            }
        };
    }
}