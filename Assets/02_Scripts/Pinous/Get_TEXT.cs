using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Get_TEXT : MonoBehaviour
{
    Vector3 target;
    Camera cam;
    public TextMeshProUGUI m_Text;

    float UpRange = 0.0f;
    private void Start()
    {
        cam = Camera.main;
    }

    public void Init(Vector3 pos, double Get)
    {
        if (Base_Manager.SavingMode)
        {
            ReturnText();
            return;
        }

        pos.x += Random.Range(-1.0f, 1.0f);
        pos.z += Random.Range(-1.0f, 1.0f);

        target = pos;
        m_Text.text = StringMethod.ToCurrencyString(Get);
        // transform.parent = Canvas_Holder.instance.LAYER_HOLDER; //Local 기준 위치/회전/스케일을 잃을 수도 있음
        transform.SetParent(Canvas_Holder.instance.LAYER_HOLDER, false); //Local 기준 위치/회전/스케일 유지됨
        
        Invoke("ReturnText", 3.0f);
    }

    private void Update()
    {
        Vector3 targetPos = new Vector3(target.x, target.y + UpRange, target.z);
        transform.position = cam.WorldToScreenPoint(targetPos);
        if (UpRange <= 0.3f)
        {
            UpRange += Time.deltaTime;
        }
    }

    private void ReturnText()
    {
        Destroy(this.gameObject);
    }
}

