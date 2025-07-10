using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mon001Generator_C : MonsterGenerator
{
    private float genRadius = 20f; //3 times of wanted peak radius
    private float randDirAngle = 40f;

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

    protected override Vector3 findGenPosition()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        float rand = Random.Range(0f, 1f);
        float radius = (rand > 0.995f) ? genRadius : 0.4f * genRadius * Mathf.Sqrt(-Mathf.Log(1 - rand)) + 1.5f;
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
        Quaternion quat = Quaternion.LookRotation(playerTransform.position - genPos, Vector3.up);
        float randAngle = (Random.Range(0, 2) * 2 - 1) * Mathf.Pow(Random.Range(0f, 1f), 2f);
        randAngle *= randDirAngle;
        if (randAngle < 0f)
        {
            randAngle += 360f;
        }

        quat *= Quaternion.Euler(0f, randAngle, 0f);
        return quat;
    }

    protected override MonsterController generateEnemy()
    {
        //todo 난이도 점유 기능 추가해야 함 -> 나중에 게임 매니저 만들어지면 하면 됨

        var rate = Random.Range(90, 100);

        if (rate < 70)
        {
            Debug.Log("패턴 1");

            base.generateEnemy();
        }
        else if (rate < 90)
        {
            Debug.Log("패턴 2");

            float angle;
            Vector3 center = playerTransform.position;
            
            if (center.magnitude > PlaySystemRefStorage.mapSetter.MaxRange - 7.2f)
            {
                center *= (PlaySystemRefStorage.mapSetter.MaxRange - 7.2f) / center.magnitude;
            }

            for (int i = 0; i < 8; i++)
            {
                angle = 1f / 4f * Mathf.PI * i;
                Vector3 pos = center + 6.5f * new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
                Quaternion quat = Quaternion.LookRotation(center - pos, Vector3.up);
                MonsterController controller = Instantiate(monsterPattern1, pos, quat).GetComponent<MonsterController>();
                controller.InitEnemy(playerTransform);
            }
        }
        else
        {
            Debug.Log("패턴 3");

            float angle;
            Vector3 center = playerTransform.position;
            
            if (center.magnitude > PlaySystemRefStorage.mapSetter.MaxRange - 7.2f)
            {
                center *= (PlaySystemRefStorage.mapSetter.MaxRange - 7.2f) / center.magnitude;
            }

            for (int i = 0; i < 8; i++)
            {
                angle = 1f / 4f * Mathf.PI * i;
                Vector3 pos = center + 7.5f * new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
                Quaternion quat = Quaternion.LookRotation(center - pos, Vector3.up);
                MonsterController controller = Instantiate(monsterPattern3, pos, quat).GetComponent<MonsterController>();
                controller.InitEnemy(playerTransform);
            }
        }

        return null;
    }

    // protected override IEnumerator exPatternCoroutine()
    // {
    //     isExtreme = true;
    //     yield return new WaitForSeconds(2f);
    //     float angle;
    //     Vector3 center = playerTransform.position;
    //     if (center.magnitude > PlaySystemRefStorage.mapSetter.MaxRange - 7.2f)
    //     {
    //         center *= (PlaySystemRefStorage.mapSetter.MaxRange - 7.2f) / center.magnitude;
    //     }
    //
    //     for (int i = 0; i < 8; i++)
    //     {
    //         angle = 1f / 4f * Mathf.PI * i;
    //         Vector3 pos = center + 6.5f * new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
    //         Quaternion quat = Quaternion.LookRotation(center - pos, Vector3.up);
    //         MonsterController controller = Instantiate(monsterPattern1, pos, quat).GetComponent<MonsterController>();
    //         controller.InitEnemy(playerTransform);
    //     }
    //
    //     yield return new WaitForSeconds(2f);
    //     isPaused = false;
    //     isExtreme = false;
    //     yield break;
    // }
}