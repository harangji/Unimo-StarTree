using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatManager : MonoBehaviour
{
    private static readonly float invincibleTime = 0.5f;

    //private PlayerHPspriter HPspriter;
    private PlayerMover playerMover;
    private AuraController auraController;
    private PlayerVisualController visualCtrl;
    private GameObject renderCam;

    private bool isInvincible = false;
    private Coroutine stunCoroutine;

    [SerializeField] private bool isTestModel = false;
    [SerializeField] private GameObject equipPrefab;
    [SerializeField] private GameObject chaPrefab;

    private int hp = 10;
    
    private void Awake()
    {
        PlaySystemRefStorage.playerStatManager = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        transform.position = -1000f * Vector3.one;
        renderCam = GameObject.Find("RenderCam");
        renderCam.SetActive(false);
        playerMover = GetComponent<PlayerMover>();
        visualCtrl = GetComponent<PlayerVisualController>();
        if (isTestModel)
        {
            visualCtrl.test_InitModeling(equipPrefab, chaPrefab);
            transform.position = Vector3.zero;
            FindAnyObjectByType<PlayTimeManager>().InitTimer();
            renderCam.SetActive(true);
            StartCoroutine(CoroutineExtensions.DelayedActionCall(() => { ActivePlayer(); }, PlayProcessController.InitTimeSTATIC));
        }
        else
        {
            visualCtrl.InitModelingAsync(PlayProcessController.InitTimeSTATIC, () =>
            {
                transform.position = Vector3.zero;
                FindAnyObjectByType<PlayTimeManager>().InitTimer();
                renderCam.SetActive(true);
                StartCoroutine(CoroutineExtensions.DelayedActionCall(() => { ActivePlayer(); }, PlayProcessController.InitTimeSTATIC));
            });
        }
        auraController = FindAnyObjectByType<AuraController>();

        playerMover.FindAuraCtrl(auraController);
        auraController.gameObject.SetActive(false);
        PlaySystemRefStorage.playProcessController.SubscribeGameoverAction(stopPlay);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ItemController>(out var item)) 
        {
            item.UseItem();
        }
    }
    public void ActivePlayer()
    {
        auraController.gameObject.SetActive(true);
        playerMover.IsStop = false;
        GetComponent<Collider>().enabled = true;
    }
    public void DeactivePlayer()
    {
        auraController.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
    public void Hit(float stun, Vector3 hitPos)
    {
        stun = Mathf.Max(stun, 0.2f);
        if (isInvincible) { return; }
        else
        {
            hitPos.y = 0;
            stunCoroutine = StartCoroutine(StunCoroutine(stun, hitPos));
            PlaySystemRefStorage.harvestLvController.LossExp(stun);
        }
    }
    
    //임시로 만듦. 컴벳 시스템의 피격 메서드를 대체하고 있음.
    public void Hit(float stun, Vector3 hitPos, int damage)
    {
        //스턴 시간 계산
        stun = Mathf.Max(stun, 0.2f);
        
        if (isInvincible) { return; }
        
        //스턴 로직
        hitPos.y = 0;
        stunCoroutine = StartCoroutine(StunCoroutine(stun, hitPos));
        PlaySystemRefStorage.harvestLvController.LossExp(stun);
            
        //데미지 로직
        hp -= damage;
        Debug.Log($"Player가 {gameObject.name}에게 {damage}의 피해를 입었습니다!! [현재 체력: {hp}]");
        
        if (hp <= 0)
        {
            Debug.Log("Player가 사망했습니다!");
        }
    }
    private void stopPlay()
    {
        if (isInvincible) { StopCoroutine(stunCoroutine); }
        isInvincible = true;
        auraController.gameObject.SetActive(false);
        visualCtrl.TriggerDisappear();
        playerMover.IsStop = true;
        GetComponent<Collider>().enabled = false;
        StartCoroutine(CoroutineExtensions.DelayedActionCall(() => { DeactivePlayer(); }, 4f));
    }
    private IEnumerator StunCoroutine(float duration, Vector3 hitPos)
    {
        playerMover.IsStop = true;
        auraController.Shrink();
        isInvincible = true;
        visualCtrl.StartHitBlink(duration);
        visualCtrl.SetHitFX(hitPos, duration);
        yield return null;
        playerMover.StunPush(duration, hitPos);
        visualCtrl.SetMovingAnim(false);
        visualCtrl.SetStunAnim(true);
        yield return new WaitForSeconds(0.8f * duration);
        playerMover.IsStop = false;
        yield return new WaitForSeconds(0.2f * duration);
        auraController.Resume();
        visualCtrl.SetStunAnim(false);
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
        yield break;
    }
}
