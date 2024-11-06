using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDestinySoldier : MonoBehaviour, InterfaceStructButton
{
    [SerializeField] private StructBase estructure;
    [SerializeField] private GameObject visualRange;
    private ClickCheckDestiny clickDestiny;
    private ClickTerrain clickTerrain;

    private void Awake()
    {
        clickDestiny =  GameObject.Find("scriptsGenerales").GetComponent<ClickCheckDestiny>();
        clickTerrain =  GameObject.Find("scriptsGenerales").GetComponent<ClickTerrain>();
    }

    public void ListenerButton()
    {
        visualRange.SetActive(true);
        estructure.ActivatedButton(false);
        clickTerrain.ResetStructures();
        clickTerrain.enabled = false;
        clickDestiny.setParametres(this,estructure.gameObject.transform.position, estructure.GetRange());
        clickDestiny.enabled = true;
    }
    public void validPointDestination(Vector2 pos)
    {
        visualRange.SetActive(false);
        estructure.MoveSoldiers(pos);
    }
    
    public void SetEnabledButton(bool state)
    {
        
    }
}
