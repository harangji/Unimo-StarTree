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
    
    //점수 반영해서 스폰할 수 없기 때문에 임시로 몇 초 뒤에 보스가 스폰되게 하는 메서드
    private IEnumerator SpawnBossMonster()
    {
        yield return new WaitForSeconds(10f);

        var pos = findGenPosition();
        var quat = setGenRotation(pos);
        
        Instantiate(monsterPattern1, pos, quat);
    }
}
