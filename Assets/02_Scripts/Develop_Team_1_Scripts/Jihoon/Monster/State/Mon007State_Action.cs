using System.Collections;
using UnityEngine;

public class Mon007State_Action : MonsterState_Action
{
    [SerializeField] private GameObject bombFX;
    private float bombRadius = 5f;
    private float chargeTime = 1.5f;
    private float lapseTime = 0f;
    private bool hasBomb;
    
    public override void TransitionAction(MonsterController controller)
    {
        base.TransitionAction(controller);
        
        Debug.Log("[자폭병] 프리액션 진입");
        controller.indicatorCtrl.GetIndicatorTransform().localScale = 2f * bombRadius * Vector3.one;
        controller.indicatorCtrl.ActivateIndicator();
    }
    public override void UpdateAction()
    {
        if (hasBomb)
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

        controller.DeactiveEnemy();
        controller.DestroyEnemy();
    }
}
