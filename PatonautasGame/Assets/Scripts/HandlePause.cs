using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HandlePause : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject menu;
    public GameObject areYouSure;
    public GameObject background;
    public GameObject controls;
    void Start()
    {
        menu.SetActive(false);
        areYouSure.SetActive(false);
        background.SetActive(false);
        controls.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused)
            {
                Unpause();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        background.SetActive(true);
        menu.SetActive(true);
        isPaused = true;
        Time.timeScale = 0f;
    }

    public void Unpause()
    {
        menu.SetActive(false);
        areYouSure.SetActive(false);
        controls.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;
        background.SetActive(false);
    }

    public void GoToMainMenu()
    {
        isPaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(0);
    }

    public void ShowAreYouSure()
    {
        areYouSure.SetActive(true);
        menu.SetActive(false);
    }

    public void ShowControls()
    {
        controls.SetActive(true);
        menu.SetActive(false);
    }

    public void ShowMenu()
    {
        areYouSure.SetActive(false);
        menu.SetActive(true);
        menu.SetActive(true);
        controls.SetActive(false);
    }
}
