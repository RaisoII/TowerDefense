using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructEmpty : StructBase
{
    [SerializeField] GameObject prefabFirstStructure,prefabSecondStructure; 
    private int costFirstStructure,costSecondStructure;
    
    private void Start()
    {
            
    }
    public void CreateStructure(int index)
    {
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        GameObject structure;
        if(index == 0)
            structure = prefabFirstStructure;
        else
            structure = prefabSecondStructure;

        GameObject prefabStructure = Instantiate(structure,transform.position,Quaternion.identity);
        prefabStructure.GetComponent<StructBase>().SetParent(gameObject);
        ActivatedButton(false);
    }
}
