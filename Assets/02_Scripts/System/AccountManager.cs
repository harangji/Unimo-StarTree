// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class AccountManager : SingletonBehaviour<AccountManager>
// {
//     // [HideInInspector] public AccountData AccountData;
//     new private void Awake()
//     {
//         base.Awake();
//         if (IsNotWantedObj) { return; }
//     }
//
//     public void SetUserName(string userName)
//     {
//         Base_Manager.Data.UserData.UserName = userName;
//         AccountInitializer initializer = FindAnyObjectByType<AccountInitializer>();
//         if (initializer != null) { initializer.EndAccountInitialize(); }
//     }
//     
//     public void loadAccountData()
//     {
//         AccountInitializer initializer = FindAnyObjectByType<AccountInitializer>();
//         if (initializer != null) { initializer.EndAccountInitialize(); }
//     }
// }
