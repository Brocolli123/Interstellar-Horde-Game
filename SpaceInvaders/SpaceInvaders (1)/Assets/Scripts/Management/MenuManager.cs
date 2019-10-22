using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    static private MenuManager instance = null;

    public GameObject pausePanel;

    private void Awake()
    {
        Debug.Assert(instance == null);
        instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown("escape"))     //Activate/Deactivate Pause Menu ("ESC" key)
        {
            pausePanel.SetActive(!pausePanel.activeInHierarchy);   //Flips the active state of the screen
            if (pausePanel.activeSelf)      //is this best way to do this?
            {
                Time.timeScale = 0;     //if paused timescale set to 0
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Time.timeScale = 1;     //if unpausing set timescale to 1
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    public void LoadScene()       //Change this to take in scene index as parameter
    {
        SceneManager.LoadScene(1);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
