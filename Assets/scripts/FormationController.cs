using UnityEngine;

public class FormationController : MonoBehaviour
{
    public Transform[] soldiers; // Array con los tres soldados
    private Vector3[] relativePositions; // Posiciones relativas de los soldados

    private void Start()
    {
        // Calculamos las posiciones relativas con respecto al primer soldado
        relativePositions = new Vector3[soldiers.Length];
        for (int i = 0; i < soldiers.Length; i++)
        {
            relativePositions[i] = soldiers[i].position - soldiers[0].position;
        }
    }

    public void MoveFormation(Vector3 targetPosition)
    {
        // Mueve cada soldado a su posición relativa respecto al objetivo
        for (int i = 0; i < soldiers.Length; i++)
        {
            Vector3 newPosition = targetPosition + relativePositions[i];
            // Podés usar Lerp o MoveTowards para suavizar el movimiento
            soldiers[i].position = newPosition;
        }
    }

    private void Update()
    {
        // Detectar clic y mover la formación
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPosition.z = 0f; // Asegurarnos de que esté en el plano 2D
            MoveFormation(targetPosition);
        }
    }
}

