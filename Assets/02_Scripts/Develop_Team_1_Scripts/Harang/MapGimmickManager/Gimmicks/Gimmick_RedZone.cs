using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

public class Gimmick_RedZone : Gimmick
{
    [Header("������ ����")]
    
    //SerializeField
    // [field: SerializeField, LabelText("������� ��"), Tooltip("��Ȧ�� ������� ����"), Required, Space]
    // private float[] OuterSuctionGravityStrength { get; set; } = { 10.0f, 12.0f, 15.0f, 20.0f };
    //
    // [field: SerializeField, LabelText("���� ������� ��"), Tooltip("��Ȧ�� ������� ����"), Required, Space]
    // private float[] InnerSuctionGravityStrength { get; set; } = { 15.0f, 20.0f, 25.0f, 30.0f };
    
    public override eGimmickType eGimmickType => eGimmickType.Dangerous;
    
    [field: SerializeField, LabelText("������ ũ��"), Tooltip("������ ���� ����"), Required, Space]
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
                mbTimeElapsed = 0f; //������ �ð� �ʱ�ȭ
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
    private float mDamageInterval = 0.5f; // ������ �ֱ� Time.deltaTime


    private void Update()
    {
        base.Update();
        
        if (mPlayerIDamageAble == null || !mbFireParticleStart) return;
        
        //�Ÿ� ��
        mDistance = Vector3.Distance(transform.position, mPlayerIDamageAble.GameObject.transform.position);
        
        if(mDistance <= EffectiveRange) //������ ���� ��ġ 
        {
            mDamageTimeElapsed += Time.deltaTime;
            
            if (mDamageTimeElapsed >= mDamageInterval) //1�ʸ���
            {
                mDamageTimeElapsed = 0f;
                
                MyDebug.Log("Player In RedZone 1 Second");
            
                //������ �߻���Ű��
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
        
        yield return new WaitForSeconds(mWaitWarningDuration); //��� �ð� ���� ���
        
        warningParticle.SetActive(false);
        fireParticle.SetActive(true);
    }
}