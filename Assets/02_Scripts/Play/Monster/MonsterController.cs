using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum Patterns
{
    Pattern1,
    Pattern2,
    Pattern3
}

public class MonsterController : MonoBehaviour
{
    static private float MONmaxLifeTimeSTATIC = 12f;

    public Transform playerTransform { get; private set; }
    public Animator enemyAnimator { get; private set; }
    public MonsterIndicatorCtrl indicatorCtrl { get; private set; }
    public Collider enemyCollider { get; private set; }

    [SerializeField] private float collideStunTime = 0.8f;
    [SerializeField] private bool isExplode = true;
    [SerializeField] private GameObject explodeFX;
    [SerializeField] private bool isExplodeFXAtPlayer = true;

    private MonsterMachine machine;
    private MonsterState_Appear enemyAppear;
    private MonsterState_Preaction enemyPreact;
    private MonsterState_Action enemyAction;
    private MonsterState_Disappear enemyDiappear;
    private MonsterState_Explode enemyExplode;

    private float existTime = 0f;

    public Patterns pattern;

    private void Update()
    {
        existTime += Time.deltaTime;
        if (existTime > MONmaxLifeTimeSTATIC)
        {
            DestroyEnemy();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isExplode)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<PlayerStatManager>(out var player))
            {
                Vector3 hitPos = transform.position; //other.ClosestPoint(transform.position + new Vector3(0f, 1.5f, 0f))
                hitPos.y = 0f;

                //todo 이 부분에서 나중에 컴벳 시스템으로 바꿔야 함 -> 지금은 그냥 Hit 메서드를 변경함
                var monster = GetComponent<IDamageAble>();
                var playerIDamageAble = GameObject.FindGameObjectWithTag("Player").GetComponent<IDamageAble>();

                CombatEvent combatEvent = new CombatEvent
                {
                    Sender = monster,
                    Receiver = playerIDamageAble,
                    Damage = (monster as Monster).defaultDamage,
                    HitPosition = hitPos,
                    Collider = other
                };

                CombatSystem.Instance.AddInGameEvent(combatEvent);

                // player.Hit(collideStunTime, hitPos, GetComponent<Monster>().GetDamage());

                Vector3 fxPos = (isExplodeFXAtPlayer)
                    ? (hitPos + other.transform.position) / 2f + 1.5f * Vector3.up
                    : hitPos + 1.5f * Vector3.up;
                GameObject obj = Instantiate(explodeFX, fxPos, Quaternion.identity);

                obj.GetComponent<AudioSource>().volume = Sound_Manager.instance._audioSources[1].volume;
                obj.transform.localScale *= transform.localScale.x;

                EnemyExplode();
            }
        }
    }

    public void InitEnemy(Transform targetPlayer)
    {
        playerTransform = targetPlayer;
        enemyAnimator = GetComponent<Animator>();
        machine = GetComponent<MonsterMachine>();
        enemyCollider = GetComponent<Collider>();
        indicatorCtrl = GetComponent<MonsterIndicatorCtrl>();

        enemyAppear = GetComponentInChildren<MonsterState_Appear>();
        enemyPreact = GetComponentInChildren<MonsterState_Preaction>();
        enemyAction = GetComponentInChildren<MonsterState_Action>();
        enemyDiappear = GetComponentInChildren<MonsterState_Disappear>();
        enemyExplode = GetComponentInChildren<MonsterState_Explode>();

        MonGeneratorManager.AllMonsterListSTATIC.Add(this);
        EnemyAppear();
    }

    public void ActivateEnemy()
    {
        if (enemyCollider != null)
        {
            enemyCollider.enabled = true;
        }

        if (indicatorCtrl != null) indicatorCtrl.ActivateIndicator();
    }

    public void DeactiveEnemy()
    {
        if (enemyCollider != null)
        {
            enemyCollider.enabled = false;
        }

        if (indicatorCtrl != null) indicatorCtrl.DeactivateIndicator();
    }

    public void DestroyEnemy()
    {
        MonGeneratorManager.AllMonsterListSTATIC.Remove(this);
        Destroy(gameObject);
    }

    public void EnemyAppear()
    {
        machine.TransitState(enemyAppear, this);
    }

    public void EnemyPreaction()
    {
        machine.TransitState(enemyPreact, this);
    }

    public void EnemyAction()
    {
        machine.TransitState(enemyAction, this);
    }

    public void EnemyDisappear()
    {
        machine.TransitState(enemyDiappear, this);
    }

    public void EnemyExplode()
    {
        machine.TransitState(enemyExplode, this);
    }
}