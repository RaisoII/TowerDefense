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
    [SerializeField] private AudioClip arrowShoot;
    private Vector2 posShot;
    private List<GameObject> enemies;
    private GameObject currentEnemy;
    private bool rutineRun;
    private Vector2 nextPoint;

    protected override void Start()
    {
        base.Start();
        calculateNextPoint();
        rutineRun = false;
        enemies = new List<GameObject>();
        posShot = new Vector2(transform.position.x,transform.position.y + heightShot);
    }

    private void calculateNextPoint()
    {
        GameObject lastPoint =  GameObject.Find("scriptsGenerales").GetComponent<GameManager>().getLastWayPoint();
        nextPoint = lastPoint.transform.position;
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
            currentEnemy = searchFirstEnemy();
            
            if(currentEnemy == null)
            {
                rutineRun = false;
                break;
            }

            float distance = Vector2.Distance(transform.position,currentEnemy.transform.position);
            
            if(distance > range + deltaRange)
            {
                enemies.Remove(currentEnemy);
                currentEnemy = null;
                continue;
            }

            SoundManager.instance.playSFX(arrowShoot,false);
            GameObject instanceProjectile = Instantiate(projectile,posShot,Quaternion.identity);
            ParabolicShotDefinitive parabolicShot = instanceProjectile.GetComponent<ParabolicShotDefinitive>();
            parabolicShot.SetMovement(posShot,currentEnemy,damage);
            yield return new WaitForSeconds(cadence);
        }
    }

    private GameObject searchFirstEnemy()
    {
        enemies.RemoveAll(item => item == null);
        
        if(enemies.Count == 0)
            return null;
        
        if(enemies.Count == 1)
            return enemies[0];
        
        GameObject closestEnemy = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(nextPoint, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
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
    public override void selectSoldier(bool state){}
}
