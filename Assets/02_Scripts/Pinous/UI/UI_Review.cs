using UnityEngine;

public class UI_Review : UI_Base
{
    public override void Start()
    {
        base.Start();
    }
    public void GetEXIT()
    {
        string link = "";
#if UNITY_ANDROID
        link = "https://play.google.com/store/apps/details?id=com.HwigGames.IDLEunimo";
#elif UNITY_IOS
        link = "https://apps.apple.com/us/app/unimo-startree/id6738403656";
#endif
        Application.OpenURL(link);
    }
    public void None() => DisableOBJ();
}
