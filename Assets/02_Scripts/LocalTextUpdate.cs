using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;

public class LocalTextUpdate : MonoBehaviour
{
    public string productID;
    private void Start()
    {
        GetComponent<TextMeshProUGUI>().text = string.Format("{0} {1}", Base_Mng.IAP.GetProduct(productID).metadata.localizedPrice, Base_Mng.IAP.GetProduct(productID).metadata.isoCurrencyCode);
    }
}
