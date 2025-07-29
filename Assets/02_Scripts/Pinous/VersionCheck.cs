using UnityEngine;

public class VersionCheck : MonoBehaviour
{
    public void ResetUserDataButton()
    {
        Debug.Log("ResetUserDataButton");
        
        EasySaveManager.Instance.DeleteAll();
        PlayerPrefs.DeleteAll();
        
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            
        #elif UNITY_ANDROID
            AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
            activity.Call<bool>("moveTaskToBack", true);
        
        #else
            Application.Quit();
        #endif
    }
    
    // public static VersionCheck instance = null;
    // public GameObject VersionPopUP;
    // private void Awake()
    // {
    //     // if(instance == null)
    //     // {
    //     //     instance = this;
    //     // }
    // }

    // public void GetVersionPopUP()
    // {
    //     VersionPopUP.SetActive(true);
    // }

//     public void GetVersionStore()
//     {
//         string link = "";
// #if UNITY_ANDROID
//         link = "https://play.google.com/store/apps/details?id=com.HwigGames.IDLEunimo";
// #elif UNITY_IOS
//         link = "https://apps.apple.com/us/app/unimo-startree/id6738403656";
// #endif
//         Application.OpenURL(link);
//     }
}
