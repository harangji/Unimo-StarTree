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
        // 1. moveDir �������� ������ ���� (�÷��̾ �հ� ��)
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        // 2. �� ��� �ٱ����� ������
        if (!mapRangeSetter.IsInMap(transform.position))
        {
            // 3. ��迡 ���̱�
            transform.position = mapRangeSetter.FindNearestPoint(transform.position);

            // 4. "���� ������ �÷��̾� ��ġ" �������� �ٽ� moveDir ����
            UpdateMoveDirToPlayer();
        }
    }

    private void UpdateMoveDirToPlayer()
    {
        if (player != null)
        {
            Vector3 toPlayer = player.position - transform.position;
            toPlayer.y = 0; // ��� �̵��� y ����
            moveDir = toPlayer.normalized;
        }
    }
}