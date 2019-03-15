using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class UIManager : MonoBehaviour {

    public GameObject MenuPanel;
    public GameObject OptionsPanel;
    public GameObject ControlsPanel;
    public GameObject GameOverPanel;
    public Toggle FullScreenToggle;
    public AudioMixer Mixer;
    public Dropdown ResolutionsDropdown;
    public GameObject DialogueBox;
    public TextScroller DialogueText;
    public GameObject HealthDisplay;

    public static UIManager Instance;
   
    private Resolution[] _resolutions;
   
	void Awake ()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            FullScreenToggle.isOn = Screen.fullScreen;

            _resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToArray();

            ResolutionsDropdown.ClearOptions();

            List<string> options = new List<string>();

            int resolutionIndex = 0;
            for (int i = 0; i < _resolutions.Length; i++)
            {
                string option = _resolutions[i].width + "x" + _resolutions[i].height;
                options.Add(option);

                if (_resolutions[i].width == Screen.currentResolution.width &&
                    _resolutions[i].height == Screen.currentResolution.height)
                {
                    resolutionIndex = i;
                }
            }

            ResolutionsDropdown.AddOptions(options);
            ResolutionsDropdown.value = resolutionIndex;
            ResolutionsDropdown.RefreshShownValue();

            DialogueText.Start();

            SceneManager.sceneLoaded += OnSceneLoadedListener;
          
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
	}
	
    void OnSceneLoadedListener(Scene scene, LoadSceneMode mode)
    {
        int SceneIndex = SceneManager.GetActiveScene().buildIndex;

        switch (SceneIndex)
        {
            case (0):
                SetGameOverMenu(false);
                SetMenu(true);
                HealthDisplay.SetActive(false);           
                break;
            case (1):            
                SetMenu(false);
                SetGameOverMenu(false);
                break;
            case (2):
                HealthDisplay.SetActive(false);
                break;
        }
    }

    public void SetMenu(bool pause)
    {
        MenuPanel.SetActive(pause);
    }

    public void SetOptionsMenu(bool state)
    {
        OptionsPanel.SetActive(state);
    }

    public void SetControlsMenu(bool state)
    {
        ControlsPanel.SetActive(state);
    }

    public void SetGameOverMenu(bool state)
    {
        GameOverPanel.SetActive(state);
    }

    public void SetFullScreen(bool fullScreen)
    {
        Screen.fullScreen = fullScreen;
    }

    public void SetVolume(float volume)
    {
        Mixer.SetFloat("Volume", volume);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void EnableDialogue(bool state)
    {
        DialogueBox.SetActive(state);
    }

    public void PlayButton()
    {
        GameManager GM = GameManager.GM;

        if (GM.gameState == GameManager.GameState.Menu)          
            GM.LoadScene(1);              
        else if (GM.gameState == GameManager.GameState.Paused)
            GM.StartGame();
    }
}
