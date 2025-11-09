using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrencyCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private TargetCurrency targetCurrency;

    [SerializeField] private int currentGem;
    private enum TargetCurrency { gem }
    void Start()
    {
        switch (targetCurrency)
        {
            case TargetCurrency.gem:
                text.SetText(PlayerCurrency.Gem.ToString());
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentGem = PlayerCurrency.Gem;
    }
}
