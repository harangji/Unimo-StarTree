using System;
using TMPro;
using UnityEngine;

public class ShopTextChange : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI mPriceText;
    private int mPriceValue;

    private void Start()
    {
        mPriceValue = int.Parse(mPriceText.text);
        mPriceText.text = StringMethod.ToCurrencyString(mPriceValue);
    }
}
