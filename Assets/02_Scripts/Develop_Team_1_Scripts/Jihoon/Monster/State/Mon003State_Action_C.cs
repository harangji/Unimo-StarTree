using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mon003State_Action_C : MonsterState_Action
{
    [SerializeField] private List<Mon003mislActivator> projectiles;
    [SerializeField] private GameObject bombFX;
    private float bombDamage = 2.5f;
    private float chargeTime = 2f;
    private float lapseTime = 0f;
    private float bombRadius = 5f;
    private bool hasBomb;

    public bool canBomb = true;

    public override void TransitionAction(MonsterController controller)
    {
        base.TransitionAction(controller);
        if (canBomb)
        {
            OnTriggerAction();
        }
        controller.enemyAnimator.SetFloat("speed", 2f / chargeTime);
        controller.indicatorCtrl.GetIndicatorTransform().localScale = 2f * bombRadius * Vector3.one;
        controller.GetComponent<AudioSource>().enabled = true;
    }

    public override void UpdateAction()
    {
        if (hasBomb)
        {
            return;
        }

        if (!canBomb)
        {
            return;
        }

        lapseTime += Time.deltaTime;
        float ratio = Mathf.Pow(lapseTime / chargeTime, 0.75f);
        controller.indicatorCtrl.ControlIndicator(ratio);
        if (lapseTime > chargeTime)
        {
            bomb();
        }
    }

    private void bomb()
    {
        hasBomb = true;
        Instantiate(bombFX, transform.position + Vector3.up, Quaternion.identity);
        
        for (int i = 0; i < projectiles.Count; i++)
        {
            projectiles[i].transform.SetParent(null, true);
            projectiles[i].ActivateMissle(controller.playerTransform, controller.transform.position);
        }
        
        if(!EditorMode.Instance.isInvincible)
        {
            Vector3 playerdiff = controller.transform.position - controller.playerTransform.position;
            if (playerdiff.magnitude < bombRadius)
            {
                if (controller.playerTransform.TryGetComponent<PlayerStatManager>(out var player))
                {
                    var monster = GetComponentInParent<IDamageAble>();
                    var playerIDamageAble = GameObject.FindGameObjectWithTag("Player").GetComponent<IDamageAble>();

                    CombatEvent combatEvent = new CombatEvent
                    {
                        Sender = monster,
                        Receiver = playerIDamageAble,
                        Damage = (monster as Monster).skillDamages[0],
                        HitPosition = controller.transform.position,
                        Collider = monster.MainCollider,
                    };

                    CombatSystem.Instance.AddInGameEvent(combatEvent);
                }
            }
        }

        controller.DeactiveEnemy();
        controller.DestroyEnemy();
    }

    public void OnTriggerAction()
    {
        controller.enemyAnimator.SetTrigger("action");
    }
}