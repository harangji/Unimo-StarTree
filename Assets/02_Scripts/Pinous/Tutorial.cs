using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;
using static UnityEngine.Rendering.GPUSort;

[System.Serializable]
public class NameMaker
{
    public Sprite[] MainSprite;
    public AudioClip clip;
    public string Title;
    public string[] Description;
}
public class Tutorial : MonoBehaviour
{
    [SerializeField] private Image MainImage;
    [SerializeField] private TextMeshProUGUI NameText;
    [SerializeField] private TextMeshProUGUI ExplaneText;

    [SerializeField] private List<NameMaker> Makers = new List<NameMaker>();
    [SerializeField] private Animator FadeAnimation;

    int tutorialIDX = 0;
    int subIDX;
    Coroutine coroutine;
    private void Start()
    {
        GetScene(0);
        Sound_Manager.BGM_volume = 0.25f;
        Sound_Manager.instance.SoundCheck();
        Sound_Manager.instance.Play(Sound.Bgm, "BGM000");
        if (Base_Mng.Data.data.ADSBuy == false)
            transform.GetChild(0).GetComponent<RectTransform>().anchorMin = new Vector2(0, 0.0f);
        else transform.GetChild(0).GetComponent<RectTransform>().anchorMin = new Vector2(0, 0.1f);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (subIDX == 1) GetScene(tutorialIDX, subIDX);
            else GetScene(++tutorialIDX, 0);
        }
    }

    public void GetAllSkip()
    {
        StopAllCoroutines();
        if (PlayerPrefs.GetInt("Tutorial") == 0)
        {
            WholeSceneController.Instance.ReadyNextScene(1);
            Base_Mng.Data.data.GamePlay++;
        }
        else
            WholeSceneController.Instance.ReadyNextScene(0);
        PlayerPrefs.SetInt("Tutorial", 1);

        Pinous_Flower_Holder.FlowerHolder.Clear();
        //WholeSceneController.Instance.ReadyNextScene(0);
    }

    IEnumerator TextCoroutine(bool LengthTwo = false, bool CheckNext = false)
    {
        if(LengthTwo && !CheckNext)
        {
            yield return new WaitForSeconds(1.5f);
            GetScene(tutorialIDX, 1);
            yield break;
        }
        yield return new WaitForSeconds(3.0f);
        GetScene(++tutorialIDX);
    }
    public void GetScene(int idx, int subidx = 0)
    {
        if(idx >= Makers.Count)
        {
            if (PlayerPrefs.GetInt("Tutorial") == 0)
            {
                WholeSceneController.Instance.ReadyNextScene(1);
                Base_Mng.Data.data.GamePlay++;
            }
            else
                WholeSceneController.Instance.ReadyNextScene(0);
    
            Pinous_Flower_Holder.FlowerHolder.Clear();
            PlayerPrefs.SetInt("Tutorial", 1);

            return;
        }

        MainImage.sprite = Makers[idx].MainSprite[subidx];
        ExplaneText.alignment = Makers[idx].Title == "" ? TextAlignmentOptions.Center : TextAlignmentOptions.TopLeft;
        if (Makers[idx].Title != "")
        {
            NameText.text = Localization_Mng.local_Data["Character/" + Makers[idx].Title].Get_Data();
        }
        else
        {
            NameText.text = "";
        }
        if (Makers[idx].Description.Length != 0)
            ExplaneText.text = Localization_Mng.local_Data["Tutorial01_" + (Makers[idx].Description[Makers[idx].Description.Length > 1 ? subidx : 0])].Get_Data();
        else ExplaneText.text = "";
        subIDX = 0;

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        if (subidx == 0)
        {
            subIDX = Makers[idx].MainSprite.Length > 1 ? 1 : 0;
            if (Makers[idx].clip != null)
            {
                Sound_Manager.instance.PlayClip(Makers[idx].clip);
            }
            coroutine = StartCoroutine(TextCoroutine(Makers[idx].MainSprite.Length > 1 ? true : false));
        }
        else coroutine = StartCoroutine(TextCoroutine(true, true));
    }
}
