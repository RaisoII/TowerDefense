using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolicShotDefinitive : MonoBehaviour
{
    public Vector2 startPoint;      // Punto de inicio
    public Vector2 endPoint;        // Punto de destino
    public float flightTime = 2f;   // Tiempo total del tiro parabólico (2 segundos)
    private float elapsedTime = 0f; // Tiempo transcurrido
    public float heightFactor = 1f;
    private Enemy currentEnemy;
    private int cantDamage;
    private void Awake()
    {
        enabled = false;
    }

    void Update()
    {
        // Incrementar el tiempo transcurrido
        elapsedTime += Time.deltaTime;

        // Calcular el progreso normalizado del disparo (0 a 1 en 2 segundos)
        float t = Mathf.Clamp01(elapsedTime / flightTime);
        
        Vector2 position = CalculateParabolicPosition(startPoint, endPoint, t);
        transform.position = position;
        
        if(t < 1)
        {
            float angle = CalculateParabolicAngle(position,startPoint,endPoint,elapsedTime);
            transform.rotation = Quaternion.Euler(0,0,angle);
        }

        if (t >= 1f)
        {
            if(currentEnemy != null)
            {
                float distance = Vector2.Distance(transform.position,currentEnemy.gameObject.transform.position);
                if(distance < 0.2f)
                {
                    currentEnemy.setLive(-cantDamage);
                    Destroy(gameObject);
                }
                else
                    StartCoroutine(waitingFrame());
            }                
            else
                StartCoroutine(waitingFrame());
        }
    }

    private Vector2 CalculateParabolicPosition(Vector2 start, Vector2 end, float t)
    {
        float maxHeight = Mathf.Abs(end.y - start.y) * heightFactor;

        // Interpolación lineal para x y parábola más baja para y
        float x = Mathf.Lerp(start.x, end.x, t);
        float y = Mathf.Lerp(start.y, end.y, t) + maxHeight * 4 * t * (1 - t);

        return new Vector2(x, y);
    }

    public void SetMovement(Vector2 start, GameObject enemy,int damage)
    {
        cantDamage = damage;
        startPoint = start;
        currentEnemy = enemy.GetComponent<Enemy>();
        Vector2 currentPos = (Vector2)enemy.transform.position;
        Vector2 nextPos =  currentEnemy.getNextDestination();
        float velocity =  currentEnemy.getVelocity();
        
        Vector2 direction = (nextPos - currentPos).normalized;
        
        // Calcular la posición final considerando la velocidad y el tiempo de vuelo
        endPoint = currentPos + direction * velocity * flightTime;
        enabled = true;
    }

    private float CalculateParabolicAngle(Vector2 currentPosition, Vector2 start, Vector2 end, float t)
    {
        // Paso muy pequeño para calcular la posición ligeramente adelante
        float deltaT = 0.01f;
        float nextT = Mathf.Clamp01(t + deltaT);

        // Posición actual y siguiente en la parábola
        Vector2 nextPosition = CalculateParabolicPosition(start, end, nextT);

        // Dirección de la tangente
        Vector2 direction = nextPosition - currentPosition;

        // Calcular el ángulo en grados
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        return angle;
    }

    private IEnumerator waitingFrame()
    {
        yield return new WaitForSeconds(.2f);
        Destroy(gameObject);
    }
}
