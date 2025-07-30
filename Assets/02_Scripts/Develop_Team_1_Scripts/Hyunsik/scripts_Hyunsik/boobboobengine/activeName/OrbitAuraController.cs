using UnityEngine;

public class OrbitAuraController : MonoBehaviour
{
    private Transform mTarget;
    private float mRadius = 3.0f;
    private float mAngle = 0.0f;
    private float mRotateSpeed = -120f; // degree/sec

    private float growthperSec = 12f;
    private float originalGrowth = 12f;
    private Vector3 originalScale;
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

        originalGrowth = 12f * auraStrength; // �⺻ ���� �ӵ� �� ����
        growthperSec = originalGrowth;
        Debug.Log($"[AuraController] �ƿ�� �ʱ�ȭ�� �� Range: {range}, Scale: {transform.localScale}, Growth: {growthperSec}");
    }

    
    public void SetTarget(Transform target, float radius, float startAngle = 0f)
    {
        mTarget = target;
        mRadius = radius;
        mAngle = startAngle;
    }

    void Update()
    {
        if (mTarget == null) return;

        // **1. ���� ���� ����(�̵�)**
        mAngle += mRotateSpeed * Mathf.Deg2Rad * Time.deltaTime;

        // 2. ���� ��ġ ���
        float x = Mathf.Cos(mAngle) * mRadius;
        float z = Mathf.Sin(mAngle) * mRadius;
        Vector3 orbitPos = mTarget.position + new Vector3(x, 0.5f, z);

        // 3. '���� �� �ռ� ����'�� ���� ��ġ ��� (���� ������)
        float nextAngle = mAngle + mRotateSpeed * Mathf.Deg2Rad * Time.deltaTime * 1.5f;
        float nextX = Mathf.Cos(nextAngle) * mRadius;
        float nextZ = Mathf.Sin(nextAngle) * mRadius;
        Vector3 nextOrbitPos = mTarget.position + new Vector3(nextX, 0.5f, nextZ);

        // 4. �̵� ���� ����
        Vector3 dir = (nextOrbitPos - orbitPos);
        dir.y = 0; // y�� ���� (��� ȸ����)
        if (dir.sqrMagnitude > 0.001f)
            transform.forward = -dir.normalized; 

        // 5. ���� ��ġ �̵�
        transform.position = orbitPos;
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
    }
    private void HandleAuraDisabled()
    {
        gameObject.SetActive(false); // OrbitAuraController�� ��Ȱ��ȭ
    }
    
}
