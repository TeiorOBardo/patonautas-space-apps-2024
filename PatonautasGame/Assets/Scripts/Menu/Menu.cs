using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject credits;
    public GameObject patonautasCube;
    public GameObject defaultMenu;
    public GameObject controls;

    void Start()
    {
        credits.SetActive(false);
        patonautasCube.SetActive(false);
        defaultMenu.SetActive(true);
        controls.SetActive(false);
    }

    public void OnCreditsPressed()
    {
        credits.SetActive(true);
        patonautasCube.SetActive(true);
        defaultMenu.SetActive(false);
        controls.SetActive(false);
    }

    public void OnBackPressed()
    {
        credits.SetActive(false);
        patonautasCube.SetActive(false);
        defaultMenu.SetActive(true);
        controls.SetActive(false);
    }

    public void OnControlsPressed()
    {
        credits.SetActive(false);
        patonautasCube.SetActive(false);
        defaultMenu.SetActive(false);
        controls.SetActive(true);
    }

    public void OnPlayPressed()
    {
        SceneManager.LoadScene(1);
    }

    public void OnQuitGamePressed()
    {
        Application.Quit();
    }
}
