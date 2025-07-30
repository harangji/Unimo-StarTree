using System.Collections;
using UnityEngine;

public class BossGenerator : MonsterGenerator
{
    private bool isSpawned = false;
    
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
            
            SpawnBossMonster();
            isSpawned = true;
        }
        else
        {
            Debug.Log($"Stage number: {StageLoader.CurrentStageNumber}");
        }
    }
    
    //점수 반영해서 스폰할 수 없기 때문에 임시로 몇 초 뒤에 보스가 스폰되게 하는 메서드
    private void SpawnBossMonster()
    {
        var pos = findGenPosition();
        var quat = setGenRotation(pos);
        
        var boss = Instantiate(monsterPattern1, pos, quat);
        boss.GetComponent<MonsterController>().InitEnemy(playerTransform);
    }
}
