using System;
using System.Collections;
using UnityEngine;

public class Mon5Pattern2Controller : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    private Vector3 moveDirection = Vector3.zero;
    private bool canMove = false;
    
    private IEnumerator Start()
    {
        SetDirection();
        
        yield return new WaitForSeconds(1f);
        
        canMove = true;
    }

    private void Update()
    {
        if (!canMove) return;

        transform.position += moveDirection * (moveSpeed * Time.deltaTime);
    }

    private void SetDirection()
    {
        var mover = FindAnyObjectByType<PlayerMover>();
        if (mover == null)
        {
            Debug.LogWarning("Mon5Controller: No player mover found");
            return;
        }

        Vector3 targetPos = mover.transform.position;
        Vector3 dir = targetPos - transform.position;

        // XZ 평면에서만 방향 설정
        dir.y = 0f;
        moveDirection = dir.normalized;

        // Y축 회전만 적용 (XZ 평면에서 바라보는 방향)
        if (dir != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
        }
    }

}