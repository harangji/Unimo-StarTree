using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerGenerator_ST001 : FlowerGenerator
{
    private MapRangeSetter mapSetter;
    private readonly int startMaxFlowers = 8;
    private readonly List<float> refreshTimeperPhase = new List<float> { 1.05f, 0.84f, 0.67f, 0.54f, 0.45f };
    private readonly List<int> goalIncrements = new List<int> { 3, 5, 8, 15, 25 };
    private readonly List<int> phaseStandards = new List<int> { 4, 7, 12, 18 };
    private int maxFlowers = 6;
    private int maxFlowerBoost = 0;
    private int nextGoal = 3;
    private float refreshTime = 1f;

    new protected void Awake()
    {
        base.Awake();
        maxFlowers = startMaxFlowers;
        refreshTime = refreshTimeperPhase[0];
    }
    // Start is called before the first frame update
    new protected void Start()
    {
        base.Start();
        mapSetter = PlaySystemRefStorage.mapSetter;
        generateFlower();
        generateFlower();
    }
    override public void GatherFlower()
    {
        base .GatherFlower();
        checkMaxFlowerInc();
    }
    protected override void generateFlower()
    {
        if (AllFlowers.Count >= maxFlowers + maxFlowerBoost) { return; }
        base.generateFlower();
    }
    override protected Vector3 findPosition()
    {
        float rand = Random.Range(0.0f, 1.0f);
        float radius = mapSetter.MaxRange * Mathf.Pow(rand, 1.2f);
        rand = Random.Range(0.0f, 1.0f);
        float angle = 2* Mathf.PI * rand;
        Vector3 pos = new Vector3(radius * Mathf.Cos(angle), 0f, radius * Mathf.Sin(angle));
        if (mapSetter.IsInMap(pos) ==false)
        { pos = mapSetter.FindNearestPoint(pos); }

        return pos;
    }

    override protected IEnumerator generateCoroutine()
    {
        float value = 2.0f;
        if (Base_Manager.Data.UserData.BuffFloating[1] > 0.0f) value = 1.5f;
        yield return new WaitForSeconds(value);
        while(true)
        {
            maxFlowerBoost = (int)(0.2f * (1f - Mathf.Pow(PlaySystemRefStorage.playTimeManager.GetRemainTimeRatio(),0.5f)) * maxFlowers);
            generateFlower();
            float tweigth = 0.5f + 0.5f * AllFlowers.Count / maxFlowers;
            tweigth *= 0.5f + 0.5f * PlaySystemRefStorage.playTimeManager.GetRemainTimeRatio();
            yield return new WaitForSeconds(tweigth*refreshTime);
        }
    }
    private void checkMaxFlowerInc()
    {
        if (gatheredFlowers >= nextGoal)
        {
            ++maxFlowers;
            setNextPhaseProperties();
        }
    }
    private void setNextPhaseProperties()
    {
        if (maxFlowers - startMaxFlowers + 1 <= phaseStandards[0])
        {
            nextGoal += goalIncrements[0];
        }
        else if (maxFlowers - startMaxFlowers + 1 <= phaseStandards[1])
        {
            nextGoal += goalIncrements[1];
            refreshTime = refreshTimeperPhase[1];
        }
        else if (maxFlowers - startMaxFlowers + 1 <= phaseStandards[2])
        {
            nextGoal += goalIncrements[2];
            refreshTime = refreshTimeperPhase[2];
        }
        else if (maxFlowers - startMaxFlowers + 1 <= phaseStandards[3])
        {
            nextGoal += goalIncrements[3];
            refreshTime = refreshTimeperPhase[3];
        }
        else
        {
            nextGoal += goalIncrements[4] + nextGoal / 20;
            refreshTime = refreshTimeperPhase[4];
        }
    }
}
