using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Land : MonoBehaviour
{
    public static Land instance = null;
    private void Awake()
    {
        if (instance == null) instance = this;
    }
    [SerializeField] private LandAnimator[] animators ;
    [SerializeField] private GameObject[] objects;
    [SerializeField] private Transform[] landCheck;
    [SerializeField] private FlowerGenerator_Lobby[] Generators;
    public GameObject GlowParticle, StarParticle;
    public GameObject TestUnimo;
    List<GameObject> Unimos = new List<GameObject>();

    int[] value = { 2, 5, 6, 7, 8 };
    public int FlowerValue = 0;
    int CacheValue = 0;

    private void Start()
    {
        Generators[0].InitGenFlower();

        for (int i = 0; i < Base_Manager.Data.AltaCount.Length; i++)
        {
            if(Base_Manager.Data.UserData.Level >= Base_Manager.Data.AltaCount[i])
            {
                Generators[i+1].InitGenFlower();
                animators[i].gameObject.SetActive(true);
                animators[i].DefaultInitalize();
            }
        }
        bool NoneDefault = false;
        for(int i = 0; i< 5; i++)
        {
            var obj = objects[i].transform.parent.gameObject;
            obj.SetActive(false);
        }
        for(int i = 0; i< 5; i++)
        {
            if (i < 4)
            {
                if (Base_Manager.Data.UserData.Level >= Base_Manager.Data.AltaCount[3 - i])
                {
                    NoneDefault = true;
                    var obj = objects[4 - i].transform.parent.gameObject;
                    obj.gameObject.SetActive(true);
                    break;
                }
            }
        }

        if (NoneDefault == false)
            objects[0].transform.parent.gameObject.SetActive(true);

        GetUnimo();
    }

    

    public void GetUnimo()
    {
        for (int i = 0; i < Base_Manager.Data.AltaCount.Length; i++)
        {
            if (i > 0)
            {
                if (Base_Manager.Data.UserData.Level >= Base_Manager.Data.AltaCount[i - 1])
                {
                    CacheValue++;
                }
            }
        }
        for(int i = 0; i < Base_Manager.Data.UserData.GetCharacterData.Length; i++)
        {
            if (Base_Manager.Data.UserData.GetCharacterData[i] == true)
            {
                GetCharacter(i);
            }
        }
    }

    public void GetCharacter(int i)
    {
        int value01 = CacheValue - 1;
        int value02 = value01 <= 0 ? 0 : value01;
        var land = landCheck[Random.Range(0, value[value02])];
        Debug.Log(value[value02]);
        var go = Instantiate(TestUnimo, new Vector3(land.position.x + Random.Range(-3.0f, 3.0f),
            land.position.y + 8.0f, land.position.z + Random.Range(-1.5f, 1.5f)), Quaternion.identity, transform).GetComponent<TempUnimoSetter>();
        go.GetComponent<AI_Move>().Name = i.ToString();
        go.currentChar = i + 1;
        go.currentEq = i + 1;
    }
    public void GetLandAnimation(int value)
    {
        animators[value].gameObject.SetActive(true);
        animators[value].Initalize();
    }

    public void GetLevelUpAlta(int value)
    {
        StartCoroutine(LevelUpAlta(value));
    }

    IEnumerator LevelUpAlta(int value)
    {
        Canvas_Holder.instance.GetLock(false);
        Camera_Event.instance.GetCameraEvent(CameraMoveState.Alta_LevelUP);

        yield return new WaitForSeconds(0.3f);
        Canvas_Holder.instance.Get_Toast("LevelUP01");

        var alta = objects[value].transform.parent.gameObject;
        float current = 0;
        float percent = 0;
        Color startColor = Color.black;
        Color endColor = Color.white;

        GlowParticle.SetActive(true);
        while(percent < 1)
        {
            current += Time.deltaTime;
            percent = current / 1.0f;

            Color LerpColor = Color.Lerp(startColor, endColor, percent);
            for (int i = 0; i < objects[value].transform.childCount; i++)
            {
                var obj = objects[value].transform.GetChild(i).GetComponent<Renderer>();
                obj.material.SetColor("_EmissionColor", LerpColor);
            }
            yield return null;
        }
        current = 0.0f;
        percent = 0.0f;
        Vector3 startPos = Vector3.one;
        Vector3 endPos = Vector3.zero;
        GlowParticle.SetActive(false);

        while (percent < 1.0f)
        {
            current += Time.deltaTime;
            percent = current / 0.2f;
            Vector3 LerpVector = Vector3.Lerp(startPos, endPos, percent);
            alta.transform.localScale = LerpVector;
            yield return null;
        }
        alta.SetActive(false);
        current = 0.0f;
        percent = 0.0f;
        Vector3 LaststartPos = Vector3.zero;
        Vector3 LastendPos = Vector3.one;
        var go = objects[value+1];
        go.transform.parent.gameObject.SetActive(true);
        while (percent < 1.0f)
        {
            current += Time.deltaTime;
            percent = current / 0.2f;
            Vector3 LerpVector = Vector3.Lerp(LaststartPos, LastendPos, percent);
            go.transform.localScale = LerpVector;
            yield return null;
        }
        StarParticle.SetActive(true);
        Sound_Manager.instance.Play(Sound.Effect, "effect_1001");

        yield return new WaitForSeconds(1.0f);
        StarParticle.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        Canvas_Holder.instance.GetLock(true);
        Camera_Event.instance.ReturnCamera();
        yield return new WaitForSeconds(1.0f);
        Canvas_Holder.instance.Get_Toast("LevelUP02");
        GetLandAnimation(value);
        if (value == 2)
        {
            yield return new WaitForSeconds(0.5f);
            if (Base_Manager.Data.UserData.GetVane == false)
            {
                Base_Manager.Data.UserData.GetVane = true;
                Canvas_Holder.instance.GetUI("##Vane");
            }
        }
    }
}
