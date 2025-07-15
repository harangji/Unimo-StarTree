using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// using ADBannerView = UnityEngine.iOS.ADBannerView;

public class Mon002Generator_C : MonsterGenerator
{
    private class PatternGroup { public int Remaining; public int Cost; }
    private List<PatternGroup> _activeGroups = new List<PatternGroup>();
    
    [SerializeField] private GameObject focusingObj;
    private float genRadius = 35f;
    private float targetRadius = 3f;
    private Vector3 newIndicatorPos = Vector3.zero;

    private bool isPattern2Active = false;
    private bool isPattern3Active = false;

    private WaitForSeconds pattern3Wait = new WaitForSeconds(0.5f);

    // Start is called before the first frame update
    new void OnEnable() { base.OnEnable(); }

    // Update is called once per frame
    new void Update() { base.Update(); }
    
    protected override MonsterController generateEnemy()
    {
        //todo ���̵� ���� ��� �߰��ؾ� �� -> ���߿� ���� �Ŵ��� ��������� �ϸ� ��
        int cost;
        var rate = Random.Range(0, 100);
        if (rate < 70) cost = 3;
        else if (rate < 90) cost = 5;
        else cost = 7;

        var stageMgr = PlaySystemRefStorage.stageManager;
        if (stageMgr == null || !stageMgr.TryConsumeDifficulty(cost))
        {
            Debug.Log("�Ҵ��� ���̵� ��ġ�� �����մϴ�."); return null;
        }
        
        int spawnCount = rate < 70 ? 1 : (rate < 90 ? 6 : 12);
        var group = new PatternGroup { Remaining = spawnCount, Cost = cost };
        _activeGroups.Add(group);
        
        if (rate < 70)
        {
            Debug.Log("���� 1");
            var ctrl = StartPattern1();
            RegisterDestroyCallback(ctrl, group);
        }
        else if (rate < 90)
        {
            Debug.Log("���� 2");
            if (isPattern2Active) return null;
            StartCoroutine(StartPattern2Coroutine(group));
        }
        else
        {
            Debug.Log("���� 3");
            if (isPattern3Active) return null;
            StartCoroutine(StartPattern3Coroutine(group));
        }

        return null;
    }
    
    private void RegisterDestroyCallback(MonsterController ctrl, PatternGroup group)
    {
        ctrl.OnDestroyed += (monster) =>
        {
            group.Remaining--;
            if (group.Remaining == 0)
            {
                PlaySystemRefStorage.stageManager.RestoreDifficulty(group.Cost);
                _activeGroups.Remove(group);
                // isPattern2Active = false;
                // isPattern3Active = false;
            }
        };
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
        // �õ带 �ٲ��� �ʾƵ� �� (���� �� ��)
        newIndicatorPos = playerTransform.position; // ��Ȯ�� �÷��̾� ��ġ�� �ܳ�

        // �÷��̾� ������ �ٶ󺸴� ȸ��
        Quaternion quat = Quaternion.LookRotation(newIndicatorPos - genPos, Vector3.up);
        return quat;
    }
    
    private MonsterController StartPattern1()
    {
        var controller = base.generateEnemy();
        controller.InitEnemy(playerTransform);
        Transform indicator = controller.indicatorCtrl.GetIndicatorTransform();
        Vector3 indicatorPos = newIndicatorPos + new Vector3(0f, indicator.position.y, 0f);
        indicator.position = indicatorPos;
        indicator.parent = transform;
        return controller;
    }

    // private void StartPattern1()
    // {
    //     MonsterController controller = base.generateEnemy();
    //     Transform indicator = controller.indicatorCtrl.GetIndicatorTransform();
    //     Vector3 indicatorPos = newIndicatorPos + new Vector3(0f, indicator.position.y, 0f);
    //     indicator.position = indicatorPos;
    //     indicator.parent = transform;
    // }

    private IEnumerator StartPattern2Coroutine(PatternGroup group)
    {
        // �߽� ��ġ�� ���� ���� ������ 5 ���� �� ���� ��ġ�� ����
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

            var controller = Instantiate(monsterPattern1, pos, quat).GetComponent<MonsterController>();
            controller.InitEnemy(playerTransform);

            Transform indicator = controller.indicatorCtrl.GetIndicatorTransform();
            Vector3 indicatorPos = newIndicatorPos + new Vector3(0f, indicator.position.y, 0f);
            indicator.position = indicatorPos;
            indicator.parent = transform;

            controller.GetComponent<AudioSource>().volume = 0.25f;
            RegisterDestroyCallback(controller, group);
        }

        yield return new WaitForSeconds(3.5f);

        isPattern2Active = false;
    }

    private IEnumerator StartPattern3Coroutine(PatternGroup group)
    {
        isPattern3Active = true;

        for (int i = 0; i < 12; i++)
        {
            var ctrl = StartPattern1();
            RegisterDestroyCallback(ctrl, group);
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
}