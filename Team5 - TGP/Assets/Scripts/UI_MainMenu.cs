using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField]
    private Button startButton;

    [SerializeField]
    private Button quitButton;

    // Start is called before the first frame update
    void Start()
    {
        // initialise the buttons that use functions from the game manager
        // other setup that needs handling before the game/a level starts can also go here

        startButton.onClick.AddListener(GameManager.Instance().GameStart);
        quitButton.onClick.AddListener(GameManager.Instance().GameQuit);

        // reset values
        GameManager.Instance().SetPause(false);
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
