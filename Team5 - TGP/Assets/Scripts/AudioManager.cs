using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    private static AudioMixer mixer;

    [SerializeField]
    private AudioSource[] sources;

    // more sources = more audio overlap = more detail
    private static AudioSource sourceBGM;
    private static AudioSource sourceWorld;
    private static AudioSource sourcePlayer;
    private static AudioSource sourceEnemies;

    private static float prePauseVolume;
    private static bool volumeLowered;

    // no buffer for bgm because that should only be one looping track
    private List<Sound> bufferWorld = new List<Sound>();
    private List<Sound> bufferPlayer = new List<Sound>();
    private List<Sound> bufferEnemies = new List<Sound>();

    struct Sound
    {
        public float timestamp;
        public AudioClip clip;
        public Sound(float time, AudioClip audio)
        {
            timestamp = time;
            clip = audio;
        }
    }

    private float playTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Initialise();

        //singleton
        DontDestroyOnLoad(gameObject);
    }

    public static AudioManager Instance()
    {
        if (_instance == null)
        {
            _instance = Instantiate(Resources.Load<GameObject>("Audio/AudioManager")).GetComponent<AudioManager>();
            _instance.Initialise();
        }
        return _instance;
    }

    private void Initialise()
    {
        sourceBGM = sources[0];
        sourceWorld = sources[1];
        sourcePlayer = sources[2];
        sourceEnemies = sources[3];

        mixer = Resources.Load<AudioMixer>("Audio/MainMixer");
        mixer.GetFloat("volumeBGM", out prePauseVolume);
        volumeLowered = false;
    }

    // Update is called once per frame
    void Update()
    {
        // lower bgm volume when game is paused
        if (GameManager.Instance().isPaused() && !volumeLowered)
        {
            mixer.GetFloat("volumeBGM", out prePauseVolume);

            float newVol = prePauseVolume - 20f;
            if (newVol < -80f) newVol = -80f;

            mixer.SetFloat("volumeBGM", newVol);

            volumeLowered = true;
        }
        else if (!GameManager.Instance().isPaused() && volumeLowered)
        {
            mixer.SetFloat("volumeBGM", prePauseVolume);
            volumeLowered = false;
        }

        // need to slow down audio so being on fire isn't awful
        if (playTimer <= 0)
        {
            EmptyBuffers();
            //playTimer = Time.maximumDeltaTime; could still be awful on fast computers
            playTimer = 0.1f;
        }
        else
            playTimer -= Time.deltaTime;
    }

    public void PlayBGM(AudioClip clip)
    {
        if (sourceBGM.clip != clip) // avoid jittery audio
        {
            if (sourceBGM.isPlaying) sourceBGM.Stop();
            sourceBGM.clip = clip;
            sourceBGM.Play();
        }
    }
    /// <summary>
    /// Play a sound effect that doesn't originate from the player or enemies.
    /// </summary>
    public void PlaySFXWorld(AudioClip clip)
    {
        // only add clip to queue if there is room and it hasn't been recently queued
        bool clipSafe = true;
        if (bufferWorld.Count < 6)
        {
            for (int i = 0; i < bufferWorld.Count; i++)
            {
                if (bufferWorld[i].clip == clip && Time.timeSinceLevelLoad - bufferWorld[i].timestamp < 1)
                    clipSafe = false;
            }
        }
        if (clipSafe) bufferWorld.Add(new Sound(Time.timeSinceLevelLoad, clip));
    }
    /// <summary>
    /// Play a sound effect that originates from the player.
    /// </summary>
    public void PlaySFXPlayer(AudioClip clip)
    {
        bool clipSafe = true;
        if (bufferPlayer.Count < 6)
        {
            for (int i = 0; i < bufferPlayer.Count; i++)
            {
                if (bufferPlayer[i].clip == clip && Time.timeSinceLevelLoad - bufferPlayer[i].timestamp < 1)
                    clipSafe = false;
            }
        }
        if (clipSafe) bufferPlayer.Add(new Sound(Time.timeSinceLevelLoad, clip));
    }
    /// <summary>
    /// Play a sound effect that originates from an enemy.
    /// </summary>
    public void PlaySFXEnemy(AudioClip clip)
    {
        bool clipSafe = true;
        if (bufferEnemies.Count < 6)
        {
            for (int i = 0; i < bufferEnemies.Count; i++)
            {
                if (bufferEnemies[i].clip == clip && Time.timeSinceLevelLoad - bufferEnemies[i].timestamp < 1)
                    clipSafe = false;
            }
        }
        if (clipSafe) bufferEnemies.Add(new Sound(Time.timeSinceLevelLoad, clip));
    }

    public AudioMixer GetMixer()
    {
        return mixer;
    }

    private void EmptyBuffers()
    {
        // could be a coroutine; plays fast enough as is
        float cutoffTime = Time.timeSinceLevelLoad - 2 * Time.maximumDeltaTime;

        for (int i = 0; i < bufferWorld.Count; i++)
        {
            if (bufferWorld[i].timestamp > cutoffTime)
            {
                if (sourceWorld.clip != bufferWorld[i].clip) sourceWorld.clip = bufferWorld[i].clip;
                sourceWorld.Play();
            }
            bufferWorld.RemoveAt(i);
        }

        for (int i = 0; i < bufferPlayer.Count; i++)
        {
            if (bufferPlayer[i].timestamp > cutoffTime)
            {
                if (sourcePlayer.clip != bufferPlayer[i].clip) sourcePlayer.clip = bufferPlayer[i].clip;
                sourcePlayer.Play();
            }
            bufferPlayer.RemoveAt(i);
        }

        for (int i = 0; i < bufferEnemies.Count; i++)
        {
            if (bufferEnemies[i].timestamp > cutoffTime)
            {
                if (sourceEnemies.clip != bufferEnemies[i].clip) sourceEnemies.clip = bufferEnemies[i].clip;
                sourceEnemies.Play();
            }
            bufferEnemies.RemoveAt(i);
        }
    }
}
