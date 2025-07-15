using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mon006State_Action : MonsterState_Action
{
    [SerializeField] private Transform laserPivot;
    [SerializeField] private GameObject laserFXObj;
    [SerializeField] private GameObject laserEndFXObj;
    [SerializeField] private LineRenderer laserBodyRender;
    [SerializeField] private Custom3DAudio fireSFX3D;
    [SerializeField] private Custom3DAudio shootSFX3D;
    [SerializeField] private float fireDuration = 2.3f;
    [SerializeField] private float laserWidth = 1.5f;
    private float remainDuration;
    private bool isFiring = false;
    public override void TransitionAction(MonsterController controller)
    {
        base.TransitionAction(controller);
        remainDuration = fireDuration;
        controller.enemyAnimator.SetTrigger("action");
        StartCoroutine(fireCoroutine());
    }
    public override void FixedUpdateAction()
    {
        base.FixedUpdateAction();
        if (isFiring) 
        {
            shootSFX3D.transform.position = calcProjectedSFXPos();
            hitPlayer(); 
        }
    }
    public float GetLWidth()
    {
        return laserWidth;
    }
    private void hitPlayer()
    {
        Vector3 toPlayerVec = controller.playerTransform.position - laserPivot.position;
        float innerP = laserPivot.forward.x * toPlayerVec.x +
            laserPivot.forward.y * toPlayerVec.y +
            laserPivot.forward.z * toPlayerVec.z;
        if (innerP < 0) { return; }
        Vector3 projectedVec = innerP * laserPivot.forward;
        if ((toPlayerVec-projectedVec).magnitude < (laserWidth/2 + 0.55f))
        {
            if (controller.playerTransform.TryGetComponent<PlayerStatManager>(out var player))
            {
                // player.Hit(Mathf.Max(remainDuration, 0.3f) + 0.5f, laserPivot.position + projectedVec);
            }
        }
    }
    private void startFireFX()
    {
        Vector3[] vP = new Vector3[laserBodyRender.positionCount];
        laserBodyRender.GetPositions(vP);
        vP[2] = new Vector3(0, 0, Mathf.Abs(controller.indicatorCtrl.GetIndicatorTransform().position.y) / controller.transform.localScale.x);
        laserBodyRender.SetPositions(vP);
        laserBodyRender.widthMultiplier = 1.2f * laserWidth;
        Vector2 endPos = laserEndFXObj.transform.position;
        endPos.y = 0;
        laserEndFXObj.transform.position = endPos;
        laserFXObj.SetActive(true);
    }
    private void stopFireFX()
    {
        laserFXObj.GetComponent<Animator>().SetTrigger("end");
    }
    private IEnumerator fireCoroutine()
    {
        yield return new WaitForSeconds(0.3f);
        float animTime = controller.enemyAnimator.GetCurrentAnimatorStateInfo(0).length;
        animTime *= 1f - controller.enemyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        animTime -= 1f/6f;
        yield return new WaitForSeconds(animTime);
        controller.indicatorCtrl.DeactivateIndicator();
        isFiring = true;
        fireSFX3D.transform.position = calcProjectedSFXPos();
        shootSFX3D.transform.position = calcProjectedSFXPos();
        startFireFX();
        while (remainDuration > 0)
        {
            remainDuration -= Time.deltaTime;
            yield return null;
        }
        isFiring = false;
        float sfxDecayT = 0.25f;
        float lapse = 0f;
        while (lapse<sfxDecayT)
        {
            lapse += Time.deltaTime;
            float ratio = Mathf.Clamp01(1-lapse/sfxDecayT);
            shootSFX3D.SetMaxVolume(0.6f * ratio);
        }
        stopFireFX();
        controller.EnemyDisappear();

        yield break;
    }
    private Vector3 calcProjectedSFXPos()
    {
        Vector3 toPlayer = controller.playerTransform.position - controller.transform.position;
        Vector3 toSFX = Vector3.Dot(toPlayer, controller.transform.forward) * controller.transform.forward;
        toSFX += controller.transform.position;
        return toSFX;
    }
}
