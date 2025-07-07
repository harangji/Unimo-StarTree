using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountManager : SingletonBehaviour<AccountManager>
{
    [HideInInspector] public AccountData AccountData;
    new private void Awake()
    {
        base.Awake();
        if (IsNotWantedObj) { return; }
    }
    public void SetAccountType(EAccountType accountType)
    {
        AccountData.AccountType = accountType;
    }
    public void SetUserName(string userName)
    {
        //First setting is completed when name is set
        AccountData.Name = userName;
        Base_Mng.Firebase.UserName = userName;
        PlayDataManager.SaveData<AccountData>(AccountData, typeof(AccountData).ToString());

        AccountConfig config = new AccountConfig();
        config.isNotFirst = true;
        config.AccountType = AccountData.AccountType;
        DataConfigIOManager.SaveData<AccountConfig>(config, typeof(AccountConfig).ToString());

        AccountInitializer initializer = FindAnyObjectByType<AccountInitializer>();
        if (initializer != null) { initializer.EndAccountInitialize(); }
    }    
    public void loadAccountData(bool hasdata)
    {
        if (hasdata == false) 
        {
            AccountInitializer initializer = FindAnyObjectByType<AccountInitializer>();
            if (initializer != null) { initializer.StartAccountInitialize(); }
        }
        else 
        {
            AccountInitializer initializer = FindAnyObjectByType<AccountInitializer>();
            if (initializer != null) { initializer.EndAccountInitialize(); }
        }
    }
}
[System.Serializable]
public class AccountConfig
{
    public bool isNotFirst;
    public EAccountType AccountType;
}
[System.Serializable]
public class AccountData
{
    public EAccountType AccountType;
    public string Name;
    public int Portrait;
}
public enum EAccountType
{
    NONE = 0,
    GUEST = 1,
    GOOGLE = 2,
    APPLE = 3,
    FACEBOOK = 4
}