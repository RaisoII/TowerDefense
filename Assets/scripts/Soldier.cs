using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    [SerializeField] private float speed;
    private Vector2 target;
    
    private void Awake()
    {
        enabled = false;
    }
    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        
        // Comprueba si se ha llegado al punto de destino
        if (Vector2.Distance(transform.position, target) < 0.1f)
        {
            transform.position = target;
            enabled = false;
        }
    }

    public void SetTarget(Vector2 posTarget)
    {
        target = posTarget;
        enabled = true;
    }
}
