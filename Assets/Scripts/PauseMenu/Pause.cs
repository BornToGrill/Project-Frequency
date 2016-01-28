﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class Pause : MonoBehaviour {

    public GameObject PauseMenu;
    void Start()
    {
        PauseMenu.SetActive(false);
    }

    public void PauseMenuTrigger()
    {
        
        if (PauseMenu.activeSelf == false)
        {
         
            PauseMenu.SetActive(true);
        }
        else if(PauseMenu.activeSelf)
        {
            PauseMenu.SetActive(false);         
        }
    }
 
}
