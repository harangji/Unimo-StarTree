using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
public class FlowerGenerator_Lobby : MonoBehaviour
{
    [SerializeField] private float genRadius;
    [SerializeField] private GameObject flowerPrefab;
    private float genTime = 7f;
    private bool isGen = true;
    public Vector3[] AllPos;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(genCoroutine());
    }

    public void InitGenFlower()
    {
        for (int i = 0; i < (Base_Manager.Data.UserData.Level / 50) + 1; i++)
        {
            bool RandomCount = Random.Range(0, 2) == 1 ? true : false;
            if (RandomCount)
            {
                genFlower();
            }
        }
    }
    private void genFlower()
    {
        if (Land.instance.FlowerValue >= 5 + (CharacterCount() * 2))
        {
            return;
        }
        var RandomValue = AllPos[Random.Range(0, AllPos.Length)];
        Land.instance.FlowerValue++;
        var go = Instantiate(flowerPrefab, new Vector3(RandomValue.x, RandomValue.y + 0.5f, RandomValue.z), Quaternion.identity);
        Pinous_Flower_Holder.FlowerHolder.Add(go.transform);
    }

    private int CharacterCount()
    {
        int value = 0;
        for(int i = 0; i< Base_Manager.Data.UserData.GetCharacterData.Length; i++)
        {
            if (Base_Manager.Data.UserData.GetCharacterData[i] == true) value++;
        }
        return value;
    }
  
    private IEnumerator genCoroutine()
    {
        yield return new WaitForSeconds(Random.Range(1f, 4f));
        while (true)
        {
            yield return new WaitForSeconds(genTime * Random.Range(0.8f, 2f));
            if (isGen) { genFlower(); }
        }
    }
}
