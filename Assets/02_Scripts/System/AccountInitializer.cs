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
        
        if (EasySaveManager.Instance.TryLoad("User_Data", out User_Data user)) //������ �ִ��� �˻�
        {
            Base_Manager.Data.LoadUserData(); //�ִٸ� ����
        }
        else
        {
            nameSetter.SetActive(true);
        }
    }
    

    // public void StartAccountInitialize()
    // {
    //     typeSetter.SetActive(true); //���� �Խ�Ʈ ����
    // }
    
    // public void EndAccountInitialize()
    // {
    //     Base_Manager.Data.LoadUserData();
    // }
    
    // public void SetAccountType(int accountType) //��ư
    // {
    //     value = accountType;
    //     Debug.Log(value);
    //     typeSetter.SetActive(false);
    //     nameSetter.SetActive(true);
    // }
    
    public void SetUserName() //��ư
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
