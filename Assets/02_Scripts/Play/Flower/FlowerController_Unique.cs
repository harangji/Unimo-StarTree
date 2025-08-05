using UnityEngine;

public class FlowerController_Unique : FlowerController
{
    private float[] maxGrowths =
    {
        18.0f,
        24.0f,
        30.0f,
        36.0f
    };
    private float maxGrowth = 18.0f;
    private float currentGrowth;
    private bool isGrowing;
    private float timeAfterActive = 0.08f;
    private int emGimmickGrade;

    private readonly IBlessingEffect[] mBlessings = new IBlessingEffect[]
    {
        new Blessing_MoveSpeed(),
        new Blessing_Armor(),
        new Blessing_ResourceGain(),
        new Blessing_AuraBoost(),
    };

    [SerializeField] private GameObject[] mBlessingsIcon;
    
    [SerializeField] private PlayerStatManager mUnimoData;
    
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeAfterActive > 0f)
        {
            timeAfterActive -= Time.deltaTime;
            if (timeAfterActive <= 0f) { isGrowing = false; visual.SetFlowerActivate(isGrowing); }
        }
    }

    public void InitFlower(FlowerGenerator generator, eGimmickGrade grade)
    {
        flowerGenerator = generator;
        if (flowerGenerator != null) { flowerGenerator.AllFlowers.Add(this);
            PlaySystemRefStorage.stageManager.DeleteText();
        }
        emGimmickGrade = (int)grade;
        maxGrowth = maxGrowths[(int)emGimmickGrade];
        
        StartCoroutine(CoroutineExtensions.DelayedActionCall(ActivateFlower, 0.5f));
    }

    public bool bCanGlow { get; set; } = true;
    
    public override void AuraAffectFlower(float affection)
    {
        if(!bCanGlow) { return; }
        
        if (!isGrowing) { isGrowing = true; visual.SetFlowerActivate(isGrowing); }
        timeAfterActive = 0.15f;
        currentGrowth += affection;
        
        (visual as FlowerFXController_ST001)?.AffectFlowerFX(currentGrowth / maxGrowth);
        
        if (currentGrowth >= maxGrowth)
        {
            int r = Random.Range(0, mBlessings.Length);
            mBlessings[r].Apply(mUnimoData, emGimmickGrade);
            mBlessingsIcon[r].SetActive(true); //아이콘 표시
            
            completeBloom();
        }
    }
    
    protected virtual void DeactivateFlower()
    {
        GetComponent<Collider>().enabled = false;
        gameObject.SetActive(false);
    }
}
