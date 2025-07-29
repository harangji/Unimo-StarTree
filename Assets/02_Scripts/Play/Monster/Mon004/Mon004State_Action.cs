#region 원본

// using System.Collections;
// using UnityEngine;
//
// public class Mon004State_Action : MonsterState_Action
// {
//     [SerializeField] private GameObject slamVFX;
//     [SerializeField] private AudioClip[] jumpSFX;
//     [SerializeField] private float attRange = 1.8f;
//
//     private AudioSource audioSource;
//     private int remainJump = 3;
//     private float jumpDuration = 10f;
//     private float maxRotation = Mathf.PI * 7f / 18f;
//     private float moveSpeed = 4f;
//     private Vector3 indicatorPos = Vector3.zero;
//     private float lapseForIndicator = 0f;
//
//     /// <summary>
//     /// 상태 전환 시 초기화 및 점프 코루틴 시작
//     /// </summary>
//     public override void TransitionAction(MonsterController controller)
//     {
//         base.TransitionAction(controller);
//         audioSource = GetComponent<AudioSource>();
//         StartCoroutine(JumpCoroutine());
//     }
//
//     /// <summary>
//     /// 매 프레임 호출. 몬스터 이동 및 인디케이터 갱신
//     /// </summary>
//     public override void UpdateAction()
//     {
//         base.UpdateAction();
//         controller.transform.position += moveSpeed * Time.deltaTime * controller.transform.forward;
//         controller.indicatorCtrl.GetIndicatorTransform().position = indicatorPos;
//         lapseForIndicator += Time.deltaTime;
//         controller.indicatorCtrl.ControlIndicator(lapseForIndicator / jumpDuration);
//     }
//
//     /// <summary>
//     /// 점프 패턴 반복 수행 코루틴
//     /// </summary>
//     private IEnumerator JumpCoroutine()
//     {
//         yield return null;
//
//         Sound_Manager.instance.PlayClip(jumpSFX[remainJump - 1]);
//
//         RotateTowardPlayer();
//         UpdateJumpDuration();
//
//         controller.indicatorCtrl.ActivateIndicator();
//         controller.indicatorCtrl.GetIndicatorTransform().localScale = 2f * attRange / controller.transform.localScale.x * Vector3.one;
//
//         while (remainJump > 0)
//         {
//             yield return new WaitForSeconds(jumpDuration);
//             ExecuteSlam();
//             RotateTowardPlayer();
//             yield return null;
//             UpdateJumpDuration();
//         }
//
//         controller.EnemyExplode();
//     }
//
//     /// <summary>
//     /// 착지 시 공격 판정 및 이펙트 처리
//     /// </summary>
//     private void ExecuteSlam()
//     {
//         Vector3 playerDiff = controller.transform.position - controller.playerTransform.position;
//
//         if (playerDiff.magnitude < attRange)
//         {
//             if (controller.playerTransform.TryGetComponent<PlayerStatManager>(out var player))
//             {
//                 var monster = GetComponentInParent<IDamageAble>();
//                 var playerIDamageAble = GameObject.FindGameObjectWithTag("Player").GetComponent<IDamageAble>();
//
//                 CombatEvent combatEvent = new CombatEvent
//                 {
//                     Sender = monster,
//                     Receiver = playerIDamageAble,
//                     Damage = (monster as Monster).skillDamages[3 - remainJump],
//                     HitPosition = controller.transform.position,
//                     Collider = monster.MainCollider,
//                 };
//
//                 CombatSystem.Instance.AddInGameEvent(combatEvent);
//             }
//         }
//
//         slamVFX.transform.localScale = attRange / 1.8f * Vector3.one;
//         slamVFX.SetActive(true);
//
//         attRange *= 0.8f;
//
//         if (remainJump > 0)
//         {
//             Sound_Manager.instance.PlayClip(jumpSFX[remainJump - 1]);
//         }
//         else
//         {
//             slamVFX.transform.SetParent(null, true);
//             var mainPtc = slamVFX.GetComponent<ParticleSystem>().main;
//             mainPtc.stopAction = ParticleSystemStopAction.Destroy;
//         }
//
//         remainJump--;
//     }
//
//     /// <summary>
//     /// 플레이어 방향으로 몬스터 회전 (최대 회전각 제한)
//     /// </summary>
//     private void RotateTowardPlayer()
//     {
//         Vector3 toPlayerVec = controller.playerTransform.position - controller.transform.position;
//
//         float angle = Mathf.Acos(
//             Mathf.Clamp(Vector3.Dot(controller.transform.forward, toPlayerVec.normalized), -0.999f, 0.999f));
//
//         if (angle <= maxRotation)
//         {
//             controller.transform.forward = toPlayerVec.normalized;
//         }
//         else
//         {
//             float ccw = controller.transform.forward.IsCCRot(toPlayerVec) ? 1f : -1f;
//             Quaternion rot = Quaternion.Euler(0f, ccw * maxRotation * Mathf.Rad2Deg, 0f);
//             controller.transform.rotation *= rot;
//         }
//     }
//
//     /// <summary>
//     /// 점프 시간 및 인디케이터 위치 갱신
//     /// </summary>
//     private void UpdateJumpDuration()
//     {
//         var stateInfo = controller.enemyAnimator.GetCurrentAnimatorStateInfo(0);
//         float remainingTime = stateInfo.length * (1f - stateInfo.normalizedTime);
//
//         jumpDuration = remainingTime;
//         lapseForIndicator = 0f;
//
//         indicatorPos = controller.transform.position + moveSpeed * jumpDuration * transform.forward;
//         controller.indicatorCtrl.GetIndicatorTransform().localScale =
//             2f * attRange / controller.transform.localScale.x * Vector3.one;
//     }
// }

#endregion

using System.Collections;
using UnityEngine;

/// <summary>
/// Mon004의 점프 공격 패턴을 담당하는 상태 클래스
/// </summary>
public class Mon004State_Action : MonsterState_Action
{
    [SerializeField] private GameObject slamVFX;
    [SerializeField] private AudioClip[] jumpSFX;
    [SerializeField] private float attRange = 1.8f;

    private AudioSource audioSource;
    private int remainJump = 3;
    private float jumpDuration = 10f;
    private float maxRotation = Mathf.PI * 7f / 18f;
    private float moveSpeed = 4f;
    private Vector3 indicatorPos = Vector3.zero;
    private float lapseForIndicator = 0f;
    private bool isJumpStarted = false;

    /// <summary>
    /// 상태 전환 시 초기화 및 점프 코루틴 시작
    /// </summary>
    public override void TransitionAction(MonsterController controller)
    {
        base.TransitionAction(controller);

        if (isJumpStarted) return;
        isJumpStarted = true;

        audioSource = GetComponent<AudioSource>();
        StartCoroutine(JumpCoroutine());
    }

    /// <summary>
    /// 매 프레임 호출. 몬스터 이동 및 인디케이터 갱신
    /// </summary>
    public override void UpdateAction()
    {
        base.UpdateAction();
        controller.transform.position += moveSpeed * Time.deltaTime * controller.transform.forward;
        controller.indicatorCtrl.GetIndicatorTransform().position = indicatorPos;
        lapseForIndicator += Time.deltaTime;
        controller.indicatorCtrl.ControlIndicator(lapseForIndicator / jumpDuration);
    }

    /// <summary>
    /// 점프 패턴 반복 수행 코루틴
    /// </summary>
    private IEnumerator JumpCoroutine()
    {
        yield return null;

        Debug.Log($"[Mon004 Jump] 점프 시작, remainJump: {remainJump}");
        Sound_Manager.instance.PlayClip(jumpSFX[remainJump - 1]);

        RotateTowardPlayer();
        UpdateJumpDuration();

        controller.indicatorCtrl.ActivateIndicator();
        controller.indicatorCtrl.GetIndicatorTransform().localScale = 2f * attRange / controller.transform.localScale.x * Vector3.one;

        while (remainJump > 0)
        {
            Debug.Log($"[Mon004 Jump] 점프 대기 시작, remainJump: {remainJump}, duration: {jumpDuration}");
            yield return new WaitForSeconds(jumpDuration);

            Debug.Log($"[Mon004 Jump] 착지 처리 시작, remainJump: {remainJump}");
            ExecuteSlam();

            RotateTowardPlayer();
            yield return null;

            UpdateJumpDuration();
            Debug.Log($"[Mon004 Jump] 다음 점프 준비, remainJump: {remainJump}, next duration: {jumpDuration}");
        }

        Debug.Log("[Mon004 Jump] 모든 점프 종료, EnemyExplode 호출");
        controller.EnemyExplode();
    }

    /// <summary>
    /// 착지 시 공격 판정 및 이펙트 처리
    /// </summary>
    private void ExecuteSlam()
    {
        Vector3 playerDiff = controller.transform.position - controller.playerTransform.position;

        if (playerDiff.magnitude < attRange)
        {
            if (controller.playerTransform.TryGetComponent<PlayerStatManager>(out var player))
            {
                var monster = GetComponentInParent<IDamageAble>();
                var playerIDamageAble = GameObject.FindGameObjectWithTag("Player").GetComponent<IDamageAble>();

                CombatEvent combatEvent = new CombatEvent
                {
                    Sender = monster,
                    Receiver = playerIDamageAble,
                    Damage = (monster as Monster).skillDamages[3 - remainJump],
                    HitPosition = controller.transform.position,
                    Collider = monster.MainCollider,
                };

                CombatSystem.Instance.AddInGameEvent(combatEvent);
            }
        }

        slamVFX.transform.localScale = attRange / 1.8f * Vector3.one;
        slamVFX.SetActive(true);

        attRange *= 0.8f;

        if (remainJump > 0)
        {
            Sound_Manager.instance.PlayClip(jumpSFX[remainJump - 1]);
        }
        else
        {
            slamVFX.transform.SetParent(null, true);
            var mainPtc = slamVFX.GetComponent<ParticleSystem>().main;
            mainPtc.stopAction = ParticleSystemStopAction.Destroy;
        }

        remainJump--;
        Debug.Log($"[Mon004 Jump] remainJump 감소됨: {remainJump}");
    }

    /// <summary>
    /// 플레이어 방향으로 몬스터 회전 (최대 회전각 제한)
    /// </summary>
    private void RotateTowardPlayer()
    {
        Vector3 toPlayerVec = controller.playerTransform.position - controller.transform.position;

        float angle = Mathf.Acos(
            Mathf.Clamp(Vector3.Dot(controller.transform.forward, toPlayerVec.normalized), -0.999f, 0.999f));

        if (angle <= maxRotation)
        {
            controller.transform.forward = toPlayerVec.normalized;
        }
        else
        {
            float ccw = controller.transform.forward.IsCCRot(toPlayerVec) ? 1f : -1f;
            Quaternion rot = Quaternion.Euler(0f, ccw * maxRotation * Mathf.Rad2Deg, 0f);
            controller.transform.rotation *= rot;
        }
    }

    /// <summary>
    /// 점프 시간 및 인디케이터 위치 갱신
    /// </summary>
    private void UpdateJumpDuration()
    {
        var stateInfo = controller.enemyAnimator.GetCurrentAnimatorStateInfo(0);
        float remainingTime = stateInfo.length * (1f - stateInfo.normalizedTime);

        jumpDuration = remainingTime;
        lapseForIndicator = 0f;

        indicatorPos = controller.transform.position + moveSpeed * jumpDuration * transform.forward;
        controller.indicatorCtrl.GetIndicatorTransform().localScale =
            2f * attRange / controller.transform.localScale.x * Vector3.one;

            Debug.Log($"[Mon004 Jump] jumpDuration: {jumpDuration}, indicatorPos: {indicatorPos}");
    }
}