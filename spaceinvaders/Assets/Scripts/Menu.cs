using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{

    public void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void LoadEndlessGameScene()
    {
        SceneManager.LoadScene("EndlessGameScene");
        Gamecontroller.activateEndlessGame();
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void QuitGame()//Quits Game
    {
        Application.Quit();
    }
}