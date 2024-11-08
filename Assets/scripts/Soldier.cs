using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float attackRange;
    [SerializeField] private float guardRange;
    [SerializeField] private int attackPower;
    private Vector2 guardPosition;
    private Soldier [] fellows;
    private Vector2 targetPosition;
    private List<GameObject> listEnemies;
    private GameObject currentEnemy;
    
    private enum SoldierState { MovingToGuard, Guarding, MovingToEnemy, Attacking, Returning }
    private SoldierState currentState;
    
    private void Awake()
    {
        fellows = new Soldier[2];
        listEnemies = new List<GameObject>();
        currentState = SoldierState.MovingToGuard;
        enabled = false;
    }
    private void Update()
    {
        switch (currentState)
        {
            case SoldierState.MovingToGuard:
                goToGuard();
                break;
            case SoldierState.MovingToEnemy:
                goToEnemy();
                break;
        }
    }

    private void goToGuard()
    {
        moveToTarget();
        
        if((Vector2) transform.position == targetPosition)
        {
            currentState = SoldierState.Guarding;
            enabled = false;

            if(listEnemies.Count > 0)
            {
                checkListEnemies();
                stopEnemy(currentEnemy);
                currentState = SoldierState.MovingToEnemy;
                enabled = true;
            }
        }
    }

    private void goToEnemy()
    {
        float distance = Vector2.Distance(targetPosition, (Vector2)transform.position);
        if(distance > attackRange)
        {
            moveToTarget();
        }
        else
        {
            currentState = SoldierState.Attacking;
            enabled = false;
        }
    }

    private void moveToTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        
        // Comprueba si se ha llegado al punto de destino
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
            transform.position = targetPosition;
    }

    public void setInitialPos(Vector2 posTarget)
    {
        if(currentEnemy != null)
        {
            currentEnemy.GetComponent<Enemy>().Move(true);
        }

        targetPosition = posTarget;
        currentState = SoldierState.MovingToGuard;
        enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(!coll.gameObject.CompareTag("Enemy"))
            return;
        
        if(!listEnemies.Contains(coll.gameObject))
            listEnemies.Add(coll.gameObject);

        if (currentState == SoldierState.Guarding)
        {
            stopEnemy(coll.gameObject);
            notifyColleagues(coll.gameObject);
        }
        else if(currentState == SoldierState.Attacking ||currentState == SoldierState.MovingToEnemy)
            checkNewEnemy(coll.gameObject);
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        Debug.Log("entra pasad");

        if(!coll.gameObject.CompareTag("Enemy"))
            return;
        
        if(listEnemies.Contains(coll.gameObject))
            listEnemies.Remove(coll.gameObject);
    }

    private void notifyColleagues(GameObject newEnemy)
    {
        foreach (Soldier s in fellows)
        {
            if(s != null)
                s.addNewEnemy(newEnemy);
        }
    }

    public void addNewEnemy(GameObject newEnemy)
    {
        if(!listEnemies.Contains(newEnemy))
        {
            listEnemies.Add(newEnemy);
            if(currentState == SoldierState.Guarding)
                stopEnemy(newEnemy);
        }
    }

    private void checkNewEnemy( GameObject enemy)
    {
        bool goToEnemy  = true;
        bool fellowsAtakingCurrentEnemy = false;
        
        // chequeo si soy el unico atacando este sujeto
        foreach(Soldier s in fellows)
        {
            if(s != null)
            {
                GameObject enemyFellow =  s.getCurrentEnemy();
                if(currentEnemy == enemyFellow)
                {
                    fellowsAtakingCurrentEnemy = true;
                    break;
                }
            }
        }
        if(!fellowsAtakingCurrentEnemy)
            return;

        //chequeo si nadie está yendo hacia el nuevo enemigo
        foreach(Soldier s in fellows)
        {
            if(s != null)
            {
                GameObject enemyFellow =  s.getCurrentEnemy();
                if(enemyFellow == enemy)
                {
                    goToEnemy = false;
                    break;
                }
            }
        }

        if(goToEnemy)
            stopEnemy(enemy);
    }

    public void checkListEnemies()
    {
        List<GameObject> listUnattackedEnemy = new List<GameObject>();

        foreach(GameObject enemy in listEnemies)
        {
            bool fellowsAtakingThisEnemy = false;
            foreach(Soldier s in fellows)
            {
                if(s != null)
                {
                    GameObject enemyFellow =  s.getCurrentEnemy();
                    
                    if(enemy == enemyFellow)
                    {
                        fellowsAtakingThisEnemy = true;
                        break;
                    }
                }
            }

            if(!fellowsAtakingThisEnemy)
                listUnattackedEnemy.Add(enemy);
        }
        Debug.Log(listUnattackedEnemy.Count);
        if(listUnattackedEnemy.Count == 0)
            currentEnemy = listEnemies[0];
        else
            currentEnemy = listUnattackedEnemy[Random.Range(0,listUnattackedEnemy.Count)];
    }

    private void stopEnemy(GameObject enemy)
    {   
        currentEnemy = enemy;
        currentEnemy.GetComponent<Enemy>().Move(false);
        targetPosition = currentEnemy.transform.position;
        currentState = SoldierState.MovingToEnemy;
        enabled = true;
    }

    public void setFellow(GameObject fellow,int index)
    {
        Soldier s = fellow.GetComponent<Soldier>();
        fellows[index] = s;
    }

    public GameObject getCurrentEnemy() => currentEnemy; 
}
