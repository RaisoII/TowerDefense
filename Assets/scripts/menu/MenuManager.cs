using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private AudioClip music;
    [SerializeField] private GameObject menuButtons,optionButtons;
    [SerializeField] private Slider sliderMusic, sliderSound;
    [SerializeField] private AudioClip checkClip;

    private void Start()
    {
        sliderMusic.value = SoundManager.instance.getMusicVolume() * 100;
        sliderSound.value = SoundManager.instance.getSFXvolume() * 100;
        StartCoroutine(waitingSecond());
    }
    private IEnumerator waitingSecond()
    {
        yield return new WaitForSeconds(1);

        if(!SoundManager.instance.isPlay()) // para cuando vuelve al menu, hay veces donde se esta reproduciendo musica ya.
            SoundManager.instance.playMusic(music,true,false);
    }
    public void loadEscene(string name) => SceneManager.LoadScene(name);

    public void OnSliderValueChanged(int index)
    {
        if(index == 0)
            SoundManager.instance.SetMusicVolume(sliderMusic.value / 100f);
        else
        {

            SoundManager.instance.setSFXVolume(sliderSound.value / 100f);
            SoundManager.instance.playSFX(checkClip,false);
        }
            
    } 

    public void changedPanel()
    {
        if(menuButtons.activeSelf)
        {
            menuButtons.SetActive(false);
            optionButtons.SetActive(true);
        }
        else
        {
            optionButtons.SetActive(false);
            menuButtons.SetActive(true);
        }
    }
}
