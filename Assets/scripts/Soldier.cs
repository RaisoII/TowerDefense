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
    private Vector2 targetPosition;
    private List<GameObject> listEnemies;
    private GameObject currentEnemy;
    
    private enum SoldierState { MovingToGuard, Guarding, MovingToEnemy, Attacking, Returning }
    private SoldierState currentState;
    
    private void Awake()
    {
        listEnemies = new List<GameObject>();
        currentState = SoldierState.MovingToGuard;
        enabled = false;
    }
    private void Update()
    {
        switch (currentState)
        {
            case SoldierState.MovingToGuard:
                GoToGuard();
                break;
            case SoldierState.MovingToEnemy:
                GoToEnemy();
                break;
        }
    }

    private void GoToGuard()
    {
        MoveToTarget();
        
        if((Vector2) transform.position == targetPosition)
        {
            currentState = SoldierState.Guarding;
            enabled = false;
        }
    }

    private void GoToEnemy()
    {
        float distance = Vector2.Distance(targetPosition, (Vector2)transform.position);
        if(distance > attackRange)
        {
            MoveToTarget();
        }
        else
        {
            currentState = SoldierState.Attacking;
            enabled = false;
        }
    }

    private void MoveToTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        
        // Comprueba si se ha llegado al punto de destino
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
            transform.position = targetPosition;
    }

    public void SetInitialPos(Vector2 posTarget)
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
        
        listEnemies.Add(coll.gameObject);

        if (currentState == SoldierState.Guarding)
        {
            currentEnemy = coll.gameObject;
            currentEnemy.GetComponent<Enemy>().Move(false);
            targetPosition = currentEnemy.transform.position;
            currentState = SoldierState.MovingToEnemy;
            enabled = true;
        }
    }
}
