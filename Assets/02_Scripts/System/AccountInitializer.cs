using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Android.Gradle.Manifest;
using static UnityEngine.Rendering.DebugUI;

public class AccountInitializer : MonoBehaviour
{
    public static AccountInitializer instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    // [SerializeField] private GameObject typeSetter;
    [SerializeField] private GameObject nameSetter;
    [SerializeField] private TMP_InputField inputField;

    // EAccountType m_accountType;
    public Sprite[] sprites;
    public UnityEngine.UI.Image TitleImage;
    int value = 0;
    private void Start()
    {
        if (Localization_Manager.LocalAccess == "kr")
            inputField.characterLimit = 10;
        else inputField.characterLimit = 20;
        Sound_Manager.instance.Play(Sound.Bgm, "BGM000");
        switch(Localization_Manager.LocalAccess)
        {
            case "kr":
                TitleImage.sprite = sprites[0];
                break;
            case "es":  TitleImage.sprite = sprites[1];
                break;
            default: TitleImage.sprite = sprites[2];
                break;
        }
        
        if (EasySaveManager.Instance.TryLoad("User_Data", out User_Data user)) //파일이 있는지 검사
        {
            Base_Manager.Data.LoadUserData(); //있다면 진행
        }
        else
        {
            nameSetter.SetActive(true);
        }
    }
    

    // public void StartAccountInitialize()
    // {
    //     typeSetter.SetActive(true); //구글 게스트 고르기
    // }
    
    // public void EndAccountInitialize()
    // {
    //     Base_Manager.Data.LoadUserData();
    // }
    
    // public void SetAccountType(int accountType) //버튼
    // {
    //     value = accountType;
    //     Debug.Log(value);
    //     typeSetter.SetActive(false);
    //     nameSetter.SetActive(true);
    // }
    
    public void SetUserName() //버튼
    {
        if(inputField.text.Length < 2)
        {
            var go = Instantiate(Resources.Load<PopUP_Toast>("UI/PopUP_Toast"), transform);
            go.GetPopUp("NameCheck");
            return;
        }

        Base_Manager.Data.UserData.UserName = inputField.text;
        nameSetter.SetActive(false);

        Base_Manager.Data.LoadUserData();
    }
}
