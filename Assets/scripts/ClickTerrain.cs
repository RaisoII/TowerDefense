using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickTerrain : MonoBehaviour
{
    private StructBase currentStruct,previousStruct;

    private void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
            CheckPointClick();
    }

    private void CheckPointClick()
    {
            Vector2 posCursor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(posCursor, Vector2.zero);

            if (hit.collider == null)
            {
                if(currentStruct != null)
                {
                    currentStruct.ActivatedButton(false);
                    currentStruct = null;
                    previousStruct = null;
                }
                
                //Debug.Log("no choca con na'");
                return;
            }

            
                //Debug.Log("col: "+hit.collider.tag + " nombre: "+hit.collider.gameObject.name);

            if(hit.collider.CompareTag("StructBase"))
            {
                currentStruct = hit.collider.gameObject.GetComponent<StructBase>();
                
                if(currentStruct == previousStruct) // si apreto la misma estructura 
                {
                    bool currentState = currentStruct.GetStateButton();
                    currentStruct.ActivatedButton(!currentState);
                    return;
                }
                else
                {
                    if(previousStruct != null)
                        previousStruct.ActivatedButton(false);
                    
                    currentStruct.ActivatedButton(true);
                    previousStruct = currentStruct;
                }
            }
            else if(hit.collider.CompareTag("StructButton"))
            {
                InterfaceStructButton button = hit.collider.gameObject.GetComponent<InterfaceStructButton>();
                button.ListenerButton();
            }
    }

    public StructBase GetCurrentStructure() => currentStruct;
}
