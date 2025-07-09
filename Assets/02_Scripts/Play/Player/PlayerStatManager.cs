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
    // HP 매니저 추가.
    private HPManager hpManager;
    
    private bool isInvincible = false;
    private Coroutine stunCoroutine;

    [SerializeField] private bool isTestModel = false;
    [SerializeField] private GameObject equipPrefab;
    [SerializeField] private GameObject chaPrefab;

    // 임시로 작성.정현식
    [SerializeField] [Range(0f, 1f)] private float bEvadeChance = 0.5f;     // 50% 확률로 피격 무시
    [SerializeField] [Range(0f, 1f)] private float fStunReduceRate = 0.5f;   // 스턴 시간 50% 감소
    
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
        // HP 매니저 추가
        hpManager = GetComponent<HPManager>();
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
    
    // Hit 수정 버전. 정현식
    public void Hit(float stun, Vector3 hitPos)
    {
        if (isInvincible) return;

        //피격 회피 판정
        if (Random.value < bEvadeChance)
        {
            Debug.Log(" 피격 회피");
            return;
        }

        //스턴 시간 감소 적용
        stun = Mathf.Max(stun * (1f - fStunReduceRate), 0.2f);
        hitPos.y = 0;

        //스턴 연출 시작
        stunCoroutine = StartCoroutine(StunCoroutine(stun, hitPos));

        //경험치 손실
        PlaySystemRefStorage.harvestLvController.LossExp(stun);

        //데미지 고정 적용
        hpManager.TakeDamage(20f);
    }
    
   //원본 Hit 코드. 정현식
   //public void Hit(float stun, Vector3 hitPos)
   //{
   //    stun = Mathf.Max(stun, 0.2f);
   //    
   //    if (isInvincible) { return; }
   //    else
   //    {
   //        hitPos.y = 0;
   //        stunCoroutine = StartCoroutine(StunCoroutine(stun, hitPos));
   //        PlaySystemRefStorage.harvestLvController.LossExp(stun);
   //    }
   //}
    
    
    // 오라 작아짐 + 스턴 애니메이션 + 무적 판정 + 데미지 적용
    //임시로 만듦. 컴벳 시스템의 피격 메서드를 대체하고 있음.
    public void Hit(float stun, Vector3 hitPos, int damage)
    {
        if (isInvincible) { return; }
        
        //  피격 회피 판정
        if (Random.value < bEvadeChance)
        {
            Debug.Log("피격 회피");
            return;
        }
        
        //  스턴 시간 감소 적용
        stun = Mathf.Max(stun * (1f - fStunReduceRate), 0.2f);
        
        //스턴 로직
        hitPos.y = 0;
        
        stunCoroutine = StartCoroutine(StunCoroutine(stun, hitPos));
        
        PlaySystemRefStorage.harvestLvController.LossExp(stun);
            
        // HPManager를 통해 데미지 처리
        hpManager.TakeDamage(20f);
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
