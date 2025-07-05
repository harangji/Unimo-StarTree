using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonGeneratorManager : MonoBehaviour
{
    public static List<MonsterController> AllMonsterListSTATIC;

    [SerializeField] private List<MonsterGenerator> generators;
    [SerializeField] private List<float> activateTime;
    [SerializeField] private float extreamTime;
    [SerializeField] private float extreamCoolTime;
    private float lapseTime = 0f;
    private float currentExCoolTime;
    private int newGenIdx = 0;
    private void Awake()
    {
        AllMonsterListSTATIC = new List<MonsterController>();
        foreach(var gen in generators) { gen.gameObject.SetActive(false); }
        currentExCoolTime = 0f;
    }
    private void Start()
    {
        PlaySystemRefStorage.playProcessController.SubscribeGameoverAction(StopGenerateAllMonster);
    }
    // Update is called once per frame
    void Update()
    {
        lapseTime += Time.deltaTime;
        if (newGenIdx < generators.Count) { checkNewGenerator(); }
        if (lapseTime > extreamTime)
        {
            currentExCoolTime -= Time.deltaTime;
            if (currentExCoolTime < 0f)
            {
                triggerRndExPattern();
                setNextExCoolTime();
            }
        }
    }
    public void StopGenerateAllMonster()
    {
        foreach(var gen in generators)
        {
            gen.PauseGenerate(true);
        }
    }
    public void PauseGenerateAllMonster()
    {
        foreach (var gen in generators)
        {
            gen.PauseGenerate(false);
        }
    }
    public void ResumeGenerateAllMonster()
    {
        foreach (var gen in generators)
        {
            gen.ResumeGenerator();
        }
    }
    private void checkNewGenerator()
    {
        if (newGenIdx >= generators.Count) { return; }
        if (lapseTime > activateTime[newGenIdx])
        {
            generators[newGenIdx].gameObject.SetActive(true);
            ++newGenIdx;
        }
    }
    private void setNextExCoolTime()
    {
        currentExCoolTime = Random.Range(0.8f, 1.1f) * extreamCoolTime;
    }
    private void triggerRndExPattern()
    {
        int idx = Random.Range(0, generators.Count);
        generators[idx].TriggerExPattern();
    }
}
