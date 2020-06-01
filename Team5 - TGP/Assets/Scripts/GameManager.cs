using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    private static bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        //singleton. there can only be one
        DontDestroyOnLoad(this);
    }

    public static GameManager Instance()
    {
        if (_instance == null)
            _instance = new GameObject("GameManager", typeof(GameManager)).GetComponent<GameManager>();
        return _instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetPause(!paused);
        }
    }

    public void GameStart()
    {
        // assuming main menu is #0, ui is #1, other scenes are levels
        SceneManager.LoadScene(2);
        SceneManager.LoadScene(1,LoadSceneMode.Additive);
    }

    public void GameQuit()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }

    public void GameMenu()
    {
        SceneManager.LoadScene(0);
    }

    public bool isPaused()
    {
        return paused;
    }
    public void SetPause(bool value)
    {
        if (!paused)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
        paused = value;
    }
}
