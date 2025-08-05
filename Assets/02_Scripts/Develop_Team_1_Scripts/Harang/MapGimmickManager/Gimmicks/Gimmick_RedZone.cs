using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

public class Gimmick_RedZone : Gimmick
{
    [Header("레드존 설정")]
    
    //SerializeField
    // [field: SerializeField, LabelText("끌어당기는 힘"), Tooltip("블랙홀이 끌어당기는 정도"), Required, Space]
    // private float[] OuterSuctionGravityStrength { get; set; } = { 10.0f, 12.0f, 15.0f, 20.0f };
    //
    // [field: SerializeField, LabelText("안쪽 끌어당기는 힘"), Tooltip("블랙홀이 끌어당기는 정도"), Required, Space]
    // private float[] InnerSuctionGravityStrength { get; set; } = { 15.0f, 20.0f, 25.0f, 30.0f };
    
    public override eGimmickType eGimmickType => eGimmickType.Dangerous;
    
    [field: SerializeField, LabelText("레드존 크기"), Tooltip("레드존 피해 구역"), Required, Space]
    private float EffectiveRange { get; set; } = 6.7f;

    //private
    private IDamageAble mPlayerIDamageAble { get; set; }
    private float mbTimeElapsed { get; set; } = 0f;

    [SerializeField] private GameObject warningParticle;
    [SerializeField] private GameObject fireParticle;
    
    private float mWaitWarningDuration = 5.0f;
    
    private float[] mWarningTimes = new float[]
    {
        5.0f,
        4.0f,
        4.0f,
        3.0f,
    };
    
    public void Awake()
    {
        base.Awake();
        mWaitWarningDuration = mWarningTimes[(int)eGimmickType];
    }
    
    private void OnEnable()
    {
        if (GimmickManager.Instance != null)
        {
            if (GimmickManager.Instance.UnimoPrefab.TryGetComponent(out IDamageAble iDamageAble))
            {
                mPlayerIDamageAble = iDamageAble;
                mbTimeElapsed = 0f; //레드존 시간 초기화
                mDamageTimeElapsed = 0f;
            }
            else
            {
                MyDebug.Log("There is no UnimoPrefab attached to this object.");
            }
        }
        else
        {
            mPlayerIDamageAble = null;
            gameObject.SetActive(false);
        }
    }

    //cash
    private float mDistance;
    private float mDamageTimeElapsed = 0f;
    private float mDamageInterval = 0.5f; // 데미지 주기 Time.deltaTime


    private void Update()
    {
        base.Update();
        
        if (mPlayerIDamageAble == null || !mbFireParticleStart) return;
        
        //거리 비교
        mDistance = Vector3.Distance(transform.position, mPlayerIDamageAble.GameObject.transform.position);
        
        if(mDistance <= EffectiveRange) //레드존 내에 위치 
        {
            mDamageTimeElapsed += Time.deltaTime;
            
            if (mDamageTimeElapsed >= mDamageInterval) //1초마다
            {
                mDamageTimeElapsed = 0f;
                
                MyDebug.Log("Player In RedZone 1 Second");
            
                //데미지 발생시키기
                CombatEvent combatEvent = new CombatEvent
                {
                    Receiver = mPlayerIDamageAble,
                    Damage = (int)mGimmickEffectValue2,
                    HitPosition = gameObject.transform.position,
                };
                
                CombatSystem.Instance.AddInGameEvent(combatEvent);
            }
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, EffectiveRange);
    }

    public override async void ActivateGimmick()
    {
        mbReadyExecute = true;
        Vector2 randomPos = Random.insideUnitCircle * ( PlaySystemRefStorage.mapSetter.MaxRange - 2 );
        gameObject.transform.position = new Vector3(randomPos.x, 0, randomPos.y);
        
        gameObject.SetActive(true);
        await FadeAll(true);
        StartCoroutine(FireParticleWaitCoroutine());
    }

    public override async void DeactivateGimmick()
    {
        StopCoroutine(FireParticleWaitCoroutine());
        mbDeactivateStart = true;
        mbReadyExecute = false;
        mbFireParticleStart = false;
        await FadeAll(false);
        fireParticle.SetActive(false);
        gameObject.SetActive(false);
    }
    
    private bool mbFireParticleStart = false;
    
    public IEnumerator FireParticleWaitCoroutine()
    {
        warningParticle.SetActive(true);
        mbFireParticleStart = true;
        
        yield return new WaitForSeconds(mWaitWarningDuration); //경고 시간 동안 대기
        
        warningParticle.SetActive(false);
        fireParticle.SetActive(true);
    }
}