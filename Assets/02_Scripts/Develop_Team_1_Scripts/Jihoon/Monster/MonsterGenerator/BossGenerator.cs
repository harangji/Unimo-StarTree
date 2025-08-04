using System.Collections;
using UnityEngine;

public class BossGenerator : MonsterGenerator
{
    private bool isSpawned = false;
    private float genRadius = 20f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    new void OnEnable()
    {
        base.OnEnable();
    }
    
    private new void Update()
    {
        if (isSpawned) return;
        
        if(StageLoader.CurrentStageNumber % 10 == 0)
        {
            //todo 현재 진행도 조건 분기 추가 할 것
            
            StartCoroutine(SpawnBossMonster());
            isSpawned = true;
        }
        else
        {
            // Debug.Log($"Stage number: {StageLoader.CurrentStageNumber}");
        }
    }
    
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
    
    private IEnumerator SpawnBossMonster()
    {
        yield return new WaitForSeconds(3f);
        
        var pos = findGenPosition();
        var quat = setGenRotation(pos);
        
        var boss = Instantiate(monsterPattern1, pos, quat);
        boss.GetComponent<MonsterController>().InitEnemy(playerTransform);
    }
}
