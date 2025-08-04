using UnityEngine;
using Random = UnityEngine.Random;

public class Gimmick_UniqueFlower : Gimmick
{
    [Header("Ư���� �� ����")]
    
    public override eGimmickType eGimmickType => eGimmickType.Helpful;
    
    [SerializeField] private FlowerGenerator mFlowerGenerator;
    [SerializeField] private FlowerController_Unique mUniqueController;
    
    private void OnEnable()
    {
        mbTimeElapsed = 0f; //�ð� �ʱ�ȭ
    }
    
    private void Start()
    {
        mUniqueController.InitFlower(mFlowerGenerator, emGimmickGrade);
    }

    private void Update()
    {
        base.Update();
    }
    
    public override async void ActivateGimmick()
    {
        Vector2 randomPos = Random.insideUnitCircle * ( PlaySystemRefStorage.mapSetter.MaxRange - 2 );
        gameObject.transform.position = new Vector3(randomPos.x, 0, randomPos.y);
        
        gameObject.SetActive(true);
        await FadeAll(true);

        mUniqueController.InitFlower(mFlowerGenerator); //�� ����
        mUniqueController.bCanGlow = true;
        mbReadyExecute = true;
    }

    public override async void DeactivateGimmick()
    {
        mbReadyExecute = false;
        mbDeactivateStart = true;
        mUniqueController.bCanGlow = false;
        
        await FadeAll(false);

        gameObject.SetActive(false);
    }
}