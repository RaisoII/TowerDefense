using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] List<Wave> waves;
    [SerializeField] private TextMeshProUGUI textWave,textSecondsWave;
    [SerializeField] private Button nextWave;
    private bool skipWaveWait;

    private void Start()
    {
        skipWaveWait = false;
        StartCoroutine(waveRutine());
    }

    private IEnumerator waveRutine()
    {  
        List<GameObject> lastEnemies = null;
        
        foreach (var wave in waves)
        {   
            yield return new WaitForSeconds(2);

            float time = wave.timeBeforeWave;
            textSecondsWave.text =""+time+" s";
            nextWave.interactable = true;
            skipWaveWait = false;
            
            while(true)
            {
                yield return new WaitForSeconds(1);
                time--;
                textSecondsWave.text =""+time+" s";
                if(time == 0  || skipWaveWait)
                    break;
            }
            
            nextWave.interactable = false;
            lastEnemies = wave.spawn.GetComponent<SpawnEnemies>().SpawnFunctionEnemies(wave.enemies);
            
            if(wave.music != null)
                SoundManager.instance.playMusic(wave.music,true,wave.isLoop);
        }

        textWave.enabled = false;
        textSecondsWave.enabled = false;
        gameManager.checkVictorious(lastEnemies);
    }

    public void listenerNextWave()
    {
        skipWaveWait = true;
        nextWave.interactable = false;
    } 
}
