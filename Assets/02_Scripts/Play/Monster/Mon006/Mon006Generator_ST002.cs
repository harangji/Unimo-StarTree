using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mon006Generator_ST002 : MonsterGenerator
{
    private float genWidth = 11.5f * 1.2f;
    private float genHeight = 13f;
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
    protected override Vector3 findGenPosition()
    {
        float rand = 0.9f * genWidth * Random.Range(-1f, 1f);
        Vector3 pos = new Vector3(rand, genHeight, 0f);

        return pos;
    }
    protected override Quaternion setGenRotation(Vector3 genPos)
    {
        return Quaternion.Euler(90f, 180f, 0f);
    }
    protected override IEnumerator startPatternCoroutine()
    {
        MonGeneratorManager manager = GetComponentInParent<MonGeneratorManager>();
        manager.PauseGenerateAllMonster();
        yield return new WaitForSeconds(4f);
        isExtreme = true;
        int empty = Random.Range(0, 5);
        for (int i = 0; i < 5; i++)
        {
            float xpos = 0.9f * genWidth * (-1f + 2f*(i/4f));
            if (i == empty) { continue; }
            Vector3 pos = new Vector3(xpos, genHeight, 0f);
            MonsterController controller = Instantiate(monsterPattern1, pos, Quaternion.Euler(90f, 180f, 0f)).GetComponent<MonsterController>();
            controller.InitEnemy(playerTransform);
        }
        yield return new WaitForSeconds(6f);
        manager.ResumeGenerateAllMonster();
        isPaused = true;
        yield return new WaitForSeconds(2f);
        isPaused = false;
        isExtreme = false;
        yield break;
    }
    protected override IEnumerator exPatternCoroutine()
    {
        MonGeneratorManager manager = GetComponentInParent<MonGeneratorManager>();
        manager.PauseGenerateAllMonster();
        yield return new WaitForSeconds(4f);
        isExtreme = true;
        for (int i = 0; i < 3; i++)
        {
            float xpos = 0.9f * genWidth * (-1f + i);
            Vector3 pos = new Vector3(xpos, genHeight, 0f);
            MonsterController controller = Instantiate(monsterPattern1, pos, Quaternion.Euler(90f, 180f, 0f)).GetComponent<MonsterController>();
            controller.InitEnemy(playerTransform);
        }
        yield return new WaitForSeconds(6f);
        manager.ResumeGenerateAllMonster();
        isPaused = true;
        yield return new WaitForSeconds(2f);
        isPaused = false;
        isExtreme = false;
        yield break;
    }

}
