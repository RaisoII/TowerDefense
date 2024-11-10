using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundManager : MonoBehaviour
{
    [Header("Volume Settings")]

    [Header("Audio Sources")]
    //public AudioSource musicSource;
    public AudioSource sfxSingle,sfxLoop,music;
    public static SoundManager instance;
    public event Action OnMusicEnd;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else 
            Destroy(gameObject);
    }

    // Función para reproducir música
    public void playMusic(AudioClip clip,bool fadeIn, bool isLoop)
    {
        music.clip = clip;
        music.loop = isLoop;
        music.Play();
        if(fadeIn)
            StartCoroutine(fadeInRutine());
    }

    public void changedMusic(AudioClip clip,bool isLoop,bool fadeIn) => StartCoroutine(changedMusicRutine(clip,isLoop,fadeIn));

    private IEnumerator changedMusicRutine(AudioClip clip,bool isLoop,bool fadeIn)
    {
        fadeOutMusic();
        yield return new WaitForSeconds(3);
        music.clip = clip;
        music.loop = isLoop;
        music.Play();
        if(fadeIn)
            StartCoroutine(fadeInRutine());
    }

    private IEnumerator fadeInRutine()
    {
        
        float targetVolume = music.volume;
        music.volume = 0f; // Volumen máximo
        float fadeDuration = 2f; // Duración del fade-in en segundos

        while (music.volume < targetVolume)
        {
            music.volume += Time.deltaTime / fadeDuration;
            yield return null;
        }

        music.volume = targetVolume; // Asegura que el volumen quede en el valor máximo
    }

    private IEnumerator waitingEndMusic()
    {
        while (music.isPlaying)
        {
            yield return new WaitForSeconds(10);
        }
        
        OnMusicEnd?.Invoke();
    }

    public void fadeOutMusic() => StartCoroutine(fadeOutRutine());

    private IEnumerator fadeOutRutine()
    {
        float duration = 3f; // Duración del desvanecimiento en segundos
        float initialVolume = music.volume;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            music.volume = Mathf.Lerp(initialVolume, 0, t / duration);
            yield return null;
        }

        music.Stop();   // Detiene la reproducción si es necesario
        music.volume = initialVolume;
    }

    // Función para reproducir efectos de sonido
    public void playSFX(AudioClip clip,bool loop)
    {
        if(!loop)
            sfxSingle.PlayOneShot(clip);
        else
        {
            sfxLoop.clip = clip;
            sfxLoop.Play();
        }
    }

    public void StopAudioLoop() => sfxLoop.Stop();

    // Función para cambiar el volumen de la música
    public void SetMusicVolume(float volume) => music.volume = volume;


    // Función para cambiar el volumen de los efectos de sonido
    public void setSFXVolume(float volume)
    {
        sfxLoop.volume = volume;
        sfxSingle.volume = volume;
    } 

    public void setMusicaVolumen(float volume) => music.volume = volume;
    public bool isPlay() => music.isPlaying;
}
