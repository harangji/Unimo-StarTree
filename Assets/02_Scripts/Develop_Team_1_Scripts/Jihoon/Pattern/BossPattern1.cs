using UnityEngine;

public class BossPattern1 : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4.2f;

    private Vector3 moveDir;

    private void Start()
    {
        SetDirection();
    }

    private void Update()
    {
        Move();
    }

    private void SetDirection()
    {
        var playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        
        moveDir = (playerPos - transform.position).normalized;
        moveDir.y = 0f;

        if (moveDir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDir);
        }
    }

    private void Move()
    {
        transform.position += transform.forward * (moveSpeed * Time.deltaTime);
    }
}