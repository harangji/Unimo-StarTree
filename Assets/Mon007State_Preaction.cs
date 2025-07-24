using System.Collections;
using UnityEngine;

public class Mon007State_Preaction : MonsterState_Preaction
{
    private static readonly int BOMB = Animator.StringToHash("Bomb");

    [SerializeField] private float moveSpeed = 6f;

    public override void TransitionAction(MonsterController controller)
    {
        this.controller = controller;
        Debug.Log("[자폭병] 프리액션 진입");
    }
    public override void UpdateAction()
    {
        RotateMonster();
        MoveMonster();
        
        float distanceToPlayer = Vector3.Distance(controller.transform.position, controller.playerTransform.position);
        if (distanceToPlayer < 3f)
        {
            StartCoroutine(StartAnimation());
            invokeActionState();
        }
    }

    private IEnumerator StartAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        controller.enemyAnimator.SetTrigger(BOMB);
    }
    
    private void RotateMonster()
    {
        Vector3 toPlayer = controller.playerTransform.position - controller.transform.position;
        toPlayer.y = 0f;

        if (toPlayer != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(toPlayer);
            controller.transform.rotation = targetRotation;
        }
    }


    private void MoveMonster()
    {
        Vector3 direction = transform.forward;
        direction.y = 0f;
        direction.Normalize();

        controller.transform.position += moveSpeed * Time.deltaTime * direction;
    }
}
