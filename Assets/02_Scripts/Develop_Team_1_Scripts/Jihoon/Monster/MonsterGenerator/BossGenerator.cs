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
            //todo ���� ���൵ ���� �б� �߰� �� ��
            
            SpawnBossMonster();
            isSpawned = true;
        }
        else
        {
            Debug.Log($"Stage number: {StageLoader.CurrentStageNumber}");
        }
    }
    
    //���� �ݿ��ؼ� ������ �� ���� ������ �ӽ÷� �� �� �ڿ� ������ �����ǰ� �ϴ� �޼���
    private void SpawnBossMonster()
    {
        var pos = findGenPosition();
        var quat = setGenRotation(pos);
        
        var boss = Instantiate(monsterPattern1, pos, quat);
        boss.GetComponent<MonsterController>().InitEnemy(playerTransform);
    }
}
