#region ����

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
//     /// ���� ��ȯ �� �ʱ�ȭ �� ���� �ڷ�ƾ ����
//     /// </summary>
//     public override void TransitionAction(MonsterController controller)
//     {
//         base.TransitionAction(controller);
//         audioSource = GetComponent<AudioSource>();
//         StartCoroutine(JumpCoroutine());
//     }
//
//     /// <summary>
//     /// �� ������ ȣ��. ���� �̵� �� �ε������� ����
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
//     /// ���� ���� �ݺ� ���� �ڷ�ƾ
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
//     /// ���� �� ���� ���� �� ����Ʈ ó��
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
//     /// �÷��̾� �������� ���� ȸ�� (�ִ� ȸ���� ����)
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
//     /// ���� �ð� �� �ε������� ��ġ ����
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
/// Mon004�� ���� ���� ������ ����ϴ� ���� Ŭ����
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
    /// ���� ��ȯ �� �ʱ�ȭ �� ���� �ڷ�ƾ ����
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
    /// �� ������ ȣ��. ���� �̵� �� �ε������� ����
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
    /// ���� ���� �ݺ� ���� �ڷ�ƾ
    /// </summary>
    private IEnumerator JumpCoroutine()
    {
        yield return null;

        Debug.Log($"[Mon004 Jump] ���� ����, remainJump: {remainJump}");
        Sound_Manager.instance.PlayClip(jumpSFX[remainJump - 1]);

        RotateTowardPlayer();
        UpdateJumpDuration();

        controller.indicatorCtrl.ActivateIndicator();
        controller.indicatorCtrl.GetIndicatorTransform().localScale = 2f * attRange / controller.transform.localScale.x * Vector3.one;

        while (remainJump > 0)
        {
            Debug.Log($"[Mon004 Jump] ���� ��� ����, remainJump: {remainJump}, duration: {jumpDuration}");
            yield return new WaitForSeconds(jumpDuration);

            Debug.Log($"[Mon004 Jump] ���� ó�� ����, remainJump: {remainJump}");
            ExecuteSlam();

            RotateTowardPlayer();
            yield return null;

            UpdateJumpDuration();
            Debug.Log($"[Mon004 Jump] ���� ���� �غ�, remainJump: {remainJump}, next duration: {jumpDuration}");
        }

        Debug.Log("[Mon004 Jump] ��� ���� ����, EnemyExplode ȣ��");
        controller.EnemyExplode();
    }

    /// <summary>
    /// ���� �� ���� ���� �� ����Ʈ ó��
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
        Debug.Log($"[Mon004 Jump] remainJump ���ҵ�: {remainJump}");
    }

    /// <summary>
    /// �÷��̾� �������� ���� ȸ�� (�ִ� ȸ���� ����)
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
    /// ���� �ð� �� �ε������� ��ġ ����
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