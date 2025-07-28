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
        // [��ġ ���� ����] : ���� AuraController���� ����, ���⼭��!
        if (mTarget == null) return;
        mAngle += mRotateSpeed * Mathf.Deg2Rad * Time.deltaTime;
        float x = Mathf.Cos(mAngle) * mRadius;
        float z = Mathf.Sin(mAngle) * mRadius;
        Vector3 orbitPos = mTarget.position + new Vector3(x, 0.5f, z); // y=0.5f ���
        transform.position = orbitPos;

        // [Aura ���]: �θ� Update() ȣ���� �ʿ� ����
        // - OnTriggerStay, InitAura, Shrink, Resume ���� ��ӵ� �״�� �۵�
    }
}
