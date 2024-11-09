using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructBarraks : StructBase
{
    [SerializeField] private GameObject soldier;
    [SerializeField] private int cantSoldier;
    [SerializeField] private List<GameObject> listSpot;
    [SerializeField] private float deltaBarrackRange;
    [SerializeField] private float timeRespawn;
    private GameObject[] soldiers;
    private Vector2[] formationOffsets;
    private Vector2[] startPos;
    private GridManager gridManager;
    private List<Vector2> posibleSpot;
    private Vector2 dir;
    private Vector2 finalPos;
    private bool respawnRunig;

    protected override void Start()
    {
        base.Start();

        respawnRunig = false;
        soldiers = new GameObject[3];
        formationOffsets = new Vector2[3];
        startPos = new Vector2[3];

        GameObject scripts = GameObject.Find("scriptsGenerales"); 
        gridManager = scripts.GetComponent<GridManager>();

        posibleSpot = new List<Vector2>();    
        
        foreach(GameObject spot in listSpot)
            checkPosibleSpot(spot.transform.position);
        
        if(posibleSpot.Count == 0)
            return;

        finalPos = posibleSpot[Random.Range(0,posibleSpot.Count)];
        dir = (finalPos - (Vector2)transform.position).normalized;
        
        spawnSoldier();
    }

    protected void spawnSoldier()
    {
        searchPositionSoldier();

        for(int i = 0; i < 3; i++) // 3 por ahora, no jodas
            instantiateSoldier(i);
    }

    private void instantiateSoldier(int index)
    {
        GameObject soldierInstantiate = Instantiate(soldier,transform.position,Quaternion.identity);
        Soldier sol = soldierInstantiate.GetComponent<Soldier>();
        sol.setInitialPos(startPos[index]);
        sol.setBarrakRange(range);
        sol.setDeltaBarrackRange(deltaBarrackRange);
        sol.setBarrak(this);
        soldiers[index] = soldierInstantiate;

        
        for(int i = 0; i < 3; i++) // todos los continue son necesarios ya que esta funcion se invoca no solo al instanicar la barraca
        {
            GameObject soldier = soldiers[i];
            
            if(soldier == null)
                continue;

            Soldier s = soldier.GetComponent<Soldier>();

            int indexFellow = 0;
            for(int j = 0 ; j < 3; j++)
            {
                
                if(j == i)
                    continue;

                GameObject soldierAux = soldiers[j];
                
                if(soldierAux == null)
                    continue;
                
                s.setFellow(soldierAux,indexFellow);
                indexFellow++;
            }
        }
    }

    protected void searchPositionSoldier()
    {

        if(dir == Vector2.up)
        {
            startPos[0] = finalPos + .1f*Vector2.up;
            startPos[1] = finalPos + .3f*Vector2.left + .3f*Vector2.down;
            startPos[2] = finalPos - .3f*Vector2.left + .3f*Vector2.down;

            formationOffsets[0] = .1f*Vector2.up;
            formationOffsets[1] = .3f*Vector2.left + .3f*Vector2.down;
            formationOffsets[2] = - .3f*Vector2.left + .3f*Vector2.down;
            
        }
        else if(dir == Vector2.down)
        {
            startPos[0] = finalPos + .1f*Vector2.down;
            startPos[1] = finalPos + .3f*Vector2.left + .3f*Vector2.up;
            startPos[2] = finalPos - .3f*Vector2.left + .3f*Vector2.up;

            
            formationOffsets[0] = .1f*Vector2.down;
            formationOffsets[1] = .3f*Vector2.left + .3f*Vector2.up;
            formationOffsets[2] = -.3f*Vector2.left + .3f*Vector2.up;
        }
        else if(dir == Vector2.right)
        {
            startPos[0] = finalPos + .1f*Vector2.right;
            startPos[1] = finalPos + .3f*Vector2.up + .3f*Vector2.left;
            startPos[2] = finalPos - .3f*Vector2.down + .3f*Vector2.left;

            
            formationOffsets[0] = .1f*Vector2.right;
            formationOffsets[1] = .3f*Vector2.up + .3f*Vector2.left;
            formationOffsets[2] = - .3f*Vector2.down + .3f*Vector2.left;
        }
        else
        {
            startPos[0] = finalPos + .1f*Vector2.left;
            startPos[1] = finalPos + .3f*Vector2.up + .3f*Vector2.right;
            startPos[2] = finalPos - .3f*Vector2.down + .3f*Vector2.right;

            
            formationOffsets[0] = .1f*Vector2.left;
            formationOffsets[1] = .3f*Vector2.up + .3f*Vector2.right;
            formationOffsets[2] = - .3f*Vector2.down + .3f*Vector2.right;
        }
    }

    private void checkPosibleSpot(Vector2 pos)
    {
        bool isWalkable = gridManager.IsWalkable(pos);
        
        if(isWalkable)
            posibleSpot.Add(pos);
    }

    public override void  destroyStructure()
    {
        foreach(GameObject soldier in soldiers)
            Destroy(soldier);
        
        parent.DestroyStructure(cost);
        Destroy(gameObject);
    }

    public override void moveSoldiers(Vector2 pos)
    {
        for(int i = 0; i < 3; i++)
        {
            GameObject soldier = soldiers[i];
            
            if(soldier == null)
                continue; 
            
            Soldier s = soldier.GetComponent<Soldier>();
            Vector2 finalPos = pos + formationOffsets[i];
            
            s.setInitialPos(finalPos);
        }
    }
    
    public void spawnNewSoldier()
    {
        if(!respawnRunig)
        {
            respawnRunig = true;
            StartCoroutine(respawnRutine());
        }
    } 

    private IEnumerator respawnRutine()
    {
        yield return new WaitForSeconds(timeRespawn);
        int posEmpty = 0;
        
        for(int i = 0; i < 3; i++)
        {
            if(soldiers[i] == null)
            {
                posEmpty = i;
                break;
            }
        }
        
        instantiateSoldier(posEmpty);

        bool newRespawn = false;
        foreach(GameObject soldier in soldiers)
        {
            if(soldier == null)
            {
                newRespawn = true;
                break;
            }
        }

        if(newRespawn)
            StartCoroutine(respawnRutine());
        else
            respawnRunig = false;
    }
}
