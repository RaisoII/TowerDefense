using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructButton : MonoBehaviour,InterfaceStructButton
{
    [SerializeField] private int indexStruct;
    [SerializeField] private StructEmpty structEmpty;

    public void listenerButton() => structEmpty.CreateStructure(indexStruct);
}
