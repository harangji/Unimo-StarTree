using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mon002Generator_C : MonsterGenerator
{
    /// <summary>
    /// 생성된 패턴 그룹 정보를 담는 클래스
    /// </summary>
    private class PatternGroup
    {
        public int Remaining; // 남은 몬스터 수
        public int Cost; // 해당 그룹이 소모한 난이도 수치
    }

    private List<PatternGroup> _activeGroups = new List<PatternGroup>();

    [SerializeField] private GameObject focusingObj;
    private float genRadius = 35f;
    private float targetRadius = 3f;
    private Vector3 newIndicatorPos = Vector3.zero;

    private bool isPattern2Active = false;
    private bool isPattern3Active = false;

    private WaitForSeconds pattern3Wait = new WaitForSeconds(0.5f);

    /// <summary>
    /// OnEnable 시 부모의 OnEnable 호출
    /// </summary>
    new void OnEnable() => base.OnEnable();

    /// <summary>
    /// 매 프레임마다 부모의 Update 호출
    /// </summary>
    new void Update() => base.Update();

    /// <summary>
    /// 적을 생성하고 적절한 패턴에 따라 동작시킴
    /// </summary>
    protected override MonsterController generateEnemy()
    {
        int cost = GetRandomCost();

        var stageMgr = PlaySystemRefStorage.stageManager;

        if (stageMgr == null || !stageMgr.TryConsumeDifficulty(cost))
        {
            Debug.Log("할당할 난이도 수치가 부족합니다.");
            return null;
        }

        int spawnCount = GetSpawnCountByCost(cost);
        var group = new PatternGroup { Remaining = spawnCount, Cost = cost };
        _activeGroups.Add(group);

        if (cost == 3)
        {
            Debug.Log("[하트] 패턴 1");
            var ctrl = StartPattern1();
            RegisterDestroyCallback(ctrl, group);
        }
        else if (cost == 5)
        {
            Debug.Log("[하트] 패턴 2");
            if (isPattern2Active) return null;
            StartCoroutine(StartPattern2Coroutine(group));
        }
        else
        {
            Debug.Log("[하트] 패턴 3");
            if (isPattern3Active) return null;
            StartCoroutine(StartPattern3Coroutine(group));
        }

        return null;
    }

    /// <summary>
    /// 몬스터 제거 시 남은 수 감소 및 난이도 회복 처리
    /// </summary>
    private void RegisterDestroyCallback(MonsterController ctrl, PatternGroup group)
    {
        ctrl.OnDestroyed += (monster) =>
        {
            group.Remaining--;
            if (group.Remaining == 0)
            {
                PlaySystemRefStorage.stageManager.RestoreDifficulty(group.Cost);
                _activeGroups.Remove(group);
            }
        };
    }

    /// <summary>
    /// 랜덤한 생성 위치 반환 (플레이어 중심 원형)
    /// </summary>
    protected override Vector3 findGenPosition()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        float angle = 2f * Mathf.PI * Random.Range(0f, 1f);
        return playerTransform.position + genRadius * new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
    }

    /// <summary>
    /// 생성 회전 방향 지정 (플레이어 방향)
    /// </summary>
    protected override Quaternion setGenRotation(Vector3 genPos)
    {
        newIndicatorPos = playerTransform.position;
        return Quaternion.LookRotation(newIndicatorPos - genPos, Vector3.up);
    }

    /// <summary>
    /// 패턴 1: 단일 몬스터 생성
    /// </summary>
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

    /// <summary>
    /// 패턴 2: 원형 중심 주변에 6마리 몬스터 생성 + SafeZone 이펙트
    /// </summary>
    private IEnumerator StartPattern2Coroutine(PatternGroup group)
    {
        isPattern2Active = true;

        // float radius = 5f;
        // float angle = Random.Range(0f, 2f * Mathf.PI);
        // Vector3 center = radius * new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));

        var center = playerTransform.position;

        focusingObj.transform.localScale = 4f * Vector3.one;
        focusingObj.transform.position = center + 0.5f * Vector3.up;
        StartCoroutine(SafeZoneAnimationCoroutine(3.1f));

        for (int i = 0; i < 6; i++)
        {
            float angleStep = 1f / 3f * Mathf.PI * i;
            Vector3 pos = center + genRadius * new Vector3(Mathf.Cos(angleStep), 0f, Mathf.Sin(angleStep));

            newIndicatorPos = center + 4.5f * new Vector3(Mathf.Cos(angleStep + Mathf.PI / 6f), 0f,
                Mathf.Sin(angleStep + Mathf.PI / 6f));
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

    /// <summary>
    /// 패턴 3: 시간 간격을 두고 12마리 연속 소환
    /// </summary>
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

    /// <summary>
    /// SafeZone 애니메이션 표시 코루틴
    /// </summary>
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
            indiMat.SetFloat("_InnerScale", (1f - ratio) * 0.1f + ratio);
            yield return null;
        }

        lapse = 0;
        while (lapse < 0.13f * duration)
        {
            lapse += Time.deltaTime;
            float ratio = Mathf.Cos(0.5f * Mathf.PI * Mathf.Clamp01(lapse / (0.13f * duration)));
            indiMat.SetFloat("_ColorChange", ratio);
            indiMat.SetFloat("_InnerScale", (1f - ratio) * 0.1f + ratio);
            yield return null;
        }

        indiMat.SetFloat("_ColorChange", 1f);
        indiMat.SetFloat("_InnerScale", 1f);
        focusingObj.SetActive(false);
    }

    /// <summary>
    /// 생성 난이도에 따른 비용 반환
    /// </summary>
    private int GetRandomCost()
    {
        int rate = Random.Range(0, 100);
        if (rate < 70) return 3;
        else if (rate < 90) return 5;
        else return 7;
    }

    /// <summary>
    /// 비용에 따라 스폰 수 결정
    /// </summary>
    private int GetSpawnCountByCost(int cost)
    {
        if (cost == 3) return 1;
        if (cost == 5) return 6;
        return 12;
    }
}