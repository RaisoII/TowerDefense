using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private SpawnEnemies spawnEnemies;
    [SerializeField] List<Wave> waves;
    [SerializeField] private TextMeshProUGUI textWave,textSecondsWave;

    private void Start()
    {
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
            
            while(true)
            {
                yield return new WaitForSeconds(1);
                time--;
                textSecondsWave.text =""+time+" s";
                if(time == 0)
                    break;
                
            }

            lastEnemies = spawnEnemies.SpawnFunctionEnemies(wave.enemies);
            
            if(wave.music != null)
                SoundManager.instance.playMusic(wave.music,true);
        }

        textWave.enabled = false;
        textSecondsWave.enabled = false;
        gameManager.checkVictorious(lastEnemies);
    }
}
