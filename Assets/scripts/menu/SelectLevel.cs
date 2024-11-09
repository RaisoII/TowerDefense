using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectLevel : MonoBehaviour
{
    public void selectEscene(string name)
    {
        SoundManager.instance.fadeOutMusic();
        SceneManager.LoadScene(name);
    } 
}
