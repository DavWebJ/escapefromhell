using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;
public enum Mode
{
    facile = 0,normal = 1,difficile = 2,hardcore = 3
}
public class M_Options : MonoBehaviour
{
    public static M_Options instance;
    private Transform menuManager;
    private GameObject panel_options;
    [Header("Audio : ")]
    private static readonly string FirstPlay = "FirstPlay";
    private static readonly string audioPlayerPref = "AudioPlayerPref";
    private static readonly string FXPlayerPref = "FXPlayerPref";
    private static readonly string screenPlayerPref = "ScreenPlayerPref";
    private static readonly string gameModePref = "GameModePref";
    private int firstPlayTime;
    private float audioPlayerValue,fxAudioPlayerValue;
    public AudioMixer audioMixer;
    public AudioMixerGroup ambient;
    public string MusicMasterName,FXMixerName;
    [SerializeField] public Text volumePercent,fxPercent;
    [SerializeField] public Slider slider;
    [SerializeField] public Slider sliderFx;
    [SerializeField] public Toggle fullscreenBtn;
    [SerializeField] public Dropdown select;
    public int GameLevelMode = 0;
    public bool isFull = false;
    public bool isInit = false;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
           
        }
    }

    
    private void Start()
    {
       
        firstPlayTime = PlayerPrefs.GetInt(FirstPlay);
        menuManager = GameObject.FindGameObjectWithTag("MenuManager").transform;
        panel_options = menuManager.Find("Panel_option").gameObject;
        slider = panel_options.transform.Find("Volume Option").GetComponent<Slider>();
        sliderFx = panel_options.transform.Find("Volume Fx Option").GetComponent<Slider>();
        fullscreenBtn = panel_options.transform.Find("full screen").GetComponent<Toggle>();
        select = panel_options.transform.Find("Mode").GetComponent<Dropdown>();
        volumePercent = slider.transform.Find("value").GetComponent<Text>();
        fxPercent = sliderFx.transform.Find("value fx").GetComponent<Text>();
        if (slider != null && sliderFx != null && fullscreenBtn != null)
        {
            if(firstPlayTime == 0)
            {
                setVolume(0.6f);
                setFXVolume(1f);
                fullscreenBtn.isOn = false;
                select.onValueChanged.AddListener(delegate
                {
                    GameLevelMode = select.value;
                    SetMode(select.value);
                });
                fullscreenBtn.onValueChanged.AddListener(delegate
                {
                    isFull = fullscreenBtn.isOn;
                    applyScreenSettings();
                    
                });

                slider.onValueChanged.AddListener(delegate
                {
                    setVolume(slider.value);
                });

                sliderFx.onValueChanged.AddListener(delegate
                {
                    setFXVolume(sliderFx.value);
                });
                PlayerPrefs.SetFloat(audioPlayerPref, getSliderVolumeValue(slider));
                PlayerPrefs.SetFloat(FXPlayerPref, getSliderVolumeValue(sliderFx));
                PlayerPrefs.SetString(screenPlayerPref, getIsFullScreenValue());
                PlayerPrefs.SetInt(gameModePref, GetGameMode());
                PlayerPrefs.SetInt(FirstPlay, -1);
                PlayerPrefs.Save();
            }
            else
            {
                audioPlayerValue = PlayerPrefs.GetFloat(audioPlayerPref);
                fxAudioPlayerValue = PlayerPrefs.GetFloat(FXPlayerPref);
                string fullToBool = PlayerPrefs.GetString(screenPlayerPref);
                GameLevelMode = PlayerPrefs.GetInt(gameModePref);
                isFull = System.Convert.ToBoolean(fullToBool);
                setVolume(audioPlayerValue);
                setFXVolume(fxAudioPlayerValue);
                setFullScreen(isFull);
                SetMode(GameLevelMode);
                select.value = GameLevelMode;
                slider.value = audioPlayerValue;
                sliderFx.value = fxAudioPlayerValue;
                fullscreenBtn.isOn = isFull;
                slider.onValueChanged.AddListener(delegate
                {
                    setVolume(slider.value);
                });

                sliderFx.onValueChanged.AddListener(delegate
                {
                    setFXVolume(sliderFx.value);
                });

                fullscreenBtn.onValueChanged.AddListener(delegate
                {
                    isFull = fullscreenBtn.isOn;
                    applyScreenSettings();

                });
                select.onValueChanged.AddListener(delegate
                {
                    SetMode(select.value);
                    GameLevelMode = select.value;
                });
            }
        } 
    }

    public void Init()
    {
        
        menuManager = GameObject.FindGameObjectWithTag("MenuManager").transform;
        panel_options = menuManager.Find("Panel_option").gameObject;
        slider = panel_options.transform.Find("Volume Option").GetComponent<Slider>();
        sliderFx = panel_options.transform.Find("Volume Fx Option").GetComponent<Slider>();
        fullscreenBtn = panel_options.transform.Find("full screen").GetComponent<Toggle>();
        select = panel_options.transform.Find("Mode").GetComponent<Dropdown>();
        volumePercent = slider.transform.Find("value").GetComponent<Text>();
        fxPercent = sliderFx.transform.Find("value fx").GetComponent<Text>();
        firstPlayTime = PlayerPrefs.GetInt(FirstPlay);
        if (slider != null && sliderFx != null && fullscreenBtn != null)
        {
            if (firstPlayTime == 0)
            {
                setVolume(0.6f);
                setFXVolume(1f);
                fullscreenBtn.isOn = false;
                select.onValueChanged.AddListener(delegate
                {
                    GameLevelMode = select.value;
                    SetMode(select.value);
                });
                fullscreenBtn.onValueChanged.AddListener(delegate
                {
                    isFull = fullscreenBtn.isOn;
                    applyScreenSettings();

                });

                slider.onValueChanged.AddListener(delegate
                {
                    setVolume(slider.value);
                });

                sliderFx.onValueChanged.AddListener(delegate
                {
                    setFXVolume(sliderFx.value);
                });
                PlayerPrefs.SetFloat(audioPlayerPref, getSliderVolumeValue(slider));
                PlayerPrefs.SetFloat(FXPlayerPref, getSliderVolumeValue(sliderFx));
                PlayerPrefs.SetString(screenPlayerPref, getIsFullScreenValue());
                PlayerPrefs.SetInt(gameModePref, GetGameMode());
                PlayerPrefs.SetInt(FirstPlay, -1);
                PlayerPrefs.Save();
            }
            else
            {
                audioPlayerValue = PlayerPrefs.GetFloat(audioPlayerPref);
                fxAudioPlayerValue = PlayerPrefs.GetFloat(FXPlayerPref);
                string fullToBool = PlayerPrefs.GetString(screenPlayerPref);
                GameLevelMode = PlayerPrefs.GetInt(gameModePref);
                isFull = System.Convert.ToBoolean(fullToBool);
                setVolume(audioPlayerValue);
                setFXVolume(fxAudioPlayerValue);
                setFullScreen(isFull);
                SetMode(GameLevelMode);
                select.value = GameLevelMode;
                slider.value = audioPlayerValue;
                sliderFx.value = fxAudioPlayerValue;
                fullscreenBtn.isOn = isFull;
                slider.onValueChanged.AddListener(delegate
                {
                    setVolume(slider.value);
                });

                sliderFx.onValueChanged.AddListener(delegate
                {
                    setFXVolume(sliderFx.value);
                });

                fullscreenBtn.onValueChanged.AddListener(delegate
                {
                    isFull = fullscreenBtn.isOn;
                    applyScreenSettings();

                });
                select.onValueChanged.AddListener(delegate
                {
                    SetMode(select.value);
                    GameLevelMode = select.value;
                });
            }
            isInit = true;
        }
          


    }

    public void setFullScreen(bool isFull)
    {
        Screen.fullScreen = isFull;
    }
    public void setVolume(float volume)
    {
        print("change vol");
        if(audioMixer != null)
            audioMixer.SetFloat(ambient.ToString(), Mathf.Log(volume) * 20f);

        if (volumePercent != null)
            volumePercent.text = Mathf.Round(volume * 100.0f).ToString() + " %";
    }

    public void setFXVolume(float volume)
    {
        if (audioMixer != null)
            audioMixer.SetFloat(FXMixerName, Mathf.Log(volume) * 20f);

        if (fxPercent != null)
            fxPercent.text = Mathf.Round(volume * 100.0f).ToString() + " %";
    }

    public void SetMode(int level)
    {
        GameLevelMode = level;
    }

    public float getSliderVolumeValue(Slider slider)
    {
        float vol = slider.value;
        return vol;
    }

    public float getSliderFXVolumeValue(Slider sliderfx)
    {
        float volFx = sliderfx.value;
        return volFx;
    }

    public string getIsFullScreenValue()
    {
        return isFull.ToString();
    }

    public int GetGameMode()
    {
        int gamemode = select.value;
        return gamemode;
    }
    public void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            saveAudioSettings();
            
        }
    }

    public void saveAudioSettings()
    {
        if(slider != null && sliderFx != null && fullscreenBtn != null && select != null)
        {
            PlayerPrefs.SetFloat(audioPlayerPref, slider.value);
            PlayerPrefs.SetFloat(FXPlayerPref, sliderFx.value);
            PlayerPrefs.SetInt(gameModePref, select.value);
            PlayerPrefs.SetString(screenPlayerPref, getIsFullScreenValue());
            PlayerPrefs.Save();
            
        }

    }



    public void applyScreenSettings()
    {
        setFullScreen(isFull);
        PlayerPrefs.SetString(screenPlayerPref, getIsFullScreenValue());
        PlayerPrefs.Save();
    }

    private void Update()
    {
        if(!isInit && MainMenuManager.instance.isInGame)
        {
            Init();
        }
    }


}
