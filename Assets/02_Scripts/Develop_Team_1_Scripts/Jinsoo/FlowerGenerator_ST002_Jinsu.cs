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
    /// 10ȸ ������ �� ����Ŭ�� ���� ����.
    /// 0~30��: 1�� �ֱ�(STS01), 30~60��: 0.5�� �ֱ�(STS02).
    /// �� ����Ŭ �� �ּ� 1ȸ�� ��Ȳ����(rare) ����.
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
                // ����Ŭ ������ ��ȸ�� ��Ȳ���� �̽��� �� ���� ����
                if (i == 9 && !rareSpawned)
                {
                    SpawnOrange(rareCount);
                }
                else
                {
                    var r = Random.value;
                    if (!rareSpawned)
                    {
                        if (r < 0.25f) { /* ���� */ }
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

            // ����Ŭ �ʱ�ȭ
            rareSpawned = false;
            mCycleCount++;
            Debug.Log($"����Ŭ �ʱ�ȭ : {mRemain}, {mCycleCount}");
        }
    }
    
    protected override Vector3 findPosition()
    {
        float xpos = mapSetter.MaxRange * Random.Range(-1f, 1f);
        Vector3 pos = new Vector3 (xpos, 30f, 0);
        return pos;
    }

    /// <summary>
    /// �������(�Ϲ�) ����: 50% Ȯ���� 1�� �Ǵ� 2��.
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
    /// ��Ȳ����(���) ����: STS01=1��, STS02=2��.
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