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

    [SerializeField]
    private AudioClip mainMenuBGM;

    [SerializeField]
    private AudioClip buttonPress;

    // Start is called before the first frame update
    void Start()
    {
        // initialise the buttons that use functions from the game manager
        // other setup that needs handling before the game/a level starts can also go here

        startButton.onClick.AddListener(GameManager.Instance().GameStart);
        quitButton.onClick.AddListener(GameManager.Instance().GameQuit);

        AudioManager.Instance().PlayBGM(mainMenuBGM);

        // reset values
        GameManager.Instance().SetPause(false);
        Time.timeScale = 1;
    }

    public void PressButton()
    {
        AudioManager.Instance().PlaySFXWorld(buttonPress);
    }

    public void ChangeAudioVolume(float value)
    {
        AudioManager.Instance().GetMixer().SetFloat("volumeAll", value);
    }

    public void ChangeBGMVolume(float value)
    {
        AudioManager.Instance().GetMixer().SetFloat("volumeBGM", value);
    }

    public void ChangeSFXVolume(float value)
    {
        AudioManager.Instance().GetMixer().SetFloat("volumeSFX", value);
    }
}
