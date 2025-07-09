using System;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

public class Analytics_Manager
{
    public async void Init()
    {
        await UnityServices.InitializeAsync();

        if (UnityServices.State == ServicesInitializationState.Initialized)
        {
            Debug.Log("Unity Gaming Services Initialized.");
        }
    }

    public void RecordCustomEventWithParameters(string ParameterName, int value)
    {
        // NOTE: this will show up on the dashboard as an invalid event, unless
        // you have created a schema that matches it.
        MyEvent myEvent = new MyEvent
        {
            FabulousString = ParameterName,
            SparklingInt = value,
        };

        AnalyticsService.Instance.RecordEvent(myEvent);
    }

    public void RecordSaleItemForInGame(string ItemName)
    {
        TransactionEvent transaction = new TransactionEvent();
        transaction.TransactionName = "InGameItem";
        transaction.TransactionType = TransactionType.PURCHASE;
        transaction.SpentItems.Add(new TransactionItem
        {
            ItemName = ItemName,
            ItemAmount = 1,
            ItemType = "InGame",
        });
    }

    public void RecordSaleItemForPurchase(string ItemName)
    {
        TransactionEvent transaction = new TransactionEvent();
        transaction.TransactionName = "Purchase_Item";
        transaction.TransactionType = TransactionType.PURCHASE;
        transaction.SpentItems.Add(new TransactionItem
        {
            ItemName = ItemName,
            ItemAmount = 1,
            ItemType = "STORE",
        });
    }
}
