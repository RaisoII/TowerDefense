using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    [SerializeField] private Point nextPoint;
    [SerializeField] private float timeMinBetweenEnemy,timeMaxBetweenEnemy;
    
    public void SpawnFunctionEnemies(List<GameObject> listEnemies) => StartCoroutine(SpawnRutine(listEnemies));

    private IEnumerator SpawnRutine(List<GameObject> listEnemies)
    {
        foreach(GameObject enemy in listEnemies)
        {
            float timeSpawn = Random.Range(timeMinBetweenEnemy,timeMaxBetweenEnemy);
            GameObject enemyInstantiate = Instantiate(enemy,transform.position,Quaternion.identity);
            enemyInstantiate.GetComponent<Enemy>().SetNextPoint(nextPoint); 
            yield return new WaitForSeconds(timeSpawn);
        }
    }
}
