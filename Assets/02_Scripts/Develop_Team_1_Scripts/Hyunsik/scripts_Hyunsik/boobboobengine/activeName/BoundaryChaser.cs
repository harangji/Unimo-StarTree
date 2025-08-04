using UnityEngine;

public class BoundaryChaser : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    private Transform player;
    private MapRSetter_Circular mapRangeSetter;
    private Vector3 moveDir;

    private float growthperSec = 30f;
    private float originalGrowth = 12f;
    private Vector3 originalScale;

    void Start()
    {
        player = FindObjectOfType<PlayerStatManager>()?.transform;
        mapRangeSetter = FindObjectOfType<MapRSetter_Circular>();
        UpdateMoveDirToPlayer();
    }

    void OnEnable()
    {
        PlayerStatManager.OnPlayerActiveChanged += HandlePlayerActiveChanged;
    }
    void OnDisable()
    {
        PlayerStatManager.OnPlayerActiveChanged -= HandlePlayerActiveChanged;
    }

    private void HandlePlayerActiveChanged(bool isActive)
    {
        gameObject.SetActive(isActive);
        // �Ǵ� enabled = isActive; (������Ʈ�� ��Ȱ��ȭ)
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Flower"))
        {
            if (other.TryGetComponent<FlowerController>(out var flower))
            {
                flower.AuraAffectFlower(growthperSec * Time.fixedDeltaTime);
            }
        }
    }

    public void InitAura(float range, float auraStrength)
    {
        transform.localScale = range * Vector3.one;
        originalScale = transform.localScale;
        originalGrowth = 12f * auraStrength;
        growthperSec = originalGrowth;
        Debug.Log($"[AuraController] �ƿ�� �ʱ�ȭ�� �� Range: {range}, Scale: {transform.localScale}, Growth: {growthperSec}");
    }

    void Update()
    {
        // �÷��̾ ������ �̵� ����
        if (player == null || mapRangeSetter == null)
            return;

        transform.position += moveDir * moveSpeed * Time.deltaTime;

        if (!mapRangeSetter.IsInMap(transform.position))
        {
            transform.position = mapRangeSetter.FindNearestPoint(transform.position);
            UpdateMoveDirToPlayer();
        }
    }

    private void UpdateMoveDirToPlayer()
    {
        if (player != null)
        {
            Vector3 toPlayer = player.position - transform.position;
            toPlayer.y = 0;
            moveDir = toPlayer.normalized;
        }
    }
}