using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    public bool IsStop { get; set; }=false;
    
    protected Transform playerTransform;
    protected float moveSpeed = 8.5f; //8.5 at max Lv
    protected Vector3 pushDir = Vector3.zero;
    protected float pushSpeed = 4f;
    protected MapRangeSetter mapSetter;
    protected AuraController auraCtrl;
    protected float auraOffset;

    public Action IKActions;
    protected PlayerVisualController visualCtrl;
    protected AudioSource moveAudio;
    protected float moveSoundMax;
    // Start is called before the first frame update
    protected void Start()
    {
        playerTransform = transform;
        mapSetter = PlaySystemRefStorage.mapSetter;
        visualCtrl = GetComponent<PlayerVisualController>();
        moveAudio = GetComponent<AudioSource>();
        moveSoundMax = moveAudio.volume;
        IsStop = true;
    }
    public void FindAuraCtrl(AuraController aura)
    {
        auraCtrl = aura;
        auraOffset = auraCtrl.transform.position.y;
    }
    
    public void SetSpeed(float speed)
    {
        moveSpeed = speed;

        // 예시: Floating 버프가 적용되면 최고속도 강제 적용
        if (Base_Manager.Data.UserData.BuffFloating[0] >= 0.0f)
        {
            moveSpeed = 8.5f;
        }
    }

    // 스탯 적용.정현식
    public void SetCharacterStat(UnimoRuntimeStat stat)
    {
        moveSpeed = stat.FinalStat.MoveSpd;
        // 향후 확장 가능 예시
        // pushSpeed = stat.FinalStat.AuraStr * 2f;
        // Debug.Log($"[PlayerMover] 이동속도 설정됨: {moveSpeed}");
    }
    
    
    //원본.정현식
    //public void SetSpeed(float speed)
    //{
    //    moveSpeed = speed;
    //    if (Base_Manager.Data.UserData.BuffFloating[0] >= 0.0f)
    //        moveSpeed = 8.5f;
    //    else moveSpeed = 8.5f;
    //}
    public void StunPush(float stunTime, Vector3 hitPos)
    {
        float duration = 0.6f * stunTime;
        StartCoroutine(pushMoveCoroutine(duration, hitPos));
    }
    protected void pushMove(Vector2 direction)
    {
        if (direction.magnitude < 0.01f) 
        {
            pushDir = Vector3.zero;
            return; 
        }
        pushDir = new Vector3(direction.x, 0, direction.y);
    }
    protected IEnumerator pushMoveCoroutine(float duration, Vector3 hitPos)
    {
        Vector3 dir = playerTransform.position - hitPos;
        dir.y = 0f;
        dir.Normalize();
        Vector2 dirin = new Vector2(dir.x, dir.z);
        duration = Mathf.Min(duration, 1f);
        float lapsetime = 0f;
        while (lapsetime < duration)
        {
            lapsetime += Time.deltaTime;
            float ratio = Mathf.Cos(0.5f * Mathf.PI * lapsetime / duration);
            pushMove(ratio * dirin);
            yield return null;
        }
        pushDir = Vector3.zero;
        yield break;
    }
}
