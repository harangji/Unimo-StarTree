using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mon003Generator_C : MonsterGenerator
{
    private class PatternGroup { public int Remaining; public int Cost; }
    private List<PatternGroup> _activeGroups = new List<PatternGroup>();
    
    private float innerRadius = 2.5f;
    private float outerRadius = 8f;

    new void OnEnable() { base.OnEnable(); }
    new void Update()   { base.Update();   }

    protected override MonsterController generateEnemy()
    {
        // Vector3 pos = findGenPosition();
        // Quaternion quat = setGenRotation(pos);
        // MonsterController controller = Instantiate(monsterPattern1, pos, quat).GetComponent<MonsterController>();
        // controller.InitEnemy(playerTransform);
        //
        // return controller;

        int cost;
        var rate = Random.Range(0, 100);
        if (rate < 70) cost = 2;
        else if (rate < 90) cost = 4;
        else cost = 7;
        
        // 난이도 소비
        var stageMgr = PlaySystemRefStorage.stageManager;
        if (stageMgr == null || !stageMgr.TryConsumeDifficulty(cost))
        {
            Debug.Log("할당할 난이도 수치가 부족합니다.");
            return null;
        }
        
        // 그룹 생성
        int spawnCount = rate < 70 ? 1 : (rate < 90 ? 5 : 8);
        var group = new PatternGroup { Remaining = spawnCount, Cost = cost };
        _activeGroups.Add(group);

        if (rate < 70)
        {
            Debug.Log("패턴 1");
            var ctrl = base.generateEnemy();
            ctrl.InitEnemy(playerTransform);
            RegisterDestroyCallback(ctrl, group);
            return ctrl;
        }
        else if (rate < 90)
        {
            Debug.Log("패턴 2");
            StartPattern2(group);
        }
        else
        {
            Debug.Log("패턴 3");
            StartPattern3(group);
        }

        return null;
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

    protected override Vector3 findGenPosition()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        float rand = Random.Range(0f, 1f);
        rand *= rand;
        float radius = innerRadius + rand * (outerRadius - innerRadius);
        float angle = 2f * Mathf.PI * Random.Range(0f, 1f);

        Vector3 pos = new Vector3(radius * Mathf.Cos(angle), 0f, radius * Mathf.Sin(angle));
        pos += playerTransform.position;
        if (PlaySystemRefStorage.mapSetter.IsInMap(pos) == false)
        {
            pos = PlaySystemRefStorage.mapSetter.FindNearestPoint(pos);
        }

        return pos;
    }

    protected override Quaternion setGenRotation(Vector3 genPos)
    {
        float rand = Random.Range(-20f, 20f);
        Quaternion quat = Quaternion.Euler(0f, 180f + rand, 0f);
        return quat;
    }

    private void StartPattern2(PatternGroup group)
    {
        float angle;
        Vector3 center = playerTransform.position;
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
            controller.InitEnemy(playerTransform);
            RegisterDestroyCallback(controller, group);
        }
    }

    private void StartPattern3(PatternGroup group)
    {
        int firstBombCount = 0;
        Vector3 center = playerTransform.position;
        Vector3[] spawnOffsets = new Vector3[]
        {
            new Vector3(-2f, 0, 4f), // 앞왼
            new Vector3(4f, 0, 2f), // 오앞
            new Vector3(2f, 0, -4f), // 뒤오
            new Vector3(-4f, 0, -2f), // 왼뒤

            new Vector3(2f, 0, 4f), // 앞오
            new Vector3(4f, 0, -2f), // 오뒤
            new Vector3(-2f, 0, -4f), // 뒤왼
            new Vector3(-4f, 0, 2f) // 왼앞
        };
        var actions = new List<Mon003State_Action_C>();
        
        foreach (var spawnOffset in spawnOffsets)
        {
            var obj = Instantiate(monsterPattern1, center + spawnOffset, Quaternion.identity);
            MonsterController controller = obj.GetComponent<MonsterController>();
            controller.InitEnemy(playerTransform);
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

    private IEnumerator DelayBomb(List<Mon003State_Action_C> actions)
    {
        yield return new WaitForSeconds(2f);

        foreach (var action in actions)
        {
            action.canBomb = true;
            action.OnTriggerAction();
        }
    }
}