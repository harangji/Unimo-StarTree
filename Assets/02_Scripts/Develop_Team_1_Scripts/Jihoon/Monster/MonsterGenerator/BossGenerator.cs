using System.Collections;
using UnityEngine;

public class BossGenerator : MonsterGenerator
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    new void OnEnable()
    {
        base.OnEnable();
    }

    new void Update()
    {
        
    }
    
    private void Start()
    {
        StartCoroutine(SpawnBossMonster());
    }
    
    //���� �ݿ��ؼ� ������ �� ���� ������ �ӽ÷� �� �� �ڿ� ������ �����ǰ� �ϴ� �޼���
    private IEnumerator SpawnBossMonster()
    {
        yield return new WaitForSeconds(5f);

        var pos = findGenPosition();
        var quat = setGenRotation(pos);
        
        var boss = Instantiate(monsterPattern1, pos, quat);
        boss.GetComponent<MonsterController>().InitEnemy(playerTransform);
    }
}
