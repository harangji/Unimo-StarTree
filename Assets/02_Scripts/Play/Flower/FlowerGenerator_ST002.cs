using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerGenerator_ST002 : FlowerGenerator
{
    private MapRangeSetter mapSetter;
    private float oriGenRate = 2f;
    private float rateDecayingStandard = 20;
    new protected void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame update
    new protected void Start()
    {
        base.Start();
        mapSetter = PlaySystemRefStorage.mapSetter;
    }
    override protected void generateFlower()
    {
        float rand = Random.Range(0f, 1f);
        int idx = 0;
        while (rand > appearAccProb[idx]) { ++idx; }
        FlowerController flower = Instantiate(flowerObjs[idx], findPosition(), setRotation()).
            GetComponent<FlowerController>();
        flower.InitFlower(this);
    }
    override protected Vector3 findPosition()
    {
        float xpos = mapSetter.MaxRange * Random.Range(-1f, 1f);
        Vector3 pos = new Vector3 (xpos, 30f, 0);
        return pos;
    }
    override protected IEnumerator generateCoroutine()
    {
        float value = 0.2f;
        if (Base_Manager.Data.UserData.BuffFloating[1] > 0.0f) value = 0.1f;
        yield return new WaitForSeconds(value);
        while (true)
        {
            int i = 0;
            while(checkAddGen(i++))
            {
                generateFlower();
                yield return new WaitForSeconds(Random.Range(0.1f, 0.25f));
            }
            yield return new WaitForSeconds(Random.Range(0.9f,1.1f) * calculateNextGen());
        }
    }
    private float calculateNextGen()
    {
        float ratio = gatheredFlowers / rateDecayingStandard;
        return oriGenRate * (0.35f + 1.65f/(1+ratio))/2f;
    }
    private bool checkAddGen(int serial)
    {
        float denom = gatheredFlowers / rateDecayingStandard + 1f;
        float rand = Random.Range(0f, 0.99f);
        return rand < Mathf.Exp(-(float)serial/denom);
    }
}
