using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// using ADBannerView = UnityEngine.iOS.ADBannerView;

public class Mon002Generator_C : MonsterGenerator
{
    [SerializeField] private GameObject focusingObj;
    private float genRadius = 35f;
    private float targetRadius = 3f;
    private Vector3 newIndicatorPos = Vector3.zero;

    private bool isPattern2Active = false;
    private bool isPattern3Active = false;

    private WaitForSeconds pattern3Wait = new WaitForSeconds(0.5f);

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
    // protected override MonsterController generateEnemy()
    // {
    //     MonsterController controller = base.generateEnemy();
    //     Transform indicator = controller.indicatorCtrl.GetIndicatorTransform();
    //     Vector3 indicatorPos = newIndicatorPos + new Vector3(0f, indicator.position.y, 0f);
    //     indicator.position = indicatorPos;
    //     indicator.parent = transform;
    //
    //     return controller;
    // }

    protected override MonsterController generateEnemy()
    {
        //todo 난이도 점유 기능 추가해야 함 -> 나중에 게임 매니저 만들어지면 하면 됨

        var rate = Random.Range(0, 100);

        if (rate < 70)
        {
            Debug.Log("패턴 1");

            StartPattern1();
        }
        else if (rate < 90)
        {
            Debug.Log("패턴 2");

            if (!isPattern2Active)
            {
                StartCoroutine(StartPattern2Coroutine());
            }
        }
        else
        {
            Debug.Log("패턴 3");

            if (!isPattern3Active)
            {
                StartCoroutine(StartPattern3Coroutine());
            }
        }

        return null;
    }

    protected override Vector3 findGenPosition()
    {
        Random.InitState(System.DateTime.Now.Millisecond);

        float angle = 2f * Mathf.PI * Random.Range(0f, 1f);
        Vector3 newPos = playerTransform.position + genRadius * new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));

        return newPos;
    }

    protected override Quaternion setGenRotation(Vector3 genPos)
    {
        // 시드를 바꾸지 않아도 됨 (랜덤 안 씀)
        newIndicatorPos = playerTransform.position; // 정확히 플레이어 위치를 겨냥

        // 플레이어 방향을 바라보는 회전
        Quaternion quat = Quaternion.LookRotation(newIndicatorPos - genPos, Vector3.up);
        return quat;
    }

    private void StartPattern1()
    {
        MonsterController controller = base.generateEnemy();
        Transform indicator = controller.indicatorCtrl.GetIndicatorTransform();
        Vector3 indicatorPos = newIndicatorPos + new Vector3(0f, indicator.position.y, 0f);
        indicator.position = indicatorPos;
        indicator.parent = transform;
    }

    private IEnumerator StartPattern2Coroutine()
    {
        // 중심 위치를 원점 기준 반지름 5 범위 내 랜덤 위치로 설정
        isPattern2Active = true;
        
        float radius = 5f;
        float angle = Random.Range(0f, 2f * Mathf.PI);
        Vector3 center = Vector3.zero + radius * new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));

        focusingObj.transform.localScale = 4f * Vector3.one;
        focusingObj.transform.position = center + 0.5f * Vector3.up;
        StartCoroutine(SafeZoneAnimationCoroutine(3.1f));

        for (int i = 0; i < 6; i++)
        {
            float angleStep = 1f / 3f * Mathf.PI * i;
            Vector3 pos = center + genRadius * new Vector3(Mathf.Cos(angleStep), 0f, Mathf.Sin(angleStep));

            newIndicatorPos = center + 4.5f * new Vector3(Mathf.Cos(angleStep + 1f / 6f * Mathf.PI), 0f, Mathf.Sin(angleStep + 1f / 6f * Mathf.PI));
            Quaternion quat = Quaternion.LookRotation(newIndicatorPos - pos, Vector3.up);

            MonsterController controller = Instantiate(monsterPattern1, pos, quat).GetComponent<MonsterController>();
            controller.InitEnemy(playerTransform);

            Transform indicator = controller.indicatorCtrl.GetIndicatorTransform();
            Vector3 indicatorPos = newIndicatorPos + new Vector3(0f, indicator.position.y, 0f);
            indicator.position = indicatorPos;
            indicator.parent = transform;

            AudioSource audio = controller.GetComponent<AudioSource>();
            audio.volume = 0.25f;
        }

        yield return new WaitForSeconds(3.5f);

        isPattern2Active = false;
    }

    private IEnumerator StartPattern3Coroutine()
    {
        isPattern3Active = true;

        for (int i = 0; i < 12; i++)
        {
            StartPattern1();

            yield return pattern3Wait;
        }

        yield return new WaitForSeconds(1.5f);

        isPattern3Active = false;
    }

    private IEnumerator SafeZoneAnimationCoroutine(float duration)
    {
        float lapse = 0;
        focusingObj.SetActive(true);
        Renderer indiRenderer = focusingObj.GetComponentInChildren<Renderer>();
        Material indiMat = indiRenderer.material;
        indiMat.SetFloat("_ColorChange", 0f);
        indiMat.SetFloat("_InnerScale", 0.1f);
        while (lapse < 0.87f * duration)
        {
            lapse += Time.deltaTime;
            float ratio = Mathf.Sin(0.5f * Mathf.PI * Mathf.Clamp01(lapse / (0.87f * duration)));
            indiMat.SetFloat("_ColorChange", ratio);
            float scale = (1f - ratio) * 0.1f + ratio;
            indiMat.SetFloat("_InnerScale", scale);
            yield return null;
        }

        lapse = 0;
        while (lapse < 0.13f * duration)
        {
            lapse += Time.deltaTime;
            float ratio = Mathf.Cos(0.5f * Mathf.PI * Mathf.Clamp01(lapse / (0.13f * duration)));
            indiMat.SetFloat("_ColorChange", ratio);
            float scale = (1f - ratio) * 0.1f + ratio;
            indiMat.SetFloat("_InnerScale", scale);
            yield return null;
        }

        indiMat.SetFloat("_ColorChange", 1f);
        indiMat.SetFloat("_InnerScale", 1f);
        focusingObj.SetActive(false);
        yield break;
    }
}