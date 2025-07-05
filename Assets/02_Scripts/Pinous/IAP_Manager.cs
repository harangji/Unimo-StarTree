using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;
using System;
using System.Runtime.InteropServices;
using GoogleMobileAds.Api;
using Unity.Services.Core;
using Unity.Services.Core.Environments;

public class IAP_Manager : IStoreListener
{
    public Action GetAction;
    [Header("Product ID")]
    public readonly string gem01 = "bluehoney_1500";
    public readonly string gem02 = "bluehoney_3300";
    public readonly string gem03 = "bluehoney_6900";
    public readonly string gem04 = "bluehoney_9900";
    public readonly string gem05 = "bluehoney_29000";
    public readonly string gem06 = "bluehoney_49000";
    public readonly string removeAds = "removeads";
    public readonly string removeAds_ALL = "revemoads_all";
    public readonly string character_cu = "character_cu";
    public readonly string character_primo = "character_primo";
    public readonly string eq_bath = "eq_bath";


    [Header("Cache")]
    private IStoreController storeController; //구매 과정을 제어하는 함수 제공자
    private IExtensionProvider storeExtensionProvider; //여러 플랫폼을 위한 확장 처리 제공자
    public string environment = "production";


    public void Init()
    {
        InitUnityIAP(); //Start 문에서 초기화 필수
    }

    /* Unity IAP를 초기화하는 함수 */
    private void InitUnityIAP()
    {
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        /* 구글 플레이 상품들 추가 */
        builder.AddProduct(gem01, ProductType.Consumable, new IDs() { { gem01, GooglePlay.Name, gem01, AppleAppStore.Name } });
        builder.AddProduct(gem02, ProductType.Consumable, new IDs() { { gem02, GooglePlay.Name, gem02, AppleAppStore.Name } });
        builder.AddProduct(gem03, ProductType.Consumable, new IDs() { { gem03, GooglePlay.Name, gem03, AppleAppStore.Name } });
        builder.AddProduct(gem04, ProductType.Consumable, new IDs() { { gem04, GooglePlay.Name, gem04, AppleAppStore.Name } });
        builder.AddProduct(gem05, ProductType.Consumable, new IDs() { { gem05, GooglePlay.Name, gem05, AppleAppStore.Name } });
        builder.AddProduct(gem06, ProductType.Consumable, new IDs() { { gem06, GooglePlay.Name, gem06, AppleAppStore.Name } });
        builder.AddProduct(removeAds, ProductType.NonConsumable, new IDs() { { removeAds, GooglePlay.Name, removeAds, AppleAppStore.Name } });
        builder.AddProduct(removeAds_ALL, ProductType.NonConsumable, new IDs() { { removeAds_ALL, GooglePlay.Name, removeAds_ALL, AppleAppStore.Name } });
        builder.AddProduct(character_cu, ProductType.NonConsumable, new IDs() { { character_cu, GooglePlay.Name, character_cu, AppleAppStore.Name } });
        builder.AddProduct(character_primo, ProductType.NonConsumable, new IDs() { { character_primo, GooglePlay.Name, character_primo, AppleAppStore.Name } });
        builder.AddProduct(eq_bath, ProductType.NonConsumable, new IDs() { { eq_bath, GooglePlay.Name, eq_bath, AppleAppStore.Name } });


        UnityPurchasing.Initialize(this, builder);
    }

    /* 구매하는 함수 */
    public void Purchase(string productId)
    {
        Product product = storeController.products.WithID(productId); //상품 정의
        if (product != null && product.availableToPurchase) //상품이 존재하면서 구매 가능하면
        {
            storeController.InitiatePurchase(product); //구매가 가능하면 진행
        }
        else //상품이 존재하지 않거나 구매 불가능하면
        {
            Debug.Log("상품이 없거나 현재 구매가 불가능합니다");
        }
    }

    /* 초기화 성공 시 실행되는 함수 */
    public void OnInitialized(IStoreController controller, IExtensionProvider extension)
    {
        Debug.Log("초기화에 성공했습니다");

        storeController = controller;
        storeExtensionProvider = extension;
    }

    /* 초기화 실패 시 실행되는 함수 */
    void IStoreListener.OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("초기화에 실패했습니다" + error);
    }
    void IStoreListener.OnInitializeFailed(InitializationFailureReason error, string temp)
    {
        Debug.Log("초기화에 실패했습니다" + error);

    }

    /* 구매에 실패했을 때 실행되는 함수 */
    public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
    {
        Debug.Log("구매에 실패했습니다");
    }

    public void RestoreClass()
    {
        storeExtensionProvider.GetExtension<IAppleExtensions>().RestoreTransactions(result =>
        {
            if (result)
            {

            }
            else
            {

            }
        });
    }


    /* 구매를 처리하는 함수 */
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        Debug.Log("#############CALL PROCESSPURCHASE");
        Debug.Log("#############" + args.purchasedProduct.transactionID);
        Debug.Log("#############" + args.purchasedProduct.definition.id);

        Base_Mng.Data.data.IAP++;

        Canvas_Holder.instance.GetUI("##Reward");
        Canvas_Holder.UI_Holder.Peek().transform.parent = Canvas_Holder.instance.transform;
        Canvas_Holder.UI_Holder.Peek().GetComponent<UI_Reward>().GetIAPReward(args.purchasedProduct.definition.id);

        return PurchaseProcessingResult.Complete;
    }

    public Product GetProduct(string _productId)
    {
        return storeController.products.WithID(_productId);
    }
}
