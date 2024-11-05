using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave 
{
    [Tooltip("Tiempo de espera antes de iniciar esta oleada (en segundos)")]
    public float timeBeforeWave;

    [Tooltip("Lista de enemigos que se spawnearán en esta oleada")]
    public List<GameObject> enemies;
}
