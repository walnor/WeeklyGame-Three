using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public Home[] m_AIHomes;
    public Home m_PlayerHome = null;

    [SerializeField] GameObject winMenu = null;
    [SerializeField] GameObject loseMenu = null;
    [SerializeField] GameObject PauseMenu = null;

    [SerializeField] GameObject m_BaseCanvas = null;

    // Update is called once per frame
    void Update ()
    {
        if (!m_PlayerHome)
            return;

        if (Time.timeScale == 0.0f)
            return;

        int num = 0;
        foreach (Home h in m_AIHomes)
        {
            if (h)
            {
                num++;
            }
        }

        if (num == 0)
        {
            Victory();
        }

        if (!m_PlayerHome)
        {
            Loss();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }

    }

    void Pause()
    {
        Time.timeScale = 0.0f;
        m_BaseCanvas.SetActive(false);
        PauseMenu.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1.0f;
        m_BaseCanvas.SetActive(true);
        PauseMenu.SetActive(false);
    }

    void Victory()
    {
        Time.timeScale = 0.0f;
        m_BaseCanvas.SetActive(false);
        winMenu.SetActive(true);
    }

    void Loss()
    {
        Time.timeScale = 0.0f;
        m_BaseCanvas.SetActive(false);
        loseMenu.SetActive(true);
    }

    public void loadEasy()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("EasyMode");
    }

    public void loadMedium()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MediumMode");
    }

    public void loadHard()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("HardMode");
    }

    public void loadImpossible()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("ImpossibleMode");
    }

    public void loadMainMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
