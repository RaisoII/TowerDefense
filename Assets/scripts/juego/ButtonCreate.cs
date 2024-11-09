using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCreate : MonoBehaviour,InterfaceStructButton
{
    [SerializeField] private StructEmpty currentStruct;
    [SerializeField] private int indexStructure;
    private bool enabledButton;

    private void Awake()
    {
        enabledButton = true;
    }

    public void ListenerButton()
    {
        if(enabledButton)
            currentStruct.CreateStructure(indexStructure);
    }

    public void SetEnabledButton(bool state)
    {
        enabledButton = state;
    } 
}
