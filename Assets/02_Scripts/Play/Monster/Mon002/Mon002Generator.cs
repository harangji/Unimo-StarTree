using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mon002Generator : MonsterGenerator
{
    [SerializeField] private GameObject focusingObj;
    private float genRadius = 35f;
    private float targetRadius = 3f;
    private Vector3 newIndicatorPos = Vector3.zero;
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
        MonsterController controller = base.generateEnemy();
        Transform indicator = controller.indicatorCtrl.GetIndicatorTransform();
        Vector3 indicatorPos = newIndicatorPos + new Vector3(0f, indicator.position.y, 0f);
        indicator.position = indicatorPos;
        indicator.parent = transform;

        return controller;
    }
    protected override Vector3 findGenPosition()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        float angle = 2f * Mathf.PI * Random.Range(0f, 1f);
        Vector3 newpos = playerTransform.position + genRadius * new Vector3(Mathf.Cos(angle),0f,Mathf.Sin(angle));
        return newpos;
    }
    protected override Quaternion setGenRotation(Vector3 genPos)
    {
        Random.InitState(System.DateTime.Now.Millisecond+10);
        float radius = targetRadius * (0.3f + 0.7f*Mathf.Pow(Random.Range(0f, 1f),0.7f));
        float angle = 2f * Mathf.PI * Random.Range(0f, 1f);
        newIndicatorPos = playerTransform.position + radius * new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
        Quaternion quat = Quaternion.LookRotation(newIndicatorPos - genPos, Vector3.up);
        return quat;
    }
    protected override IEnumerator startPatternCoroutine()
    {
        isExtreme = true;
        for (int i = 0; i < 8; i++)
        {
            generateEnemy();
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(1.5f);
        Vector3 center = playerTransform.position;
        focusingObj.transform.position = center + 0.5f * Vector3.up;
        for (int i = 0; i < 8; i++)
        {
            float angle = 1f / 4f * Mathf.PI * i;
            Vector3 pos = center + genRadius * new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
            newIndicatorPos = center + 6f * new Vector3(Mathf.Cos(angle + 1f / 8f * Mathf.PI), 0f, Mathf.Sin(angle + 1f / 8f * Mathf.PI));
            Quaternion quat = Quaternion.LookRotation(newIndicatorPos - pos, Vector3.up);
            MonsterController controller = Instantiate(monsterPrefab, pos, quat).GetComponent<MonsterController>();
            controller.InitEnemy(playerTransform);
            Transform indicator = controller.indicatorCtrl.GetIndicatorTransform();
            Vector3 indicatorPos = newIndicatorPos + new Vector3(0f, indicator.position.y, 0f);
            indicator.position = indicatorPos;
            indicator.parent = transform;
            AudioSource audio = controller.GetComponent<AudioSource>();
            audio.volume = 0.25f;
        }
        StartCoroutine(monGenIndicatorCoroutine(3.1f));
        yield return new WaitForSeconds(3f);
        isPaused = false;
        isExtreme = false;
        yield break;
    }
    protected override IEnumerator exPatternCoroutine()
    {
        isExtreme = true;
        Vector3 center = playerTransform.position;
        focusingObj.transform.localScale = 4f * Vector3.one;
        focusingObj.transform.position = center + 0.5f * Vector3.up;
        StartCoroutine(monGenIndicatorCoroutine(3.1f));
        for (int i = 0; i < 6; i++)
        {
            float angle = 1f / 3f * Mathf.PI * i;
            Vector3 pos = center + genRadius * new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
            newIndicatorPos = center + 4.5f * new Vector3(Mathf.Cos(angle + 1f / 6f * Mathf.PI), 0f, Mathf.Sin(angle + 1f / 6f * Mathf.PI));
            Quaternion quat = Quaternion.LookRotation(newIndicatorPos - pos, Vector3.up);
            MonsterController controller = Instantiate(monsterPrefab, pos, quat).GetComponent<MonsterController>();
            controller.InitEnemy(playerTransform);
            Transform indicator = controller.indicatorCtrl.GetIndicatorTransform();
            Vector3 indicatorPos = newIndicatorPos + new Vector3(0f, indicator.position.y, 0f);
            indicator.position = indicatorPos;
            indicator.parent = transform;
            AudioSource audio = controller.GetComponent<AudioSource>();
            audio.volume = 0.25f;
        }
        yield return new WaitForSeconds(2f);
        isPaused = false;
        isExtreme = false;
        yield break;
    }
    private IEnumerator monGenIndicatorCoroutine(float duration)
    {
        float lapse = 0;
        focusingObj.SetActive(true);
        Renderer indiRenderer = focusingObj.GetComponentInChildren<Renderer>();
        Material indiMat = indiRenderer.material;
        indiMat.SetFloat("_ColorChange", 0f);
        indiMat.SetFloat("_InnerScale", 0.1f);
        while (lapse < 0.87f *duration)
        {
            lapse += Time.deltaTime;
            float ratio = Mathf.Sin(0.5f * Mathf.PI * Mathf.Clamp01(lapse / (0.87f *duration)));
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
