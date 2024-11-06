using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructEmpty : StructBase
{

    public void CreateStructure(int index)
    {
        gameObject.GetComponent<EdgeCollider2D>().enabled = false;
        GameObject structure = prefabsStructures[index];
    
        GameObject prefabInstantiate = Instantiate(structure,transform.position,Quaternion.identity);
        StructBase structBase = prefabInstantiate.GetComponent<StructBase>();
        structBase.SetParent(this);
        moneyManager.SetCantMoney(-structBase.GetCost());
        ActivatedButton(false);
    }
    public override void  DestroyStructure(){}
    
    public override void MoveSoldiers(Vector2 pos){}

    public void DestroyStructure(int cost)
    {
        gameObject.GetComponent<EdgeCollider2D>().enabled = true;
        moneyManager.SetCantMoney(Mathf.RoundToInt(50*cost/100f)); // 50% de reingresos del costo original
    }
}
