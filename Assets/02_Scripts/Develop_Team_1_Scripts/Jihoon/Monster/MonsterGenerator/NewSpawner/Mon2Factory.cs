using System.Collections;
using UnityEngine;

public class Mon2Factory : MonsterFactory
{
    private static readonly int COLOR_CHANGE = Shader.PropertyToID("_ColorChange");
    private static readonly int INNER_SCALE = Shader.PropertyToID("_InnerScale");
    
    [SerializeField] private GameObject focusingObj;

    private float genRadius = 35f;
    private bool isPattern2Active = false;
    private bool isPattern3Active = false;
    private Vector3 newIndicatorPos = Vector3.zero;

    private WaitForSeconds pattern3Wait = new WaitForSeconds(0.5f);

    #region 재정의 메서드

    public override MonsterController SpawnMonster(int allowedPattern)
    {
        var cost = GetCostFromRate(Random.Range(0, 100), allowedPattern);
        var spawnCount = GetSpawnCountByCost(cost);

        if (!TryConsumeDifficulty(cost)) return null;

        var group = new PatternGroup { Remaining = spawnCount, Cost = cost };
        ActiveGroups.Add(group);

        if (cost == 3)
        {
            var ctrl = StartPattern1();
            RegisterDestroyCallback(ctrl, group);
        }
        else if (cost == 5)
        {
            if (isPattern2Active) return null;
            StartCoroutine(StartPattern2Coroutine(group));
        }
        else
        {
            if (isPattern3Active) return null;
            StartCoroutine(StartPattern3Coroutine(group));
        }

        return null;
    }

    protected override int GetCostFromRate(int rate, int allowedPattern)
    {
        switch (allowedPattern)
        {
            case 1:
                return 3;
            case 2:
                return rate switch
                {
                    < 70 => 3,
                    _ => 5
                };
            case 3:
                return rate switch
                {
                    < 60 => 3,
                    < 85 => 5,
                    _ => 7
                };
        }

        return 0;
    }

    protected override Vector3 FindGenPosition()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        float angle = 2f * Mathf.PI * Random.Range(0f, 1f);
        return PlayerTransform.position + genRadius * new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
    }

    protected override Quaternion SetGenRotation(Vector3 genPos)
    {
        newIndicatorPos = PlayerTransform.position;
        return Quaternion.LookRotation(newIndicatorPos - genPos, Vector3.up);
    }

    #endregion

    #region 내부 정의 메서드

    /// <summary>
    /// 코스트 값에 따라 소환할 몬스터 수를 반환합니다.
    /// </summary>
    /// <param name="cost">몬스터 생성에 필요한 난이도 코스트</param>
    /// <returns>소환할 몬스터 수</returns>
    private int GetSpawnCountByCost(int cost)
    {
        if (cost == 3) return 1;
        if (cost == 5) return 6;
        return 12;
    }

    /// <summary>
    /// 기본 패턴(패턴1)으로 몬스터를 한 마리 생성하고 인디케이터를 설정합니다.
    /// </summary>
    /// <returns>생성된 몬스터 컨트롤러</returns>
    private MonsterController StartPattern1()
    {
        var controller = DefaultSpawn(0);
        controller.InitEnemy(PlayerTransform);

        Transform indicator = controller.indicatorCtrl.GetIndicatorTransform();
        Vector3 indicatorPos = newIndicatorPos + new Vector3(0f, indicator.position.y, 0f);
        indicator.position = indicatorPos;
        indicator.parent = transform;

        return controller;
    }

    /// <summary>
    /// 패턴 2: 플레이어 중심으로 6마리의 몬스터를 원형으로 생성하고 SafeZone 애니메이션을 재생합니다.
    /// </summary>
    /// <param name="group">소환된 몬스터들이 속할 패턴 그룹</param>
    /// <returns>코루틴 IEnumerator</returns>
    private IEnumerator StartPattern2Coroutine(PatternGroup group)
    {
        isPattern2Active = true;

        var center = PlayerTransform.position;

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
            controller.InitEnemy(PlayerTransform);

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
    /// 패턴 3: 일정 시간 간격으로 총 12마리의 몬스터를 순차적으로 생성합니다.
    /// </summary>
    /// <param name="group">소환된 몬스터들이 속할 패턴 그룹</param>
    /// <returns>코루틴 IEnumerator</returns>
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
    /// SafeZone 애니메이션을 시간에 따라 점점 색과 크기를 변화시키며 출력합니다.
    /// </summary>
    /// <param name="duration">애니메이션 총 재생 시간 (초)</param>
    /// <returns>코루틴 IEnumerator</returns>
    private IEnumerator SafeZoneAnimationCoroutine(float duration)
    {
        float lapse = 0;
        focusingObj.SetActive(true);
        Renderer indiRenderer = focusingObj.GetComponentInChildren<Renderer>();
        Material indiMat = indiRenderer.material;

        indiMat.SetFloat(COLOR_CHANGE, 0f);
        indiMat.SetFloat(INNER_SCALE, 0.1f);

        while (lapse < 0.87f * duration)
        {
            lapse += Time.deltaTime;
            float ratio = Mathf.Sin(0.5f * Mathf.PI * Mathf.Clamp01(lapse / (0.87f * duration)));
            indiMat.SetFloat(COLOR_CHANGE, ratio);
            indiMat.SetFloat(INNER_SCALE, (1f - ratio) * 0.1f + ratio);
            yield return null;
        }

        lapse = 0;
        while (lapse < 0.13f * duration)
        {
            lapse += Time.deltaTime;
            float ratio = Mathf.Cos(0.5f * Mathf.PI * Mathf.Clamp01(lapse / (0.13f * duration)));
            indiMat.SetFloat(COLOR_CHANGE, ratio);
            indiMat.SetFloat(INNER_SCALE, (1f - ratio) * 0.1f + ratio);
            yield return null;
        }

        indiMat.SetFloat(COLOR_CHANGE, 1f);
        indiMat.SetFloat(INNER_SCALE, 1f);
        focusingObj.SetActive(false);
    }

    #endregion
}