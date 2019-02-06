using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public Image DialogueBox;
    public GameObject UIHearts;
  
    private Resolution[] _resolutions;
    private static UIManager UI;
    
	void Start ()
    {
        if(UI == null)
        {
            UI = this;

            FullScreenToggle.isOn = Screen.fullScreen;

            _resolutions = Screen.resolutions;

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

            DontDestroyOnLoad(gameObject);
        }     
	}
	
	// Update is called once per frame
	void Update ()
    {

	
	}

    public void SetPauseMenu(bool pause)
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
        DialogueBox.gameObject.SetActive(state);
    }

    public void PlayButton()
    {
        GameManager GM = GameManager._GM;

        if (GM._gameState == GameManager.GameState.Start)          
            GM.LoadScene(1);              
        else if (GM._gameState == GameManager.GameState.Paused)
            GM.StartGame();
    }
}
