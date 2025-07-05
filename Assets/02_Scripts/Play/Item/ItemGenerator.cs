using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> itemPrefabs;
    [SerializeField] private List<float> appearRatios;
    [SerializeField] private float tickTime = 20f;
    [SerializeField] private float appearProb = 0.1f;
    private List<float> appearAccProb;
    private float currentTick;
    private MapRangeSetter mapSetter;

    void Awake()
    {
        if (itemPrefabs.Count != appearRatios.Count) { Debug.Log("Wrong set item generator."); }
        float appearAcc = 0f;
        appearAccProb = new List<float>();
        foreach (float ratio in appearRatios)
        {
            appearAcc += ratio;
        }
        float prob = 0f;
        foreach (float ratio in appearRatios)
        {
            prob += ratio / appearAcc;
            appearAccProb.Add(prob);
        }
        appearAccProb[^1] = 1.4f;
    }
    // Start is called before the first frame update
    void Start()
    {
        mapSetter = PlaySystemRefStorage.mapSetter;
        currentTick = tickTime;
    }
    public void DecreaseTick(float time)
    {
        currentTick -= time;
        if (currentTick < 0f)
        {
            currentTick = tickTime;
            float rand = Random.Range(0f, 1f);
            if (rand < appearProb)
            {
                generateItem();
            }
        }
    }
    private void generateItem()
    {
        float rand = Random.Range(0f, 1f);
        int idx = 0;
        while (rand > appearAccProb[idx]) { ++idx; }
        FlowerController flower = Instantiate(itemPrefabs[idx], findPosition(), Quaternion.Euler(0f,180f,0f)).
            GetComponent<FlowerController>();
    }
    private Vector3 findPosition()
    {
        float rand = Random.Range(0.0f, 1.0f);
        float radius = mapSetter.MaxRange * rand;
        rand = Random.Range(0.0f, 1.0f);
        float angle = 2 * Mathf.PI * rand;
        Vector3 pos = new Vector3(radius * Mathf.Cos(angle), 0f, radius * Mathf.Sin(angle));
        if (mapSetter.IsInMap(pos) == false)
        { pos = mapSetter.FindNearestPoint(pos); }

        return pos;
    }
}
