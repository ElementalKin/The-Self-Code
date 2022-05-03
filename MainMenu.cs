using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public UnityEngine.GameObject mainMenuUI;
    public UnityEngine.GameObject optionsMenuUI;
    public UnityEngine.GameObject creditsMenuUI;

    private void Start()
    {
        //mainMenuUI.SetActive(true);
        //optionsMenuUI.SetActive(false);
        //creditsMenuUI.SetActive(false);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Options()
    {
        mainMenuUI.SetActive(false);
        optionsMenuUI.SetActive(true);
    }

    public void Credits()
    {
        mainMenuUI.SetActive(false);
        creditsMenuUI.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }

    public void OptionsBackButton()
    {
        optionsMenuUI.SetActive(false);
        mainMenuUI.SetActive(true);
    }

    public void CreditsBackButton()
    {
        creditsMenuUI.SetActive(false);
        mainMenuUI.SetActive(true);
    }
}
