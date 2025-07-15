using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mon003mislState_Action : MonsterState_Action
{
    [SerializeField] private GameObject bombFX;
    static readonly private Vector3 AddGravitySTATIC = new Vector3(0f, -10f, 0f);
    static private float masterBombRadius = 5f;
    private Rigidbody rigidBody;
    private Vector3 targetPos;
    private Vector3 centerPos;
    private float bombDamage = 1f;
    private float flightTime = 0.5f;
    private float flightDist = 4f;
    private float lapseTime = 0f;
    private float bombRadius;
    private bool hasBomb;

    public override void TransitionAction(MonsterController controller)
    {
        base.TransitionAction(controller);
        bombRadius = masterBombRadius / 3f;
        rigidBody = controller.GetComponent<Rigidbody>();
        setTargetPos();
        calculateParabola();
        rigidBody.useGravity = true;
        controller.indicatorCtrl.GetIndicatorTransform().SetPositionAndRotation(targetPos, Quaternion.identity);
        controller.indicatorCtrl.GetIndicatorTransform().SetParent(null, true);
        controller.indicatorCtrl.GetIndicatorTransform().localScale = 2 * bombRadius * Vector3.one;
    }

    public override void FixedUpdateAction()
    {
        rigidBody.AddForce(AddGravitySTATIC);
        controller.transform.forward = rigidBody.linearVelocity.normalized;
    }

    public override void UpdateAction()
    {
        if (hasBomb)
        {
            return;
        }

        lapseTime += Time.deltaTime;
        float ratio = Mathf.Pow(lapseTime / flightTime, 0.75f);
        controller.indicatorCtrl.ControlIndicator(ratio);
        if (lapseTime > flightTime)
        {
            bomb();
        }
    }

    public void SetCenterPos(Vector3 pos)
    {
        centerPos = pos;
    }

    private void setTargetPos()
    {
        Vector3 projForward = controller.transform.forward;
        projForward.y = 0f;
        projForward.Normalize();
        targetPos = centerPos + 0.8f * masterBombRadius * projForward;
        Vector3 diffVec = targetPos - controller.transform.position;
        diffVec.y = 0;
        flightDist = diffVec.magnitude;
    }

    private void calculateParabola()
    {
        float angle = Mathf.Acos(Vector3.Dot(controller.transform.forward, Vector3.up));
        float speed = flightDist * Mathf.Sqrt((Physics.gravity + AddGravitySTATIC).magnitude /
                                              (controller.transform.position.y * Mathf.Sin(angle) +
                                               flightDist * Mathf.Cos(angle)))
                      / Mathf.Sqrt(2 * Mathf.Sin(angle));
        rigidBody.linearVelocity = speed * controller.transform.forward;
        flightTime = flightDist / speed / Mathf.Sin(angle);
    }

    private void bomb()
    {
        hasBomb = true;
        Instantiate(bombFX, transform.position + 0.7f * Vector3.up, Quaternion.identity);
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
                    Damage = (monster as Monster).skillDamage,
                    HitPosition = controller.transform.position,
                    Collider = null,
                };

                CombatSystem.Instance.AddInGameEvent(combatEvent);
            }
        }

        controller.DeactiveEnemy();
        controller.DestroyEnemy();
    }
}