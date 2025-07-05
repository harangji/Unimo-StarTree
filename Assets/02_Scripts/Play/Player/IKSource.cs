using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKSource : MonoBehaviour
{
    private Transform originTransform;
    private Vector3 localVelocity;
    private Vector3 prevLocalPos;

    private float radius;
    [SerializeField] private float maxTheta_inAngle = 45;
    [SerializeField] private float torsionCoeff = 100f;
    [SerializeField] private float drag = 12;
    public void Initialize(Transform startBone)
    {
        originTransform = startBone;
        radius = (transform.position - originTransform.position).magnitude;
        localVelocity = Vector3.zero;
        maxTheta_inAngle *= Mathf.PI / 180f;
        prevLocalPos = calcPseudoLocalPos();
    }
    public void ApplyInertia(Vector3 inertiaAcc, Vector3 inertiaVel)
    {
        Vector3 pos = calcPseudoLocalPos();
        float cosV = Mathf.Clamp01(Vector3.Dot(originTransform.up, prevLocalPos.normalized));
        float theta0 = Mathf.Acos(cosV);
        Vector3 Fext = - inertiaAcc - drag * inertiaVel;
        Fext -= Vector3.Dot(Fext, pos.normalized) * pos.normalized;
        Vector3 projToOriginXZ = pos - Vector3.Dot(originTransform.up, pos) * originTransform.up;
        projToOriginXZ /= (projToOriginXZ.magnitude + 0.0001f);
        Vector3 toCenterVec = -Mathf.Cos(theta0) * Vector3.Dot(projToOriginXZ, originTransform.right) * originTransform.right +
            -Mathf.Cos(theta0) * Vector3.Dot(projToOriginXZ, originTransform.forward) * originTransform.forward +
            Mathf.Sin(theta0) * originTransform.up;
        //Vector3 toCenterVec = radius * Vector3.Dot(originTransform.up, pos) * originTransform.up - pos;
        Vector3 torsionF = radius * torsionCoeff * theta0 * toCenterVec;
        Vector3 localDrag = -drag * localVelocity;
        
        Vector3 dpos = Time.deltaTime * (Time.deltaTime * (Fext + torsionF + localDrag) + 
            localVelocity);
        setPseudoLocalPos(prevLocalPos + dpos);
        setPseudoLocalPos(radius * calcPseudoLocalPos().normalized);
        Vector3 newlocalpos = calcPseudoLocalPos();
        cosV = Mathf.Clamp01(Vector3.Dot(originTransform.up, newlocalpos.normalized));
        float newTheta = Mathf.Acos(cosV);
        if (newTheta > maxTheta_inAngle)
        {
            projToOriginXZ = newlocalpos - Vector3.Dot(originTransform.up, newlocalpos) * originTransform.up;
            projToOriginXZ.Normalize();
            Vector3 limitPos = radius *
                (Mathf.Sin(maxTheta_inAngle) * Vector3.Dot(projToOriginXZ, originTransform.right) * originTransform.right +
            Mathf.Sin(maxTheta_inAngle) * Vector3.Dot(projToOriginXZ, originTransform.forward) * originTransform.forward +
            Mathf.Cos(maxTheta_inAngle) * originTransform.up);
            setPseudoLocalPos(limitPos);
        }
        Vector3 actdPos = calcPseudoLocalPos() - prevLocalPos;
        localVelocity = actdPos / Time.deltaTime;
        prevLocalPos = calcPseudoLocalPos();
    }
    private Vector3 calcPseudoLocalPos()
    {
        return transform.position - originTransform.position;
    }
    private void setPseudoLocalPos(Vector3 localPos)
    {
        transform.position = originTransform.position + localPos;
    }
}
