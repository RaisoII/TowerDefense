using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMoney;
    [SerializeField] private ClickTerrain clickTerrain;
    [SerializeField] private AudioClip[] coins;

    public int getMoney()
    {
        int cantMoney = int.Parse(textMoney.text);
        return cantMoney;
    }

    public void setCantMoney(int cant)
    {
        if(cant > 0)
            SoundManager.instance.playSFX(coins[Random.Range(0,coins.Length)],false);
            
        int cantMoney = int.Parse(textMoney.text);
        cantMoney +=cant;
        textMoney.text = ""+ cantMoney;
        StructBase structBase =  clickTerrain.GetCurrentStructure();
        
        if(structBase != null)
            structBase.activatedButton(true);
    }
}
