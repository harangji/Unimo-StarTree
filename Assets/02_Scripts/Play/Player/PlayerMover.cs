using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    public bool IsStop { get; set; } = false;

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
        Debug.Log($"[PlayerMover] 이동속도 설정됨: {moveSpeed}");
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

    /// <summary>
    /// 피격 위치를 기준으로 밀려나는 방향, 세기, 지속시간을 설정하고 스턴 푸시 코루틴을 시작한다.
    /// </summary>
    /// <param name="stunTime">스턴 전체 시간</param>
    /// <param name="hitPos">피격당한 위치</param>
    /// <param name="pushPower">밀려나는 세기 (기본값 1)</param>
    public void StunPush(float stunTime, Vector3 hitPos, float pushPower)
    {
        float pushDuration = Mathf.Min(0.6f * stunTime, 1f);
        
        Vector3 pushDirection = pushPower == 1 ? GetPushDirection(hitPos) : -GetPushDirection(hitPos);

        StartCoroutine(PushCoroutine(pushDuration, pushDirection, pushPower));
    }

    /// <summary>
    /// 피격 위치를 기준으로 플레이어가 밀려날 방향을 계산한다.
    /// </summary>
    /// <param name="hitPos">피격 위치</param>
    /// <returns>정규화된 밀려날 방향 벡터</returns>
    private Vector3 GetPushDirection(Vector3 hitPos)
    {
        Vector3 dir = playerTransform.position - hitPos;
        dir.y = 0f;
        return dir.normalized;
    }

    /// <summary>
    /// 일정 시간 동안 점차 감소하는 힘으로 플레이어를 밀어내는 코루틴이다.
    /// </summary>
    /// <param name="duration">푸시 지속 시간</param>
    /// <param name="direction">정규화된 밀려날 방향</param>
    /// <param name="power">푸시 강도 계수</param>
    private IEnumerator PushCoroutine(float duration, Vector3 direction, float power)
    {
        float elapsed = 0f;
        Vector2 flatDir = new Vector2(direction.x, direction.z);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float easeRatio = Mathf.Cos(0.5f * Mathf.PI * elapsed / duration);
            ApplyPush(flatDir * (easeRatio * power));
            yield return null;
        }

        pushDir = Vector3.zero;
    }

    /// <summary>
    /// 계산된 방향 벡터를 3D 방향으로 변환하여 실제 이동 방향(pushDir)에 적용한다.
    /// </summary>
    /// <param name="pushVec">밀려나는 2D 방향 벡터</param>
    private void ApplyPush(Vector2 pushVec)
    {
        if (pushVec.sqrMagnitude < 0.0001f)
        {
            pushDir = Vector3.zero;
            return;
        }

        pushDir = new Vector3(pushVec.x, 0f, pushVec.y);
    }
}