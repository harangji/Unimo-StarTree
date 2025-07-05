using UnityEngine;

public class UI_ADS_INTERESTING : MonoBehaviour
{
    void Start()
    {
        GetRectADS();
    }
    public void GetRectADS()
    {
        RectTransform rect = GetComponent<RectTransform>();

        if (Base_Mng.Data.data.ADSBuy == true)
        {
            rect.anchorMin = new Vector2(0.0f, 0.0f);
        }
        else rect.anchorMin = new Vector2(0.0f, 0.1f);
    }
}
