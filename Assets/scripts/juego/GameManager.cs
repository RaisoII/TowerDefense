using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textLife;
    [SerializeField] private GameObject textLose,textWin;
    private List<GameObject> lastEnemies;

    public void setNewValueLife()
    {
        int value = int.Parse(textLife.text);
        value--;
        textLife.text = ""+value;
        
        if(value == 0)
        {
            textLose.SetActive(true);
            StartCoroutine(returnRutine());
        }
    }

    public void checkVictorious(List<GameObject> enemies) => StartCoroutine(winRutine(enemies));

    private IEnumerator winRutine(List<GameObject> enemies)
    {
        lastEnemies = enemies;

        bool enemyLive;
        while(true)
        {
            enemyLive = false;
            
            foreach(GameObject enemy in  enemies)
            {
                if(enemy != null)
                {
                    enemyLive = true;
                    break;
                }
            }

            if(!enemyLive)
            {
                textWin.SetActive(true);
                StartCoroutine(returnRutine());
                break;
            }
            
            yield return new WaitForSeconds(5);
        }
    }

    private IEnumerator returnRutine()
    {
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene("Menu");
    }

    public void setLife(int value) => textLife.text = ""+value;
    
}
