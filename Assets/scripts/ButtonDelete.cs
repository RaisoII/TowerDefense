using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDelete : MonoBehaviour, InterfaceStructButton
{
    [SerializeField] private StructBase currentStruct;
    public void ListenerButton() => currentStruct.destroyStructure();
    public void SetEnabledButton(bool state){}
}
