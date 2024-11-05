using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolicShot : MonoBehaviour
{
    private Vector2 targetPos;

    [Tooltip("Horizontal speed, in units/sec")]
    public float speed = 5;

    [Tooltip("How high the arc should be, in units")]
    public float arcHeight = 2;
    Vector2 startPos;
    Vector2 nextPos;
    private float damage;
    private float timeImpact;
    void Awake()
    {   
        startPos = transform.position;
    }   

    void Update()
    {
            // Compute the next position, with arc added in
            float x0 = startPos.x;
            float x1 = targetPos.x;
            float dist = x1 - x0;
            float nextX = Mathf.MoveTowards(transform.position.x, x1, speed * Time.deltaTime);
            float baseY = Mathf.Lerp(startPos.y, targetPos.y, (nextX - x0) / dist);
            float arc = arcHeight * (nextX - x0) * (nextX - x1) / (-0.25f * dist * dist);
            
            nextPos = new Vector3(nextX, baseY + arc, transform.position.z);
            
            // Rotate to face the next position, and then move there
        
            transform.rotation = LookAt2D(nextPos - (Vector2)transform.position);
            transform.position = nextPos;

            // Do something when we reach the target
            if (nextPos == targetPos)
            {
                gameObject.GetComponent<BoxCollider2D>().enabled = true;
                enabled = false;
            }
    }

    /// 
    /// This is a 2D version of Quaternion.LookAt; it returns a quaternion
    /// that makes the local +X axis point in the given forward direction.
    /// 
    /// forward direction
    /// Quaternion that rotates +X to align with forward
    static Quaternion LookAt2D(Vector2 forward)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg);
    }

    public void SetMovement(Vector2 start, GameObject enemy,float damage)
    {
        this.damage = damage;
        Enemy enemyTipe = enemy.GetComponent<Enemy>();
        Vector2 enemyDirection = SearchDirection(enemyTipe);
        Vector2 posEnemy = (Vector2)enemy.transform.position;
        float enemyVelocity = enemyTipe.GetVelocity();
        
        Vector2 OriginalDistance = posEnemy - start;
        Vector2 relativeVelocity = enemyDirection * enemyTipe.GetVelocity() - OriginalDistance.normalized * speed;

        float timeImpact = OriginalDistance.magnitude / relativeVelocity.magnitude;

        Vector2 posImpact = posEnemy + enemyDirection * enemyVelocity * timeImpact;

        targetPos = posImpact;
        //speed = (targetPos - (Vector2)transform.position).magnitude;
    }

    private Vector2 SearchDirection(Enemy enemy)
    {
        Vector2 nextDestination = enemy.GetNextDestination();
        Vector2 direction = (nextDestination - (Vector2)enemy.transform.position).normalized;
        return direction;
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.CompareTag("Enemy"))
        {
            Enemy enemyTipe = coll.gameObject.GetComponent<Enemy>();
            enemyTipe.setLive(-damage);
            Destroy(gameObject);
        }
    }

}
