using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructTower : StructBase
{
    [SerializeField] private float range,deltaRange;
    [SerializeField] private float cadence;
    [SerializeField] private GameObject projectile;
    [SerializeField] private float heightShot;
    [SerializeField] private float damage;
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

    public override void DestroyStructure()
    {
        parent.DestroyStructure(cost);
        Destroy(gameObject);
    }

    private IEnumerator Atack()
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
                    currentEnemy = enemies[0];
            }

            float distance = Vector2.Distance(transform.position,currentEnemy.transform.position);
            if(distance > range + deltaRange)
            {
                currentEnemy = null;
                continue;
            }

            GameObject instanceProjectile = Instantiate(projectile,posShot,Quaternion.identity);
            ParabolicShot parabolicShot = instanceProjectile.GetComponent<ParabolicShot>();
            parabolicShot.SetMovement(posShot,currentEnemy,damage);
            yield return new WaitForSeconds(cadence);
        }
    }

    public void detectedEnemy(Collider2D coll)
    {
        enemies.Add(coll.gameObject);
        
        if(!rutineRun)
        {
            currentEnemy = coll.gameObject;
            StartCoroutine(Atack());
            rutineRun = true;
        }
    }
}
