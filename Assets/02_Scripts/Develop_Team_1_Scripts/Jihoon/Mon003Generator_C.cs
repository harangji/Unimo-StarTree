using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mon003Generator_C : MonsterGenerator
{
    private float innerRadius = 2.5f;
    private float outerRadius = 8f;

    // Start is called before the first frame update
    new void OnEnable()
    {
        base.OnEnable();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }
    
    protected override MonsterController generateEnemy()
    {
        // Vector3 pos = findGenPosition();
        // Quaternion quat = setGenRotation(pos);
        // MonsterController controller = Instantiate(monsterPattern1, pos, quat).GetComponent<MonsterController>();
        // controller.InitEnemy(playerTransform);
        //
        // return controller;
        
        var rate = Random.Range(70, 90);

        if (rate < 70)
        {
            Debug.Log("패턴 1");

            base.generateEnemy();
        }
        else if (rate < 90)
        {
            Debug.Log("패턴 2");

            StartPattern2();
        }
        else
        {
            Debug.Log("패턴 3");
        }

        return null;
    }
    
    protected override Vector3 findGenPosition()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        float rand = Random.Range(0f, 1f);
        rand *= rand;
        float radius = innerRadius + rand * (outerRadius - innerRadius);
        float angle = 2f * Mathf.PI * Random.Range(0f, 1f);

        Vector3 pos = new Vector3(radius * Mathf.Cos(angle), 0f, radius * Mathf.Sin(angle));
        pos += playerTransform.position;
        if (PlaySystemRefStorage.mapSetter.IsInMap(pos) == false)
        {
            pos = PlaySystemRefStorage.mapSetter.FindNearestPoint(pos);
        }
        return pos;
    }
    protected override Quaternion setGenRotation(Vector3 genPos)
    {
        float rand = Random.Range(-20f, 20f);
        Quaternion quat = Quaternion.Euler(0f, 180f + rand, 0f);
        return quat;
    }
    
    //소환 시작되면 나오는 패턴
    protected override IEnumerator startPatternCoroutine()
    {
        isExtreme = true;
        float angle;
        Vector3 center = playerTransform.position;
        if (center.magnitude > PlaySystemRefStorage.mapSetter.MaxRange-6)
        {
            center *= (PlaySystemRefStorage.mapSetter.MaxRange - 6) / center.magnitude;
        }
        for (int i = 0; i < 3; i++)
        {
            angle = 2f/3f*Mathf.PI * i;
            Vector3 pos = center + 5.5f * new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
            MonsterController controller = Instantiate(monsterPattern1, pos, Quaternion.identity).GetComponent<MonsterController>();
            controller.InitEnemy(playerTransform);
        }
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i < 3; i++)
        {
            angle = 2f / 3f * Mathf.PI * i + 1f / 3f * Mathf.PI;
            Vector3 pos = center + 5.5f * new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
            MonsterController controller = Instantiate(monsterPattern1, pos, Quaternion.identity).GetComponent<MonsterController>();
            controller.InitEnemy(playerTransform);
        }
        yield return new WaitForSeconds(4f);
        isPaused = false;
        isExtreme = false;
        yield break;
    }

    private void StartPattern2()
    {
        float angle;
        Vector3 center = playerTransform.position;
        if (center.magnitude > PlaySystemRefStorage.mapSetter.MaxRange - 6f)
        {
            center *= (PlaySystemRefStorage.mapSetter.MaxRange - 6f) / center.magnitude;
        }
        for (int i = 0; i < 5; i++)
        {
            angle = 0.4f * Mathf.PI * i;
            Vector3 pos = center + 4.8f * new Vector3(Mathf.Sin(angle), 0f, Mathf.Cos(angle));
            MonsterController controller = Instantiate(monsterPattern1, pos, Quaternion.identity).GetComponent<MonsterController>();
            controller.InitEnemy(playerTransform);
        }
    }
}

