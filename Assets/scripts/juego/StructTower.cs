using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructTower : StructBase
{
    [SerializeField] private float deltaRange;
    [SerializeField] private float cadence;
    [SerializeField] private GameObject projectile;
    [SerializeField] private float heightShot;
    [SerializeField] private int damage;
    private Vector2 posShot;
    private List<GameObject> enemies;
    private GameObject currentEnemy;
    private bool rutineRun;

    protected override void Start()
    {
        base.Start();
        rutineRun = false;
        enemies = new List<GameObject>();
        posShot = new Vector2(transform.position.x,transform.position.y + heightShot);
    }

    public override void destroyStructure()
    {
        parent.DestroyStructure(cost);
        Destroy(gameObject);
    }

    private IEnumerator attack()
    {  
        while(true)
        {
            if(currentEnemy == null)
            {
                enemies.RemoveAt(0);
                if(enemies.Count == 0)
                {
                    rutineRun = false;
                    break;
                }                
                else
                {
                    currentEnemy = searchNewEnemy();
                    if(currentEnemy == null)
                        break;
                }
            }

            float distance = Vector2.Distance(transform.position,currentEnemy.transform.position);
            
            if(distance > range + deltaRange)
            {
                currentEnemy = null;
                continue;
            }

            GameObject instanceProjectile = Instantiate(projectile,posShot,Quaternion.identity);
            ParabolicShotDefinitive parabolicShot = instanceProjectile.GetComponent<ParabolicShotDefinitive>();
            parabolicShot.SetMovement(posShot,currentEnemy,damage);
            yield return new WaitForSeconds(cadence);
        }
    }

    private GameObject searchNewEnemy()
    {
        enemies.RemoveAll(item => item == null);
        if(enemies.Count == 0)
            return null;
        else
            return enemies[0];
    }

    public void detectedEnemy(Collider2D coll)
    {
        enemies.Add(coll.gameObject);
        
        if(!rutineRun)
        {
            currentEnemy = coll.gameObject;
            StartCoroutine(attack());
            rutineRun = true;
        }
    }
    
    public override void moveSoldiers(Vector2 pos){}
}
