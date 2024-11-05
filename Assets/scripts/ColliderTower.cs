using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTower : MonoBehaviour
{
    [SerializeField] private StructTower structTower;

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.CompareTag("Enemy"))
            structTower.detectedEnemy(coll);
    }
}
