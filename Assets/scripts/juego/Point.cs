using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    [SerializeField] private Point nextPoint;
    private Vector2 pos;
    private void Start()
    {
        pos = transform.position;
    }
    public Point NexPoint() => nextPoint;

    public Vector2 GetPos() => pos;
}
