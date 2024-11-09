using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statue : MonoBehaviour
{
    [SerializeField] private int cantLife;
    private GameManager gameManager;
    private void Start()
    {
        gameManager = GameObject.Find("scriptsGenerales").GetComponent<GameManager>();
        gameManager.setLife(cantLife);
    } 
    
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.CompareTag("Enemy"))
        {
            cantLife--;
            gameManager.setNewValueLife();
        }
    }
}
