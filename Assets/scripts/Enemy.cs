using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float live;
    [SerializeField] private float speed;  // Velocidad de movimiento
    private Point nextPoint;
    private void Awake()
    {
        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (nextPoint != null)
        {
            // Calcula la nueva posición utilizando MoveTowards
            transform.position = Vector2.MoveTowards(transform.position, nextPoint.GetPos(), speed * Time.deltaTime);

            // Comprueba si se ha llegado al punto de destino
            if (Vector2.Distance(transform.position, nextPoint.GetPos()) < 0.1f)
            {
                nextPoint = nextPoint.NexPoint();
                if(nextPoint == null)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    public void SetNextPoint(Point nextPoint)
    {
        this.nextPoint = nextPoint;
        enabled = true;
    } 
    public float GetVelocity() => speed;

    public Vector2 GetNextDestination() => nextPoint.GetPos();

    public void setLive(float cant)
    {
        live += cant;
        if(live <= 0)
            Destroy(gameObject);
    } 
}
