using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolicShotAlternative : MonoBehaviour
{
    public Vector2 ini;
    public Vector2 fin;
    public float height = 2.0f;  // Altura máxima de la parábola
    public float speed = 1.0f;   // Velocidad del movimiento

    private float time;
   // Establecemos el movimiento del proyectil
    public void SetMovement(Vector2 start, GameObject currentEnemy)
    {
         // Obtenemos la posición del enemigo y su velocidad
        Vector2 enemyPosition = currentEnemy.transform.position;
        Vector2 targetPosition = currentEnemy.GetComponent<Enemy>().getNextDestination();  // Asumiendo que tienes el objetivo del enemigo
        float enemySpeed = currentEnemy.GetComponent<Enemy>().getVelocity();  // La velocidad del enemigo

        // Calculamos la dirección del enemigo (normalizada)
        Vector2 direction = (targetPosition - enemyPosition).normalized;

        // Estimamos el tiempo que el proyectil tardará en llegar al objetivo
        float projectileSpeed = 10f; // Ajusta la velocidad del proyectil según sea necesario
        float distanceToTarget = Vector2.Distance(start, targetPosition);
        float timeToReachTarget = distanceToTarget / projectileSpeed;

        // Calculamos la posición predicha del enemigo en el futuro
        Vector2 predictedEnemyPosition = enemyPosition + direction * enemySpeed * timeToReachTarget;

        // Establecemos el final (end) del movimiento del proyectil
        Vector2 end = predictedEnemyPosition;
        fin = end;
        time = 0; // Reiniciamos el tiempo para cada nuevo disparo
    }

    private void Update()
    {
        time += Time.deltaTime * speed;

        // Interpolación lineal de 0 a 1
        float progress = Mathf.Clamp01(time);

        // Interpolación de posición en línea recta entre el inicio y el fin
        Vector2 pos = Vector2.Lerp(ini, fin, progress);

        // Añadimos el efecto de parábola utilizando una función cuadrática
        float parabola = 4 * height * (progress - progress * progress); // Fórmula para la parábola
        pos.y += parabola;

        // Actualizamos la posición del proyectil
        transform.position = pos;

        // Destruir el proyectil o finalizar la animación al llegar al destino
        if (progress >= 1.0f)
        {
            Destroy(gameObject);  // O puedes hacer algo diferente al final del disparo
        }
    }
}

