using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
}

    public void Exit()
    {
        Application.Quit();
        Debug.Log("Exit");
    }
    
    public void Restart()
    {
        print("Restart");
        Application.LoadLevel(Application.loadedLevel);
    }
    
}
