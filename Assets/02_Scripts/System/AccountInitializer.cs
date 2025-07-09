using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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
    [SerializeField] private GameObject typeSetter;
    [SerializeField] private GameObject nameSetter;
    [SerializeField] private TMP_InputField inputField;

    EAccountType m_accountType;
    public Sprite[] sprites;
    public UnityEngine.UI.Image TitleImage;
    int value = 0;
    private void Start()
    {
        if (Localization_Mng.LocalAccess == "kr")
            inputField.characterLimit = 10;
        else inputField.characterLimit = 20;
        Sound_Manager.instance.Play(Sound.Bgm, "BGM000");
        switch(Localization_Mng.LocalAccess)
        {
            case "kr":
                TitleImage.sprite = sprites[0];
                break;
            case "es":  TitleImage.sprite = sprites[1];
                break;
            default: TitleImage.sprite = sprites[2];
                break;
        }

        var data = PlayerPrefs.GetInt("GetUser");
        
        value = data;
        EAccountType accountT = (EAccountType)data;
        m_accountType = accountT;
        if (data == 0)
        {
            StartAccountInitialize();
        }
        else
        {
            EndAccountInitialize();
        }
    }
    
    public void StartAccountInitialize()
    {
        typeSetter.SetActive(true);
    }
    public void EndAccountInitialize()
    {
        PlayerPrefs.SetInt("GetUser", value);
        PlayerPrefs.Save();
        Base_Mng.Firebase.Login(m_accountType);
    }
    public void SetAccountType(int accountType)
    {
        EAccountType accountT = (EAccountType)accountType;
        m_accountType = accountT;
        value = accountType;
        Debug.Log(value);
        typeSetter.SetActive(false);
        nameSetter.SetActive(true);
    }
    public void SetUserName()
    {
        if(inputField.text.Length < 2)
        {
            var go = Instantiate(Resources.Load<PopUP_Toast>("UI/PopUP_Toast"), transform);
            go.GetPopUp("NameCheck");
            return;
        }
        Base_Mng.Firebase.UserName = inputField.text;
        nameSetter.SetActive(false);

        EndAccountInitialize();
    }
}
