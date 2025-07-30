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

        originalGrowth = 12f * auraStrength; // 기본 성장 속도 × 배율
        growthperSec = originalGrowth;
        Debug.Log($"[AuraController] 아우라 초기화됨 → Range: {range}, Scale: {transform.localScale}, Growth: {growthperSec}");
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

        // **1. 각도 먼저 증가(이동)**
        mAngle += mRotateSpeed * Mathf.Deg2Rad * Time.deltaTime;

        // 2. 현재 위치 계산
        float x = Mathf.Cos(mAngle) * mRadius;
        float z = Mathf.Sin(mAngle) * mRadius;
        Vector3 orbitPos = mTarget.position + new Vector3(x, 0.5f, z);

        // 3. '조금 더 앞선 각도'의 다음 위치 계산 (방향 추정용)
        float nextAngle = mAngle + mRotateSpeed * Mathf.Deg2Rad * Time.deltaTime * 1.5f;
        float nextX = Mathf.Cos(nextAngle) * mRadius;
        float nextZ = Mathf.Sin(nextAngle) * mRadius;
        Vector3 nextOrbitPos = mTarget.position + new Vector3(nextX, 0.5f, nextZ);

        // 4. 이동 방향 벡터
        Vector3 dir = (nextOrbitPos - orbitPos);
        dir.y = 0; // y값 고정 (평면 회전만)
        if (dir.sqrMagnitude > 0.001f)
            transform.forward = -dir.normalized; 

        // 5. 실제 위치 이동
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
        gameObject.SetActive(false); // OrbitAuraController도 비활성화
    }
    
}
