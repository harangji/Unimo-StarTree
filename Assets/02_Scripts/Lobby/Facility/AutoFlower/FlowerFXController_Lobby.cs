using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerFXController_Lobby : MonoBehaviour
{
    [SerializeField] private GameObject harvestVFX;
    [SerializeField] private AudioSource harvestSFX;
    [SerializeField] private string bloomAnimClipname = "anim_FLO001_Blossom";

    private Animator anim;
    private float randAngle = 20f;
    private float randScale = 0.12f;

    private float lastRatio;
    private float followSpeed = 0.05f;
    // Start is called before the first frame update
    private void Start()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
        float angle = randAngle * Random.Range(-1f, 1f);
        float scale = randScale * Random.Range(-1f, 1f);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
        transform.localScale *= 1 + scale;
    }

    public void AffectFlowerFX(float growth)
    {
        if (isinBlossom())
        {
            float ratio = Mathf.Clamp01(growth);
            calculateNewLastRatio(ratio);
            jumpToTime(bloomAnimClipname, lastRatio);
        }
    }
    public void TriggerHarvestFX()
    {
        StartCoroutine(harvestFXCoroutine(null));
        jumpToTime(bloomAnimClipname, 1f);
        anim.SetBool("isharvested", true);
        anim.SetLayerWeight(1, 0);
    }
    public void TriggerHarvestFX(Transform unimoPos)
    {
        StartCoroutine(harvestFXCoroutine(unimoPos));
        jumpToTime(bloomAnimClipname, 1f);
        anim.SetBool("isharvested", true);
        anim.SetLayerWeight(1, 0);
    }
    public void SetFlowerActivate()
    {
        anim.SetBool("isactive", true);
    }
    private bool isinBlossom()
    {
        if (anim.GetCurrentAnimatorStateInfo(1).IsName(bloomAnimClipname))
        {
            return true;
        }
        return false;
    }
    private void jumpToTime(string name, float nTime)
    {
        anim.Play(name, 1, nTime);
    }
    private void calculateNewLastRatio(float ratio)
    {
        lastRatio = followSpeed * ratio + (1 - followSpeed) * lastRatio;
    }
    protected IEnumerator harvestFXCoroutine(Transform unimoPos)
    {
        yield return new WaitForSeconds(0.28f);
        float normTime = 0f;
        while (normTime <= 0.6666f)
        {
            normTime = anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
            yield return null;
        }
        harvestSFX.volume = Sound_Manager.FX_volume;
        harvestSFX.gameObject.SetActive(true);
        harvestVFX.SetActive(true);
        harvestVFX.transform.SetParent(null, true);
        harvestVFX.transform.rotation = Quaternion.Euler(-30f, 180f, 0f);
        harvestVFX.GetComponent<FlowerHarvestFX_Lobby>().SetUnimoTransform(unimoPos);

        // ·Îºñ¿¡¼­ ²ÉÀ» ÄºÀ» ? È¹µæÇÏ´Â ÀçÈ­µµ ¸·¾Æ³ù½À´Ï´Ù.
        // double reward = Base_Manager.Data.UserData.Second_Base;
        // if (Base_Manager.Data.UserData.BuffFloating[0] > 0.0f)
        // {
        //     reward *= 1.3f;
        // }
        // Base_Manager.Data.AssetPlus(Asset_State.Yellow, reward);
        // Land.instance.FlowerValue--;
        //
        // var go = Instantiate(Resources.Load<Get_TEXT>("TextMESH"));
        // go.gameObject.SetActive(false); //±ôºýÀÌ´Â Çö»ó ¼öÁ¤
        // go.Init(transform.position + new Vector3(0, 3.0f, 0), reward);

        yield break;
    }
}
