using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float live;
    [SerializeField] private float speed;  // Velocidad de movimiento
    private float offset;
    private Point nextPoint;
    private Vector2 target;
    private Vector2 direction;
    private void Awake()
    {
        int option = Random.Range(0, 3);
        offset = (option - 1) * 0.5f; // -0.5 para izquierda, 0 para centro, 0.5 para derecha
        enabled = false;
    }

    void Update()
    {
        // Mueve el enemigo hacia el objetivo (target) utilizando MoveTowards
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // Comprueba si se ha llegado al punto de destino
        if (Vector2.Distance(transform.position, target) < 0.1f)
        {
            // Cambia al siguiente punto y calcula el nuevo target
            nextPoint = nextPoint.NexPoint();

            if (nextPoint == null)
            {
                Destroy(gameObject); // Destruye al enemigo al final del recorrido
            }
            else
            {
                // Calcula la dirección hacia el siguiente punto
                direction = (nextPoint.GetPos() - (Vector2)transform.position).normalized;

                // Calcula el vector perpendicular a la dirección actual y aplica el offset
                Vector2 perpendicular = new Vector2(-direction.y, direction.x);
                target = nextPoint.GetPos() + perpendicular * offset;
            }
        }
    }

    public void SetNextPoint(Point nextPoint)
    {
        this.nextPoint = nextPoint;
        enabled = true;

        // Calcula el primer target cuando se establece el primer punto
        direction = (nextPoint.GetPos() - (Vector2)transform.position).normalized;
        Vector2 perpendicular = new Vector2(-direction.y, direction.x);
        target = nextPoint.GetPos() + perpendicular * offset;
    }

    public float GetVelocity() => speed;

    public Vector2 GetNextDestination() => nextPoint.GetPos();

    public void setLive(float cant)
    {
        live += cant;
        if(live <= 0)
            Destroy(gameObject);
    } 

    public void Move(bool state)
    {
        enabled = state;
    }
}
