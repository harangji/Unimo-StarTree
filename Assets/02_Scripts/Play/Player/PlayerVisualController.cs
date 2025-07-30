using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class PlayerVisualController : MonoBehaviour
{
    [SerializeField] private GameObject hitFX;
    [SerializeField] private GameObject disappearPortal;
    [SerializeField] private ParticleSystem hitParticle;
    private Animator equipAnimator;
    private Animator characterAnimator;
    private PlayerFaceController faceController;
    private List<HitInvincibleBlinker> invincibleBlinkers;
    private bool isMoving;
    private bool isStun;
    private float specialAnimMinSpeed = 1f;
    private float specialAnimMaxSpeed = 3f;
    
    public void test_InitModeling(GameObject equipPrefab, GameObject playerPrefab)
    {
        GameObject eq = Instantiate(equipPrefab, transform);
        equipAnimator = eq.GetComponent<Animator>();
        eq.GetComponent<Equip_ChaSetter>().InstCharacter(playerPrefab, out characterAnimator, out faceController);
        equipAnimator.enabled = true;
        characterAnimator.enabled = true;
    }
    public void InitModelingAsync(float initTime, Action loadedAction)
    {
        loadedAction += () =>
        {
            StartCoroutine(CoroutineExtensions.DelayedActionCall(() =>
            {
                disappearPortal.SetActive(true);
                StartCoroutine(CoroutineExtensions.ScaleInterpCoroutine(transform.Find("Shadow"), new Vector3(1.5f, 1.5f, 1f), initTime));
            }, 0.3f));
        };
        Addressables.InstantiateAsync(AddressableKeyCtrl.EqAssetKey_Play(GameManager.Instance.EqIdx), transform).Completed += (op) =>
        {
            GameObject equip = op.Result.gameObject;
            equipAnimator = equip.GetComponent<Animator>();
            equip.GetComponent<Equip_ChaSetter>().InstCharacterAsync(AddressableKeyCtrl.ChaAssetKey_Play(GameManager.Instance.ChaIdx), loadedAction, InitChaAnimator);
        };
    }
    public void SetMovingAnim(bool ismove)
    {
        if (isMoving != ismove)
        {
            if (equipAnimator != null) { equipAnimator.SetBool("ismoving", ismove); }
            isMoving = ismove;
        }
    }
    public void SetStunAnim(bool isstun)
    {
        if (isStun != isstun)
        {
            if (isstun == true) 
            { 
                equipAnimator.SetTrigger("stun");
                hitFX.SetActive(true);
            }
            equipAnimator.SetBool("isstun", isstun);
            characterAnimator.SetBool("isstun", isstun);
            if (faceController != null) { faceController.ToggleBlink(isstun); }
            isStun = isstun;
        }
    }
    public void StartHitBlink(float duration)
    {
        for (int i = 0; i < invincibleBlinkers.Count; i++)
        {
            invincibleBlinkers[i].StartBlink(duration);
        }
    }
    public void SetHitFX(Vector3 hitpos, float duration)
    {
        var emission = hitParticle.emission;
        emission.enabled = true;
        int pnum = 2 + (int)(duration/0.58f);
        emission.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0.0f, pnum,1,0.01f) });
        float ltmulti = 1 + 0.075f*(int)(duration / 0.58f);
        var main = hitParticle.main;
        main.startLifetime = new ParticleSystem.MinMaxCurve(ltmulti*0.8f, ltmulti * 1.7f);

        Vector3 fxforward = hitpos - transform.position;
        fxforward.y = 0f;
        fxforward.Normalize();
        hitFX.transform.rotation = Quaternion.LookRotation(fxforward, Vector3.up);
    }
    public void TriggerDisappear()
    {
        equipAnimator.SetTrigger("disappear");
        characterAnimator.SetTrigger("disappear");
        StartCoroutine(CoroutineExtensions.DelayedActionCall(() => { disappearPortal.SetActive(true); }, 0.7f));
        StartCoroutine(CoroutineExtensions.ScaleInterpCoroutine(transform.Find("Shadow"), Vector3.zero, 1.5f));
    }
    public void SetEquipSpecialAnimSpeed(float ratio)
    {
        ratio = Mathf.Clamp01(ratio);
        float newspeed = (1f-ratio) * specialAnimMinSpeed + ratio * specialAnimMaxSpeed;
        // if (equipAnimator != null) { equipAnimator.SetFloat("movesync", newspeed); }
        // if (characterAnimator != null) { characterAnimator.SetFloat("movesync", newspeed); }
    }
    public void AddInvincibleBlinker(HitInvincibleBlinker blinker)
    {
        if (invincibleBlinkers == null) { invincibleBlinkers = new List<HitInvincibleBlinker>(); }
        invincibleBlinkers.Add(blinker);
    }
    private void InitChaAnimator(GameObject chaObj)
    {
        characterAnimator = chaObj.GetComponent<Animator>();
        faceController = chaObj.GetComponent<PlayerFaceController>();
        equipAnimator.enabled = true;
        characterAnimator.enabled = true;
    }
}
