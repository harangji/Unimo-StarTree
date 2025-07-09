using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mon002Generator_ST002 : MonsterGenerator
{
    private float genWidth = 11.5f * 1.2f;
    private Vector3 newIndicatorPos = Vector3.zero;
    // Start is called before the first frame update
    new void OnEnable()
    {
        genWidth = PlaySystemRefStorage.mapSetter.MaxRange * 1.2f;
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
        Transform indicator = controller.indicatorCtrl.GetIndicatorTransform();
        Vector3 indicatorPos = newIndicatorPos + new Vector3(0f, 0f, -1f);
        indicator.position = indicatorPos;
        indicator.parent = transform;

        return controller;
    }
    protected override Vector3 findGenPosition()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        float rand = genWidth * Random.Range(-1f, 1f);
        Vector3 pos = new Vector3(rand, 30f, 1f);
        return pos;
    }
    protected override Quaternion setGenRotation(Vector3 genPos)
    {
        Random.InitState(System.DateTime.Now.Millisecond + 10);
        float xdiff = 1.5f * Random.Range(-0.5f, 0.5f);
        Vector3 target = playerTransform.position + new Vector3(xdiff,0,0);
        target.z = genPos.z;
        newIndicatorPos = target;
        Quaternion quat = Quaternion.LookRotation(target - genPos, Vector3.back);
        return quat;
    }
    protected override IEnumerator startPatternCoroutine()
    {
        MonGeneratorManager manager = GetComponentInParent<MonGeneratorManager>();
        manager.PauseGenerateAllMonster();
        yield return new WaitForSeconds(3f);
        isExtreme = true;
        int lfirst = 1 - 2 * Random.Range(0, 2);
        WaitForSeconds wait = new WaitForSeconds(0.15f);
        for (int i = 0; i < 9; i++)
        {
            float xpos = (float)lfirst * genWidth * (float)(i - 4) / 4f;
            Vector3 pos = new Vector3(xpos, 30f, 1f);
            newIndicatorPos = playerTransform.position;
            newIndicatorPos.z = pos.z;
            Quaternion quat = Quaternion.LookRotation(newIndicatorPos - pos, Vector3.back);
            MonsterController controller = Instantiate(monsterPattern1, pos, quat).GetComponent<MonsterController>();
            controller.InitEnemy(playerTransform);
            Transform indicator = controller.indicatorCtrl.GetIndicatorTransform();
            Vector3 indicatorPos = newIndicatorPos + new Vector3(0f, 0f, -1f);
            indicator.position = indicatorPos;
            indicator.parent = transform;
            yield return wait;
        }
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 9; i++)
        {
            float xpos = -(float)lfirst * genWidth * (float)(i - 4) / 4f;
            Vector3 pos = new Vector3(xpos, 30f, 1f);
            newIndicatorPos = playerTransform.position;
            newIndicatorPos.z = pos.z;
            Quaternion quat = Quaternion.LookRotation(newIndicatorPos - pos, Vector3.back);
            MonsterController controller = Instantiate(monsterPattern1, pos, quat).GetComponent<MonsterController>();
            controller.InitEnemy(playerTransform);
            Transform indicator = controller.indicatorCtrl.GetIndicatorTransform();
            Vector3 indicatorPos = newIndicatorPos + new Vector3(0f, 0f, -1f);
            indicator.position = indicatorPos;
            indicator.parent = transform;
            yield return wait;
        }
        manager.ResumeGenerateAllMonster();
        isPaused = true;
        yield return new WaitForSeconds(2.5f);
        isPaused = false;
        isExtreme = false;
        yield break;
    }
    protected override IEnumerator exPatternCoroutine()
    {
        MonGeneratorManager manager = GetComponentInParent<MonGeneratorManager>();
        manager.PauseGenerateAllMonster();
        yield return new WaitForSeconds(2f);
        isExtreme = true;
        int lfirst = 1 - 2 * Random.Range(0, 2);
        WaitForSeconds wait = new WaitForSeconds(0.1f);
        for (int i = 0; i < 5; i++)
        {
            float xpos = (float)lfirst * genWidth * (float)(i - 2) / 2f;
            Vector3 pos = new Vector3(xpos, 30f, 1f);
            newIndicatorPos = playerTransform.position;
            newIndicatorPos.z = pos.z;
            Quaternion quat = Quaternion.LookRotation(newIndicatorPos - pos, Vector3.back);
            MonsterController controller = Instantiate(monsterPattern1, pos, quat).GetComponent<MonsterController>();
            controller.InitEnemy(playerTransform);
            Transform indicator = controller.indicatorCtrl.GetIndicatorTransform();
            Vector3 indicatorPos = newIndicatorPos + new Vector3(0f, 0f, -1f);
            indicator.position = indicatorPos;
            indicator.parent = transform;
            yield return wait;
        }
        manager.ResumeGenerateAllMonster();
        isPaused = true;
        yield return new WaitForSeconds(2f);
        isPaused = false;
        isExtreme = false;
        yield break;
    }
}
