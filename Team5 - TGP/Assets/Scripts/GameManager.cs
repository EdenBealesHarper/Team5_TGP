using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    private static bool paused = false;
    private static int levelCount;
    private static AudioClip[] backgroundMusic;

    // Start is called before the first frame update
    void Start()
    {
        levelCount = SceneManager.sceneCountInBuildSettings - 1; // ignore hud, main menu at 0
        backgroundMusic = new AudioClip[levelCount];

        backgroundMusic[0] = Resources.Load<AudioClip>("Audio/MusicMainMenu");
        for (int i = 1; i < levelCount; i++)
        {
            AudioClip bgm = Resources.Load<AudioClip>("Audio/MusicLevel" + i);
            if (bgm)
                backgroundMusic[i] = bgm;
        }

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
        // don't pause in the main menu
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SetPause(!paused);
            }
        }
    }

    public void GameStart()
    {
        // assuming main menu is #0, ui is #1, other scenes are levels
        LoadLevel(1);
    }

    public void GameQuit()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }

    public void GameMenu()
    {
        SceneManager.LoadScene(0);
        AudioManager.Instance().PlayBGM(backgroundMusic[0]);
    }

    public void LoadLevel(int levelNumber)
    {
        SceneManager.LoadScene(levelNumber + 1);
        SceneManager.LoadScene(1, LoadSceneMode.Additive);

        AudioClip nextLevelBGM = backgroundMusic[levelNumber];
        if (nextLevelBGM)
            AudioManager.Instance().PlayBGM(nextLevelBGM);

    }
    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
        SceneManager.LoadScene(1, LoadSceneMode.Additive);

        AudioClip nextLevelBGM = backgroundMusic[SceneManager.GetSceneByName(levelName).buildIndex - 1];
        if (nextLevelBGM)
            AudioManager.Instance().PlayBGM(nextLevelBGM);
    }

    public void ReloadLevel()
    {
        LoadLevel(SceneManager.GetSceneAt(0).name);
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
