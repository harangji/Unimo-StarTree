using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerGenerator : MonoBehaviour
{
    [HideInInspector] public List<FlowerController> AllFlowers;
    [SerializeField] protected List<GameObject> flowerObjs;
    [SerializeField] protected List<float> appearRatios;
    protected List<float> appearAccProb;
    protected int gatheredFlowers = 0;

    protected void Awake()
    {
        if (flowerObjs.Count != appearRatios.Count) 
        { 
            Debug.Log("Wrong set flower generator.");
            return;
        }
        float appearAcc = 0f;
        appearAccProb = new List<float>();
        foreach (float ratio in appearRatios)
        {
            appearAcc += ratio;
        }
        float prob = 0f;
        foreach (float ratio in appearRatios)
        {
            prob += ratio / appearAcc;
            appearAccProb.Add(prob);
        }
        appearAccProb[^1] = 1.4f;
    }
    // Start is called before the first frame update
    protected void Start()
    {
        AllFlowers = new();
        StartCoroutine(generateCoroutine());
    }
    
    public virtual void GatherFlower()
    {
        ++gatheredFlowers;
    }
    
    protected virtual void generateFlower()
    {
        float rand = Random.Range(0f, 1f);
        int idx = 0;
        while (rand > appearAccProb[idx]) { ++idx; }
        FlowerController flower = Instantiate(flowerObjs[idx], findPosition(), setRotation()).
            GetComponent<FlowerController>();
        flower.InitFlower(this);
    }
    protected virtual Vector3 findPosition()
    {
        return Vector3.zero;
    }
    protected virtual Quaternion setRotation()
    {
        return Quaternion.identity;
    }
    protected virtual IEnumerator generateCoroutine()
    {
        yield break;
    }
}
