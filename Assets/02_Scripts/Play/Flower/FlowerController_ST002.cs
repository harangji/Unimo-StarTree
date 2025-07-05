using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerController_ST002 : FlowerController
{
    static private Transform playerTransform;
    [SerializeField] private float fallingSpeed = 3f;
    [SerializeField] private float maxPullingSpeed = 3f;
    [SerializeField] private float pullingRatio = 1.2f;
    [SerializeField] private List<GameObject> visualObjs = new();
    private float toPlayerSpeed = 0f;
    private Vector3 fallVector;
    private float timeAfterAffection = -1f;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        fallVector = new Vector3(0f, -fallingSpeed, 0f);
        if (playerTransform == null) { playerTransform = PlaySystemRefStorage.playerStatManager.transform; }
    }

    // Update is called once per frame
    void Update()
    {
        if (timeAfterAffection < 0f)
        {
            toPlayerSpeed = 0.5f * toPlayerSpeed;
            if (toPlayerSpeed < 0.1f) { toPlayerSpeed = 0f; }
        }
        else
        {
            timeAfterAffection -= Time.deltaTime;
        }
        Vector3 toplayerVec = playerTransform.position - transform.position + 1f * Vector3.up;
        toplayerVec.Normalize();
        if (toplayerVec.magnitude < 2f)
        {
            float multi = (1.5f / (1 + toplayerVec.magnitude) + 0.5f);
            toplayerVec *= multi;
        }
        Vector3 moveVelVec = fallVector + toPlayerSpeed * toplayerVec;
        if (toPlayerSpeed > 0.01f)
        {
            float yvlimit = 2f * (1.5f - transform.position.y);
            if (yvlimit > 0f)
            {
                if (moveVelVec.y > -yvlimit) { moveVelVec.y = -yvlimit; }
            }
        }
        transform.position += Time.deltaTime * moveVelVec;
        if (transform.position.y < -1.5f) 
        {
            flowerGenerator.AllFlowers.Remove(this);
            Destroy(gameObject); 
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            completeBloom();
        }
    }
    override public void AuraAffectFlower(float affection)
    {
        timeAfterAffection = 0.25f;
        toPlayerSpeed += affection * pullingRatio;
        if (toPlayerSpeed > maxPullingSpeed) { toPlayerSpeed = maxPullingSpeed; }
    }
    override protected void DeactivateFlower()
    {
        base.DeactivateFlower();
        for (int i = 0; i < visualObjs.Count; i++)
        {
            visualObjs[i].SetActive(false);
        }
    }
}
