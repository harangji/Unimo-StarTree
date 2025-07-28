using UnityEngine;

public class OrbitAuraController : MonoBehaviour
{
    private Transform mTarget;
    private float mRadius = 3.0f;
    private float mAngle = 0.0f;
    private float mRotateSpeed = 120f; // degree/sec

    public void SetTarget(Transform target, float radius, float startAngle = 0f)
    {
        mTarget = target;
        mRadius = radius;
        mAngle = startAngle;
    }

    void Update()
    {
        // [위치 공전 로직] : 기존 AuraController에는 없음, 여기서만!
        if (mTarget == null) return;
        mAngle += mRotateSpeed * Mathf.Deg2Rad * Time.deltaTime;
        float x = Mathf.Cos(mAngle) * mRadius;
        float z = Mathf.Sin(mAngle) * mRadius;
        Vector3 orbitPos = mTarget.position + new Vector3(x, 0.5f, z); // y=0.5f 띄움
        transform.position = orbitPos;

        // [Aura 기능]: 부모 Update() 호출은 필요 없음
        // - OnTriggerStay, InitAura, Shrink, Resume 등은 상속된 그대로 작동
    }
}
