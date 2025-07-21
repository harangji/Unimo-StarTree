using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mon004State_Action : MonsterState_Action
{
    [SerializeField] private GameObject slamVFX;
    [SerializeField] private AudioClip[] jumpSFX;
    private AudioSource audioSource;
    private int remainJump = 3;
    private float jumpDuration = 10f;
    private float maxRotation = Mathf.PI * 7f / 18f;
    private float moveSpeed = 4f;
    private float attRange = 1.8f;
    private float attDamage = 2f;
    private Vector3 indicatorPos = Vector3.zero;
    private float lapseForIndicator = 0f;

    public override void TransitionAction(MonsterController controller)
    {
        base.TransitionAction(controller);
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(jumpCoroutine());
    }

    public override void UpdateAction()
    {
        base.UpdateAction();
        controller.transform.position += moveSpeed * Time.deltaTime * controller.transform.forward;
        controller.indicatorCtrl.GetIndicatorTransform().position = indicatorPos;
        lapseForIndicator += Time.deltaTime;
        controller.indicatorCtrl.ControlIndicator(lapseForIndicator / jumpDuration);
    }

    private IEnumerator jumpCoroutine()
    {
        yield return null;
        
        Sound_Manager.instance.PlayClip(jumpSFX[remainJump - 1]);
        
        seePlayer();
        checkJumpDuration();
        
        controller.indicatorCtrl.ActivateIndicator();
        controller.indicatorCtrl.GetIndicatorTransform().localScale = 2f * attRange / controller.transform.localScale.x * Vector3.one;
        
        while (remainJump > 0)
        {
            yield return new WaitForSeconds(jumpDuration);
            hitGround();
            seePlayer();
            yield return null;
            checkJumpDuration();
        }

        controller.EnemyExplode();
    }

    private void hitGround()
    {
        --remainJump;
        Vector3 playerdiff = controller.transform.position - controller.playerTransform.position;

        if (playerdiff.magnitude < attRange)
        {
            if (controller.playerTransform.TryGetComponent<PlayerStatManager>(out var player))
            {
                var monster = GetComponentInParent<IDamageAble>();
                var playerIDamageAble = GameObject.FindGameObjectWithTag("Player").GetComponent<IDamageAble>();

                CombatEvent combatEvent = new CombatEvent
                {
                    Sender = monster,
                    Receiver = playerIDamageAble,
                    Damage = (monster as Monster).skillDamage1,
                    HitPosition = controller.transform.position,
                    Collider = monster.MainCollider,
                };

                CombatSystem.Instance.AddInGameEvent(combatEvent);
            }
        }

        slamVFX.transform.localScale = attRange / 1.8f * Vector3.one;
        slamVFX.SetActive(true);

        attRange *= 0.8f;
        attDamage *= 0.65f;
        
        if (remainJump > 0)
        {
            Sound_Manager.instance.PlayClip(jumpSFX[remainJump - 1]);
        }
        else
        {
            slamVFX.transform.SetParent(null, true);
            var mainptc = slamVFX.GetComponent<ParticleSystem>().main;
            mainptc.stopAction = ParticleSystemStopAction.Destroy;
        }
    }

    private void seePlayer()
    {
        Vector3 toPlyaerVec = controller.playerTransform.position - controller.transform.position;
        float btwangle = Mathf.Acos(
            Mathf.Clamp(controller.transform.forward.x * toPlyaerVec.x +
                        controller.transform.forward.z * toPlyaerVec.z,
                -0.999f, 0.999f));

        if (btwangle <= maxRotation)
        {
            controller.transform.forward = toPlyaerVec.normalized;
        }
        else
        {
            float CCrot = (controller.transform.forward.IsCCRot(toPlyaerVec)) ? 1f : -1f;
            Quaternion quat = Quaternion.Euler(0f, CCrot * maxRotation * 180f / Mathf.PI, 0f);
            controller.transform.rotation *= quat;
        }
    }

    private void checkJumpDuration()
    {
        float animTime = controller.enemyAnimator.GetCurrentAnimatorStateInfo(0).length;
        animTime *= 1f - controller.enemyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        jumpDuration = animTime;
        lapseForIndicator = 0f;
        indicatorPos = controller.transform.position + moveSpeed * jumpDuration * transform.forward;
        controller.indicatorCtrl.GetIndicatorTransform().localScale =
            2f * attRange / controller.transform.localScale.x * Vector3.one;
    }
}