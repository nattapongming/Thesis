using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;
using UnityEngine.InputSystem;

public class AnalyticManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            OnPuyPromotion("A", 1);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            OnPuyPromotion("D", 5);
        }
    }

    private async void Initialize()
    {
        await UnityServices.InitializeAsync();
        AnalyticsService.Instance.StartDataCollection();
    }

    private void OnPuyPromotion(string item, int amount)
    {
        CustomEvent exampleEvent = new CustomEvent("buyItem")
        {
            { "itemName",item}, { "itemAmount", amount}
        };

        AnalyticsService.Instance.RecordEvent(exampleEvent);
        Debug.Log($"Record buying item : {item} by {amount} amount!");
    }

}
