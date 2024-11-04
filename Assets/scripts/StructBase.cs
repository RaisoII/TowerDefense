using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StructBase : MonoBehaviour
{
    [SerializeField] private List<GameObject> listButton;
    private bool currentState;
    private GameObject parent; // para los edificios saber que pedazo de tierra lo invocó
    
    public void ActivatedButton(bool state)
    {
        foreach(GameObject button in listButton)
            button.SetActive(state);

        currentState = state;
    }

    public bool GetStateButton() => currentState;
    public void SetParent(GameObject parent) => this.parent = parent;
    public GameObject GetParent() => parent;
}
