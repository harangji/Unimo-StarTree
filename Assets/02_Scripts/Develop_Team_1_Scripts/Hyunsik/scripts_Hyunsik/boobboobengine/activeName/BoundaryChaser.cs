using UnityEngine;

public class BoundaryChaser : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    private Transform player;
    private MapRSetter_Circular mapRangeSetter;
    private Vector3 moveDir;

    void Start()
    {
        player = FindObjectOfType<PlayerStatManager>().transform;
        mapRangeSetter = FindObjectOfType<MapRSetter_Circular>();
        UpdateMoveDirToPlayer();
    }

    void Update()
    {
        // 1. moveDir 방향으로 무조건 직진 (플레이어를 뚫고 감)
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        // 2. 맵 경계 바깥으로 나가면
        if (!mapRangeSetter.IsInMap(transform.position))
        {
            // 3. 경계에 붙이기
            transform.position = mapRangeSetter.FindNearestPoint(transform.position);

            // 4. "현재 시점의 플레이어 위치" 방향으로 다시 moveDir 갱신
            UpdateMoveDirToPlayer();
        }
    }

    private void UpdateMoveDirToPlayer()
    {
        if (player != null)
        {
            Vector3 toPlayer = player.position - transform.position;
            toPlayer.y = 0; // 평면 이동시 y 무시
            moveDir = toPlayer.normalized;
        }
    }
}