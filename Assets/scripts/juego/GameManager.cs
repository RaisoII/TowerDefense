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
    [SerializeField] private AudioClip SFXloopClip, startMusic,winMusic,loseMusic;
    [SerializeField] private float timeReturnWin,timeReturnLose;
    [SerializeField] GameObject lastWayPoint;
    private List<GameObject> lastEnemies;


    private void Start()
    {
        SoundManager.instance.changedMusic(startMusic,true,false);
        SoundManager.instance.playSFX(SFXloopClip,true);
    }
    public void setNewValueLife()
    {
        int value = int.Parse(textLife.text);
        value--;
        
        if(value >= 0)
            textLife.text = ""+value;
        
        if(value == 0)
        {
            SoundManager.instance.changedMusic(winMusic,false,false);
            SoundManager.instance.fadeOutSFXLoop();
            textLose.SetActive(true);
            StartCoroutine(returnRutine(false));
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
            
            foreach(GameObject enemy in  lastEnemies)
            {
                if(enemy != null)
                {
                    enemyLive = true;
                    break;
                }
            }

            if(!enemyLive)
            {
                SoundManager.instance.fadeOutSFXLoop();
                SoundManager.instance.changedMusic(winMusic,false,false);
                textWin.SetActive(true);
                StartCoroutine(returnRutine(true));
                break;
            }
            
            yield return new WaitForSeconds(5);
        }
    }

    private IEnumerator returnRutine(bool isWin)
    {
        if(isWin)
            yield return new WaitForSeconds(timeReturnWin);
        else
            yield return new WaitForSeconds(timeReturnLose);
        SceneManager.LoadScene("Menu");
    }

    public void setLife(int value) => textLife.text = ""+value;
    public GameObject getLastWayPoint() =>  lastWayPoint;
    
}
