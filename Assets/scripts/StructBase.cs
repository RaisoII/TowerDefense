using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class StructBase : MonoBehaviour
{
    [SerializeField] protected List<GameObject> prefabsStructures; 
    [SerializeField] protected List<GameObject> listStructuresButton;
    [SerializeField] protected GameObject buttonDelete,buttonDestinySoldier;
    [SerializeField] protected GameObject gameObjectRange;
    [SerializeField] protected int cost;
    [SerializeField] protected float range;
    [SerializeField] protected Color enabledColor,disabledColor;
    protected MoneyManager moneyManager;
    protected bool currentState;
    protected StructEmpty parent; // para los edificios saber que pedazo de tierra lo invocó
    
    protected virtual void Start()
    {
        moneyManager = GameObject.Find("scriptsGenerales").GetComponent<MoneyManager>();
        int costStructure;
        
        for(int i = 0; i < prefabsStructures.Count;i++)
        {
            GameObject prefab = prefabsStructures[i];
            costStructure = prefab.GetComponent<StructBase>().GetCost();
            GameObject button = listStructuresButton[i];
            button.transform.GetChild(2).gameObject.GetComponent<TextMeshPro>().text = ""+costStructure;
        }
    }    
    public void ActivatedButton(bool state)
    {
        int cantMoney = moneyManager.GetMoney();
        foreach(GameObject button in listStructuresButton)
        {
            button.SetActive(state);
            if(state)
                checkStateButton(button,cantMoney);
        }

        if(buttonDelete != null)
            buttonDelete.SetActive(state);

        if(buttonDestinySoldier != null)
            buttonDestinySoldier.SetActive(state);
        
        gameObjectRange.SetActive(state);
        currentState = state;
    }

    private void checkStateButton(GameObject button,int cantMoney)
    {
        InterfaceStructButton interfaceButton = button.GetComponent<InterfaceStructButton>();
        string stringValue = button.transform.GetChild(2).gameObject.GetComponent<TextMeshPro>().text;
        
        int value = int.Parse(stringValue);
        SpriteRenderer renderer = button.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        
        if(value <= cantMoney)
        {
            interfaceButton.SetEnabledButton(true);
            renderer.color = enabledColor;
        }
        else
        {
            interfaceButton.SetEnabledButton(false);
            renderer.color = disabledColor;
        }
    }

    public bool GetStateButton() => currentState;
    public void SetParent(StructEmpty parent) => this.parent = parent;
    public StructEmpty GetParent() => parent;
    public int GetCost() => cost;

    public float GetRange() => range;
    public abstract void DestroyStructure();
    
    public abstract void MoveSoldiers(Vector2 pos);
}
