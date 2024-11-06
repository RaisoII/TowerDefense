using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickCheckDestiny : MonoBehaviour
{
    [SerializeField] private ClickTerrain clickTerrain;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private GameObject terrainInvalid;
    private float range;
    private ButtonDestinySoldier button;
    private Vector2 origen;
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
            CheckDestination();
    }

    private void CheckDestination()
    {

        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float distance = Vector2.Distance(mouseWorldPosition, origen);

        if(distance > range)
        {
            Instantiate(terrainInvalid,mouseWorldPosition,Quaternion.identity);
            return;
        }
        
        bool walkable = gridManager.IsWalkable(mouseWorldPosition);
        
        if(walkable)
        {
            button.validPointDestination(mouseWorldPosition);
            clickTerrain.enabled = true;
            enabled = false;
        }
        else
            Instantiate(terrainInvalid,mouseWorldPosition,Quaternion.identity);
        
    }

    public void setParametres(ButtonDestinySoldier button,Vector2 origen, float range)
    {
        this.button = button;
        this.range = range;
        this.origen = origen;
    }
}
