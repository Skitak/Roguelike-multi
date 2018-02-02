﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GlobalHealthManager : MonoBehaviour {

    public int globalHealth;
    public Text globalHealthNumber;

    public GameObject panel;
    public GameObject gameOver;
    public GameObject quite;
    public GameObject replay;
 
    public Sprite notSelectedQuite;
    public Sprite selectedQuite;
    public Sprite notSelectedReplay;
    public Sprite selectedReplay;

    private int numb;

    // Use this for initialization
    void Start () {
        panel.gameObject.SetActive(false);
        gameOver.gameObject.SetActive(false);
        quite.gameObject.SetActive(false);
        replay.gameObject.SetActive(false);

        numb = 0;
    }
	
	// Update is called once per frame
	void Update () {
        globalHealthNumber.text = "x" +globalHealth;
        
        var butT = UnityEngine.Input.GetButtonDown("Keyboard 2 left-selection"); // Bouton Q pour gauche
        var butG = UnityEngine.Input.GetButtonDown("Keyboard 2 right-selection"); // Bouton D pour droite
        var butB = UnityEngine.Input.GetButtonDown("Keyboard 2 start"); // Bouton W pour selectionner

        if (globalHealth == 0)
        {
            
            panel.gameObject.SetActive(true);
            gameOver.gameObject.SetActive(true);
            quite.gameObject.SetActive(true);
            replay.gameObject.SetActive(true);

            Time.timeScale = 0.0f;

            if (butT) // gauche
            {
                numb = 0;
                changeSprite(numb);
            }
            if (butG) // droite
            {
                numb = 1;
                changeSprite(numb);
            }

            if (numb == 0 && butB) // Selection de play
            {
                SceneManager.LoadScene("BastienTest");
                Time.timeScale = 1.0f;

            }
            if (numb == 1 && butB) // Selection de quite
            {
                SceneManager.LoadScene("Menu");
                Time.timeScale = 1.0f;
            }
            
        }
    }
    
    public void changeSprite(int i)
    {
        if (i == 0)
        {
            quite.GetComponent<UnityEngine.UI.Image>().overrideSprite = notSelectedQuite;
            replay.GetComponent<UnityEngine.UI.Image>().overrideSprite = selectedReplay;
        }

        if (i == 1)
        {
            replay.GetComponent<UnityEngine.UI.Image>().overrideSprite = notSelectedReplay;
            quite.GetComponent<UnityEngine.UI.Image>().overrideSprite = selectedQuite;
        }
    }
}
