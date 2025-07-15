using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Mon001Generator_C : MonsterGenerator
{
    private class PatternGroup { public int Remaining; public int Cost; }
    private List<PatternGroup> _activeGroups = new List<PatternGroup>();
    
    private float genRadius = 20f; //3 times of wanted peak radius
    private float randDirAngle = 40f;

    // Start is called before the first frame update
    new void OnEnable()
    {
        base.OnEnable();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
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

    protected override MonsterController generateEnemy()
    {
        //todo 난이도 점유 기능 추가해야 함 -> 나중에 게임 매니저 만들어지면 하면 됨

        var rate = Random.Range(0, 100);
        int cost;
        
        // 패턴 별 코스트 비용
        if (rate < 70) cost = 2;
        else if (rate < 90) cost = 4;
        else cost = 5;
        
        // 정해진 코스트를 소모할 자원이 남아있는지 StageManager의 함수를 통해서 확인
        var stageMgr = PlaySystemRefStorage.stageManager;
        if (stageMgr == null || !stageMgr.TryConsumeDifficulty(cost))
        {
            Debug.Log("할당할 난이도 수치가 부족합니다.");
            return null; // 남은 난이도 부족 → 소환 취소
        }

        int spawnCount = rate < 70 ? 1 : 8;
        var group = new PatternGroup { Remaining = spawnCount, Cost = cost };
        _activeGroups.Add(group);

        MonsterController primary = null;
        var prefab = rate < 70 ? monsterPattern1 : (rate < 90 ? monsterPattern1 : monsterPattern3);
        float offset = rate < 90 ? 6.5f : 7.5f;

        if (spawnCount == 1)
        {
            var ctrl = base.generateEnemy(); 
            ctrl.InitEnemy(playerTransform);
            RegisterDestroyCallback(ctrl, group); 
            primary = ctrl;
        }
        else
        {
            Vector3 center = playerTransform.position;
            float max = PlaySystemRefStorage.mapSetter.MaxRange - 7.2f;
            if (center.magnitude > max) center = center.normalized * max;

            for (int i = 0; i < 8; i++)
            {
                float angle = Mathf.PI / 4f * i;
                Vector3 pos = center + offset * new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
                Quaternion rot = Quaternion.LookRotation(center - pos, Vector3.up);
                var ctrl = Instantiate(prefab, pos, rot).GetComponent<MonsterController>();
                ctrl.InitEnemy(playerTransform); 
                RegisterDestroyCallback(ctrl, group);
                if (primary == null) primary = ctrl;
            }
        }

        return primary;
        // else if (rate < 90)
        // {
        //     Debug.Log("패턴 2");
        //
        //     float angle;
        //     Vector3 center = playerTransform.position;
        //     
        //     if (center.magnitude > PlaySystemRefStorage.mapSetter.MaxRange - 7.2f)
        //     {
        //         center *= (PlaySystemRefStorage.mapSetter.MaxRange - 7.2f) / center.magnitude;
        //     }
        //
        //     for (int i = 0; i < 8; i++)
        //     {
        //         angle = 1f / 4f * Mathf.PI * i;
        //         Vector3 pos = center + 6.5f * new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
        //         Quaternion quat = Quaternion.LookRotation(center - pos, Vector3.up);
        //         controller = Instantiate(monsterPattern1, pos, quat).GetComponent<MonsterController>();
        //         controller.InitEnemy(playerTransform);
        //         Debug.Log("패턴 2 : 몇 번 실행되나요");
        //         controller.SetDifficultyCost(cost);
        //     }
        // }
        // else
        // {
        //     Debug.Log("패턴 3");
        //
        //     float angle;
        //     Vector3 center = playerTransform.position;
        //     
        //     if (center.magnitude > PlaySystemRefStorage.mapSetter.MaxRange - 7.2f)
        //     {
        //         center *= (PlaySystemRefStorage.mapSetter.MaxRange - 7.2f) / center.magnitude;
        //     }
        //
        //     for (int i = 0; i < 8; i++)
        //     {
        //         angle = 1f / 4f * Mathf.PI * i;
        //         Vector3 pos = center + 7.5f * new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
        //         Quaternion quat = Quaternion.LookRotation(center - pos, Vector3.up);
        //         controller = Instantiate(monsterPattern3, pos, quat).GetComponent<MonsterController>();
        //         controller.InitEnemy(playerTransform);
        //         Debug.Log("패턴 3 : 몇 번 실행되나요");
        //         controller.SetDifficultyCost(cost);
        //     }
        // }
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

    // protected override IEnumerator exPatternCoroutine()
    // {
    //     isExtreme = true;
    //     yield return new WaitForSeconds(2f);
    //     float angle;
    //     Vector3 center = playerTransform.position;
    //     if (center.magnitude > PlaySystemRefStorage.mapSetter.MaxRange - 7.2f)
    //     {
    //         center *= (PlaySystemRefStorage.mapSetter.MaxRange - 7.2f) / center.magnitude;
    //     }
    //
    //     for (int i = 0; i < 8; i++)
    //     {
    //         angle = 1f / 4f * Mathf.PI * i;
    //         Vector3 pos = center + 6.5f * new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
    //         Quaternion quat = Quaternion.LookRotation(center - pos, Vector3.up);
    //         MonsterController controller = Instantiate(monsterPattern1, pos, quat).GetComponent<MonsterController>();
    //         controller.InitEnemy(playerTransform);
    //     }
    //
    //     yield return new WaitForSeconds(2f);
    //     isPaused = false;
    //     isExtreme = false;
    //     yield break;
    // }
}