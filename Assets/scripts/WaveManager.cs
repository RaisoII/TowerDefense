using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private SpawnEnemies spawnEnemies;
    [SerializeField] List<Wave> waves;

    private void Start()
    {
        StartCoroutine(WaveRutine());
    }

    private IEnumerator WaveRutine()
    {  
        foreach (var wave in waves)
        {
            yield return new WaitForSeconds(wave.timeBeforeWave);
            spawnEnemies.SpawnFunctionEnemies(wave.enemies);
        }
    }
}
