using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class BossController : MonsterController
{
    [HideInInspector] public BossState_Pattern[] bossPatterns;

    public bool canAction = true;
    public bool isPattern3 = false;

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

        // MonGeneratorManager.AllMonsterListSTATIC.Add(this);
        EnemyAppear();
    }

    public void EnemyPattern(BossState_Pattern pattern)
    {
        machine.TransitState(pattern, this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<PlayerStatManager>(out var player))
            {
                Vector3 hitPos = transform.position; 
                hitPos.y = 0f;

                var monster = GetComponent<IDamageAble>();

                CombatEvent combatEvent = new CombatEvent
                {
                    Sender = monster,
                    Receiver = player,
                    Damage = ((Monster)monster).appliedDamage,
                    HitPosition = hitPos,
                    Collider = other,
                    IsStrongKnockback = false,
                };

                if (isPattern3)
                {
                    combatEvent.Damage = ((Monster)monster).skillDamages[2];
                    combatEvent.IsStrongKnockback = true;
                }

                CombatSystem.Instance.AddInGameEvent(combatEvent);

                Vector3 fxPos = (isExplodeFXAtPlayer) ? (hitPos + other.transform.position) / 2f + 1.5f * Vector3.up : hitPos + 1.5f * Vector3.up;
                GameObject obj = Instantiate(explodeFX, fxPos, Quaternion.identity);

                obj.GetComponent<AudioSource>().volume = Sound_Manager.instance._audioSources[1].volume;
                obj.transform.localScale *= transform.localScale.x;
            }

            StartCoroutine(StartStun());
        }
    }

    private IEnumerator StartStun()
    {
        canAction = false;

        yield return new WaitForSeconds(1f);

        canAction = true;
    }
}