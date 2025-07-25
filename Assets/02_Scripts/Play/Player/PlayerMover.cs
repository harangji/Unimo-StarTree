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

        // ����: Floating ������ ����Ǹ� �ְ�ӵ� ���� ����
        if (Base_Manager.Data.UserData.BuffFloating[0] >= 0.0f)
        {
            moveSpeed = 8.5f;
        }
    }

    // ���� ����.������
    public void SetCharacterStat(UnimoRuntimeStat stat)
    {
        moveSpeed = stat.FinalStat.MoveSpd;
        Debug.Log($"[PlayerMover] �̵��ӵ� ������: {moveSpeed}");
        // ���� Ȯ�� ���� ����
        // pushSpeed = stat.FinalStat.AuraStr * 2f;
        // Debug.Log($"[PlayerMover] �̵��ӵ� ������: {moveSpeed}");
    }


    //����.������
    //public void SetSpeed(float speed)
    //{
    //    moveSpeed = speed;
    //    if (Base_Manager.Data.UserData.BuffFloating[0] >= 0.0f)
    //        moveSpeed = 8.5f;
    //    else moveSpeed = 8.5f;
    //}

    /// <summary>
    /// �ǰ� ��ġ�� �������� �з����� ����, ����, ���ӽð��� �����ϰ� ���� Ǫ�� �ڷ�ƾ�� �����Ѵ�.
    /// </summary>
    /// <param name="stunTime">���� ��ü �ð�</param>
    /// <param name="hitPos">�ǰݴ��� ��ġ</param>
    /// <param name="pushPower">�з����� ���� (�⺻�� 1)</param>
    public void StunPush(float stunTime, Vector3 hitPos, float pushPower)
    {
        float pushDuration = Mathf.Min(0.6f * stunTime, 1f);
        
        Vector3 pushDirection = pushPower == 1 ? GetPushDirection(hitPos) : -GetPushDirection(hitPos);

        StartCoroutine(PushCoroutine(pushDuration, pushDirection, pushPower));
    }

    /// <summary>
    /// �ǰ� ��ġ�� �������� �÷��̾ �з��� ������ ����Ѵ�.
    /// </summary>
    /// <param name="hitPos">�ǰ� ��ġ</param>
    /// <returns>����ȭ�� �з��� ���� ����</returns>
    private Vector3 GetPushDirection(Vector3 hitPos)
    {
        Vector3 dir = playerTransform.position - hitPos;
        dir.y = 0f;
        return dir.normalized;
    }

    /// <summary>
    /// ���� �ð� ���� ���� �����ϴ� ������ �÷��̾ �о�� �ڷ�ƾ�̴�.
    /// </summary>
    /// <param name="duration">Ǫ�� ���� �ð�</param>
    /// <param name="direction">����ȭ�� �з��� ����</param>
    /// <param name="power">Ǫ�� ���� ���</param>
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
    /// ���� ���� ���͸� 3D �������� ��ȯ�Ͽ� ���� �̵� ����(pushDir)�� �����Ѵ�.
    /// </summary>
    /// <param name="pushVec">�з����� 2D ���� ����</param>
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