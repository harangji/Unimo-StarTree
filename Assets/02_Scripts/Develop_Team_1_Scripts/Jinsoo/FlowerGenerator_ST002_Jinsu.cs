using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FlowerGenerator_ST002_Jinsu : FlowerGenerator
{
    private MapRangeSetter mapSetter;
    private float mRemain;
    private int mCycleCount;

    new protected void Awake()
    {
        base.Awake();
        mapSetter = PlaySystemRefStorage.mapSetter;
    }

    protected new void Start()
    {
        mRemain = PlaySystemRefStorage.playTimeManager.GetRemainTime();
        AllFlowers = new();
        StartCoroutine(generateCoroutine());
    }

    /// <summary>
    /// 10회 스폰을 한 싸이클로 묶어 관리.
    /// 0~30초: 1초 주기(STS01), 30~60초: 0.5초 주기(STS02).
    /// 한 싸이클 내 최소 1회는 주황별꿀(rare) 보장.
    /// </summary>
    override protected IEnumerator generateCoroutine()
    {
        var rareSpawned = false;

        while (true)
        {
            mRemain = PlaySystemRefStorage.playTimeManager.GetRemainTime();
            var isSTS02 = mRemain <= 30f;
            var period = (isSTS02 || mCycleCount >= 3) ? 0.5f : 1f;
            var rareCount = isSTS02 ? 2 : 1;

            for (var i = 0; i < 10; i++)
            {
                // 싸이클 마지막 기회에 주황별꿀 미스폰 시 강제 스폰
                if (i == 9 && !rareSpawned)
                {
                    SpawnOrange(rareCount);
                }
                else
                {
                    var r = Random.value;
                    if (!rareSpawned)
                    {
                        if (r < 0.25f) { /* 실패 */ }
                        else if (r < 0.85f) { SpawnYellow(); }
                        else
                        {
                            SpawnOrange(rareCount);
                            rareSpawned = true;
                        }
                    }
                    else
                    {
                        if (r >= 0.25f) SpawnYellow();
                    }
                }

                yield return new WaitForSeconds(period);
            }

            // 싸이클 초기화
            rareSpawned = false;
            mCycleCount++;
            Debug.Log($"싸이클 초기화 : {mRemain}, {mCycleCount}");
        }
    }
    
    protected override Vector3 findPosition()
    {
        float xpos = mapSetter.MaxRange * Random.Range(-1f, 1f);
        Vector3 pos = new Vector3 (xpos, 30f, 0);
        return pos;
    }

    /// <summary>
    /// 노랑별꿀(일반) 생성: 50% 확률로 1개 또는 2개.
    /// </summary>
    private void SpawnYellow()
    {
        Debug.Log("Spawn Yellow Flower");
        int count = Random.value < 0.5f ? 1 : 2;
        Debug.Log($"Yello : {count}");
        for (int j = 0; j < count; j++)
        {
            FlowerController flower = Instantiate(flowerObjs[0], findPosition(), setRotation()).
                GetComponent<FlowerController>();
            flower.InitFlower(this);
        }
    }

    /// <summary>
    /// 주황별꿀(희귀) 생성: STS01=1개, STS02=2개.
    /// </summary>
    private void SpawnOrange(int count)
    {
        Debug.Log("Spawn Orange Flower");
        for (int j = 0; j < count; j++)
        {
            FlowerController flower = Instantiate(flowerObjs[1], findPosition(), setRotation()).
                GetComponent<FlowerController>();
            flower.InitFlower(this);
        }
    }
}