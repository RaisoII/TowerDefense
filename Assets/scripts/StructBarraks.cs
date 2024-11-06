using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructBarraks : StructBase
{
    [SerializeField] private GameObject soldier;
    [SerializeField] private int cantSoldier;
    [SerializeField] private List<GameObject> listSpot;
    private GridManager gridManager;
    private List<Vector2> posibleSpot;
    private Vector2 dir;
    private Vector2 finalPos;

    protected override void Start()
    {
        base.Start();

        GameObject scripts = GameObject.Find("scriptsGenerales"); 
        gridManager = scripts.GetComponent<GridManager>();

        posibleSpot = new List<Vector2>();    
        
        foreach(GameObject spot in listSpot)
            CheckPosibleSpot(spot.transform.position);
        
        if(posibleSpot.Count == 0)
            return;

        finalPos = posibleSpot[Random.Range(0,posibleSpot.Count)];
        dir = (finalPos - (Vector2)transform.position).normalized;
        
        spawnSoldier();
    }

    protected void spawnSoldier()
    {
        Vector2[] posArray = SearchPositionSoldier();
        int j = 0; // lleva el indice del arreglo si posArray < cantSoldier
        for(int i = 0; i < cantSoldier; i++)
        {
            GameObject soldierInstantiate = Instantiate(soldier,transform.position,Quaternion.identity);
            Soldier sol = soldierInstantiate.GetComponent<Soldier>();
            sol.SetTarget(posArray[j]);
            j++;
            if(j == posArray.Length)
                j = 0;
        }
    }

    protected Vector2[] SearchPositionSoldier()
    {
        Vector2[] posArray = new Vector2[3];
        posArray[0] = finalPos;
        
        if(dir == Vector2.up || dir == Vector2.down)
        {
            posArray[1] = finalPos + .5f*Vector2.left;
            posArray[2] = finalPos - .5f*Vector2.left;
        }
        else
        {
            posArray[1] = finalPos + .5f*Vector2.up;
            posArray[2] = finalPos - .5f*Vector2.down;
        }
        return posArray;
    }

    private void CheckPosibleSpot(Vector2 pos)
    {
        bool isWalkable = gridManager.IsWalkable(pos);
        
        if(isWalkable)
            posibleSpot.Add(pos);
    }

    public override void  DestroyStructure()
    {
        parent.DestroyStructure(cost);
        Destroy(gameObject);
    }
}
