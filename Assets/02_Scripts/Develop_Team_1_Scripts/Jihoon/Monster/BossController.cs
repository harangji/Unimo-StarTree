using UnityEngine;
using UnityEngine.Serialization;

public class BossController : MonsterController
{
    [HideInInspector] public BossState_Pattern[] bossPatterns;
    
    public override void InitEnemy(Transform targetPlayer)
    {
        playerTransform = targetPlayer;
        
        enemyAnimator = GetComponent<Animator>();
        machine = GetComponent<MonsterMachine>();
        enemyCollider = GetComponent<Collider>();
        indicatorCtrl = GetComponent<MonsterIndicatorCtrl>();

        enemyAppear = GetComponentInChildren<MonsterState_Appear>();
        enemyPreact = GetComponentInChildren<MonsterState_Preaction>();
        enemyDiappear = GetComponentInChildren<MonsterState_Disappear>();
        enemyExplode = GetComponentInChildren<MonsterState_Explode>();

        bossPatterns = GetComponentsInChildren<BossState_Pattern>();
        
        MonGeneratorManager.AllMonsterListSTATIC.Add(this);
        EnemyAppear();
    }

    public void EnemyPattern(BossState_Pattern pattern)
    {
        machine.TransitState(pattern, this);
    }
}