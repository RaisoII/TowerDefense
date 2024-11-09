using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    [SerializeField] private Point nextPoint;
    [SerializeField] private float timeMinBetweenEnemy,timeMaxBetweenEnemy;
    
    public List<GameObject> SpawnFunctionEnemies(List<GameObject> listEnemies)
    {
        List<GameObject> listPrefabs = new List<GameObject>();
        foreach(GameObject enemy in listEnemies)
        {
            GameObject enemyInstantiate = Instantiate(enemy,transform.position,Quaternion.identity);
            listPrefabs.Add(enemyInstantiate);
        }

        StartCoroutine(enabledRutine(listPrefabs));
        return listPrefabs;
    } 
    private IEnumerator enabledRutine(List<GameObject> listEnemies)
    {
        float timeSpawn;
        
        foreach(GameObject enemy in listEnemies)
        {
            Enemy enem = enemy.GetComponent<Enemy>();
            enem.setNextPoint(nextPoint); 
            timeSpawn = Random.Range(timeMinBetweenEnemy,timeMaxBetweenEnemy);
            yield return new WaitForSeconds(timeSpawn);
        }
    }

}
