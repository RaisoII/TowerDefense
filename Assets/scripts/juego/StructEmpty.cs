using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructEmpty : StructBase
{

    public void CreateStructure(int index)
    {
        SoundManager.instance.playSFX(clipConstruction,false);
        gameObject.GetComponent<EdgeCollider2D>().enabled = false;
        GameObject structure = prefabsStructures[index];

        GameObject prefabInstantiate = Instantiate(structure,transform.position,Quaternion.identity);
        StructBase structBase = prefabInstantiate.GetComponent<StructBase>();
        structBase.setParent(this);
        moneyManager.setCantMoney(-structBase.getCost());
        activatedButton(false);
    }
    public override void  destroyStructure(){}
    
    public override void moveSoldiers(Vector2 pos){}

    public void DestroyStructure(int cost)
    {
        SoundManager.instance.playSFX(clipDestruction,false);
        gameObject.GetComponent<EdgeCollider2D>().enabled = true;
        moneyManager.setCantMoney(Mathf.RoundToInt(50*cost/100f)); // 50% de reingresos del costo original
    }
}
