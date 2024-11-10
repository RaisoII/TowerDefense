using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectLevel : MonoBehaviour
{
    public void selectEsceneFadeMusic(string name)
    {
        SoundManager.instance.fadeOutMusic();
        SceneManager.LoadScene(name);
    } 
    
    public void selectEscene(string name) => SceneManager.LoadScene(name);

}
