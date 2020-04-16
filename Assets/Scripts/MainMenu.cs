using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static int mainSelect = 1;

    public void PlayLoadGame()
    {
        mainSelect = 2;
        SceneManager.LoadScene("PlayScene");
    }

    public void PlayNewGame()
    {
        mainSelect = 3;
        SceneManager.LoadScene("PlayScene");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
