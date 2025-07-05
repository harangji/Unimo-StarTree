using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mon005Generator_ST002 : MonsterGenerator
{
    private float genWidth = 11.5f * 1.2f;
    private float randAngle = 15f;
    private int randSeed = 0;
    // Start is called before the first frame update
    new void OnEnable()
    {
        genWidth = PlaySystemRefStorage.mapSetter.MaxRange;
        base.OnEnable();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }
    protected override MonsterController generateEnemy()
    {
        MonsterController controller = base.generateEnemy();
        int i = 1;
        while (checkGeneration(i++))
        {
            controller = base.generateEnemy();
            controller.transform.position += Random.Range(0.7f, 1.8f) * i * Vector3.up;
        }
        
        return controller;
    }
    protected override Vector3 findGenPosition()
    {
        Random.InitState(System.DateTime.Now.Millisecond + randSeed);
        float rand = genWidth * Random.Range(-1f, 1f);
        Vector3 pos = new Vector3(rand, 30f, 0f);
        randSeed++;
        randSeed %= 2048;
        return pos;
    }
    protected override Quaternion setGenRotation(Vector3 genPos)
    {
        return Quaternion.Euler(0, 180f, Random.Range(-randAngle, randAngle));
    }
    protected override IEnumerator exPatternCoroutine()
    {
        MonGeneratorManager manager = GetComponentInParent<MonGeneratorManager>();
        manager.PauseGenerateAllMonster();
        yield return new WaitForSeconds(1.5f);
        isExtreme = true;
        for (int i = 0; i < 3; i++)
        {
            float offset = -1f + 0.4f * (i % 2);
            for (int j = 0; j < 3; j++)
            {
                float xpos = offset + 0.8f * j;
                xpos *= genWidth;
                Vector3 pos = new Vector3(xpos, 30f, 0f);
                MonsterController controller = Instantiate(monsterPrefab, pos, 
                    Quaternion.Euler(0, 180f, Random.Range(-randAngle, randAngle))).GetComponent<MonsterController>();
                controller.transform.localScale = (0.9f + 0.3f * i) * Vector3.one;
                controller.InitEnemy(playerTransform);
            }
            yield return new WaitForSeconds(0.9f + 0.2f * i);
        }
        yield return new WaitForSeconds(1f);
        Vector3 Bigpos = new Vector3(genWidth * Random.Range(-0.5f, 0.5f), 30f, 0f);
        MonsterController Bigcontroller = Instantiate(monsterPrefab, Bigpos,
                    Quaternion.Euler(0, 180f, Random.Range(-randAngle, randAngle))).GetComponent<MonsterController>();
        Bigcontroller.transform.localScale = 5f * Vector3.one;
        Bigcontroller.InitEnemy(playerTransform);
        yield return new WaitForSeconds(4f);
        manager.ResumeGenerateAllMonster();
        isPaused = true;
        yield return new WaitForSeconds(2f);
        isPaused = false;
        isExtreme = false;
        yield break;
    }
    private bool checkGeneration(int idx)
    {
        if (idx >= 4) { return false; }
        float denom = monGenCalculator.GetGenTime()/30f + 0.1f;
        if(denom > 3.5f) denom = 3.5f;
        float rand = Random.Range(0f, 0.99f);
        return rand < Mathf.Exp(-(float)idx/denom);
    }
}
