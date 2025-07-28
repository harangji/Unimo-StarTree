using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Mon004Generator_C : MonsterGenerator
{
    private class PatternGroup { public int Remaining; public int Cost; }
    private List<PatternGroup> _activeGroups = new List<PatternGroup>();
    
    private float genRadius = 14f; //3 times of wanted peak radius
    private float randDirAngle = 30f;

    new void OnEnable() { base.OnEnable(); }
    new void Update()   { base.Update();   }

    protected override MonsterController generateEnemy()
    {
        int cost;
        // var rate = Random.Range(0, 100);
        // var rate = Random.Range(0, 70);
        var rate = Random.Range(90,100);
        if (rate < 70) {cost = 1;}
        else if (rate < 90) cost = 3;
        else cost = 5;
        
        var stageMgr = PlaySystemRefStorage.stageManager;
        if (stageMgr == null || !stageMgr.TryConsumeDifficulty(cost))
        {
            Debug.Log("할당할 난이도 수치가 부족합니다.");
            return null;
        }

        int spawnCount;
        if (rate < 70) {spawnCount = 1;}
        else if (rate < 90) spawnCount = 5;
        else spawnCount = 1;
        
        var group = new PatternGroup { Remaining = spawnCount, Cost = cost };
        _activeGroups.Add(group);
        
        if (rate < 70)
        {
            Debug.Log("패턴 1");
            var ctrl = base.generateEnemy();
            ctrl.InitEnemy(playerTransform);
            RegisterDestroyCallback(ctrl, group);
        }
        else if (rate < 90)
        {
            Debug.Log("패턴 2");
            StartCoroutine(StartPattern2(group));
        }
        else
        {
            Debug.Log("패턴 3");
            Vector3 pos = findGenPosition();
            Quaternion quat = setGenRotation(pos);
            var ctrl = Instantiate(monsterPattern3, pos, quat).GetComponent<MonsterController>();
            ctrl.InitEnemy(playerTransform);
            RegisterDestroyCallback(ctrl, group);
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
        float radius = (rand > 0.995f) ? genRadius : 0.4f * genRadius * Mathf.Sqrt(-Mathf.Log(1 - rand)) + 1.5f;
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
        Quaternion quat = Quaternion.LookRotation(playerTransform.position - genPos, Vector3.up);
        float randAngle = (Random.Range(0, 2) * 2 - 1) * Mathf.Pow(Random.Range(0f, 1f), 2f);
        randAngle *= randDirAngle;
        if (randAngle < 0f)
        {
            randAngle += 360f;
        }

        quat *= Quaternion.Euler(0f, randAngle, 0f);
        return quat;
    }

    private IEnumerator StartPattern2(PatternGroup group)
    {
        float angleOffset = Random.Range(0f, 2f * Mathf.PI);
        for (int i = 0; i < 5; i++)
        {
            var angle = 1f / 6.5f * Mathf.PI * i + angleOffset;
            Vector3 center = playerTransform.position;
            if (center.magnitude > PlaySystemRefStorage.mapSetter.MaxRange - 6f)
            {
                center *= (PlaySystemRefStorage.mapSetter.MaxRange - 6f) / center.magnitude;
            }

            Vector3 pos = center + 5f * new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
            Quaternion quat = Quaternion.LookRotation(playerTransform.position - pos, Vector3.up);
            var ctrl = Instantiate(monsterPattern1, pos, quat).GetComponent<MonsterController>();
            ctrl.InitEnemy(playerTransform);
            RegisterDestroyCallback(ctrl, group);
            yield return new WaitForSeconds(0.33f);
        }
    }
}