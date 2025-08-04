using System.Collections;
using UnityEngine;

public class Boss1State_Pattern_2 : BossState_Pattern
{
    private static readonly int PATTERN2 = Animator.StringToHash("Pattern2");
    private float safeZoneChargeTime = 2f;
    private float chargeTime = 5f;
    
    private float lapseTime;
    private float radius = 25f;
    
    private bool hasBomb;

    [SerializeField]private MonsterIndicatorCtrl safeZoneIndicatorCtrl;
    
    public override void TransitionAction(MonsterController controller)
    {
        base.TransitionAction(controller);
        Debug.Log("[Boss Pattern] ÆÐÅÏ 2");

        lapseTime = 0f;
        hasBomb = false;

        controller.indicatorCtrl.GetIndicatorTransform().localScale = 2f * radius * Vector3.one;
        controller.indicatorCtrl.InitIndicator();
        controller.indicatorCtrl.ActivateIndicator();
        
        safeZoneIndicatorCtrl.InitIndicator();
        safeZoneIndicatorCtrl.ActivateIndicator();
    }

    public override void UpdateAction()
    {
        base.UpdateAction();

        lapseTime += Time.deltaTime;
        float ratio = Mathf.Pow(lapseTime / chargeTime, 0.75f);
        float ratioSafe = Mathf.Pow(lapseTime / safeZoneChargeTime, 0.75f);
        
        controller.indicatorCtrl.ControlIndicator(ratio);
        safeZoneIndicatorCtrl.ControlIndicator(ratioSafe);

        if (lapseTime > chargeTime && !hasBomb)
        {
            StartCoroutine(Pattern());
        }
    }

    private IEnumerator Pattern()
    {
        hasBomb = true;

        Vector3 playerDist = controller.transform.position - controller.playerTransform.position;
        var safeZone = radius * 0.42;
        
        controller.enemyAnimator.SetTrigger(PATTERN2);
        
        yield return new WaitForSeconds(0.2f);

        if (safeZone < playerDist.magnitude && playerDist.magnitude < radius)
        {
            if (controller.playerTransform.TryGetComponent<PlayerStatManager>(out var player))
            {
                var monster = GetComponentInParent<IDamageAble>();
                var playerGo = GameObject.FindGameObjectWithTag("Player");
                    
                if (playerGo != null && playerGo.TryGetComponent<IDamageAble>(out var receiver))
                {
                    CombatEvent combatEvent = new CombatEvent
                    {
                        Sender = monster,
                        Receiver = receiver,
                        Damage = (monster as Monster).skillDamages[1],
                        HitPosition = controller.transform.position,
                        Collider = monster.MainCollider,
                    };

                    CombatSystem.Instance.AddInGameEvent(combatEvent);
                }
            }
        }

        controller.indicatorCtrl.DeactivateIndicator();
        safeZoneIndicatorCtrl.DeactivateIndicator();

        yield return new WaitForSeconds(1f);

        controller.EnemyPreaction();
    }
}