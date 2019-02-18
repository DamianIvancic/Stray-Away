using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public CameraController MainCam;
    public AudioSource MainAudio;
    public PlayerController Player;
    public InputManager InputManager;
    public SLSManager SaveLoadSystem;
    public UIManager UI;
    public HealthManager HPManager;
    public InventoryManager Inventory;
    public CutsceneManager CutsceneManager;
   
    public enum GameState
    {
        Menu,
        Playing,
        Dialogue,
        Paused,
        Cutscene,
        GameOver,
        Finished
    }

    [HideInInspector]
    public GameState _gameState;
    [HideInInspector]
    public static GameManager _GM;

    private void Awake()
    {
        if (_GM == null)
        {        
            _GM = this;

            int SceneIndex = SceneManager.GetActiveScene().buildIndex;

            switch (SceneIndex)
            {
                case (0):
                    _gameState = GameState.Menu;
                    break;
                default:
                    _gameState = GameState.Playing;
                    break;
            }

            if (Inventory != null)
                Inventory.Initialize();

            SceneManager.sceneLoaded += OnSceneLoadedListener;
           
            DontDestroyOnLoad(gameObject);
        }
    }
	
	void Update ()
    {
        if(_gameState == GameState.Playing)
        {
            if(Input.GetKeyDown(KeyCode.Escape))   
                PauseGame();      
        }   
        else if(_gameState == GameState.GameOver)
        {
            UI.SetGameOverMenu(true);
        }
        else if(_gameState == GameState.Finished)
        {
            Debug.Log("Finished");

            if (Input.GetKeyDown(KeyCode.Escape))
            {    //LoadScene(0);
                Application.Quit();
            }
        }
	}

    void OnSceneLoadedListener(Scene scene, LoadSceneMode mode) //listener for SceneManager.sceneLoaded
    {
        int SceneIndex = SceneManager.GetActiveScene().buildIndex;

        switch (SceneIndex)
        {
            case (0):
                _gameState = GameState.Menu;
                break;
            case (1):                         
                MainCam = FindObjectOfType<CameraController>();
                if(MainAudio == null)
                MainAudio = GameObject.FindWithTag("MainAudio").GetComponent<AudioSource>();
                UI.SetPauseMenu(false);             
                CutsceneManager.PlayCutscene(0);
                break;
            case (2):
                _gameState = GameState.Finished;
                break;   
        }
    }

    public void InitializeSettings() //saves settings into a file in binary format
    {
        SaveLoadSystem.Settings = new Settings(InputManager.KeyBindings);
        SaveLoadSystem.SaveSettings();
    }

    public void SetState(GameState state)
    {
        _gameState = state;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadScene(int sceneNum)
    {
        SceneManager.LoadScene(sceneNum);

    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame()
    {     
        UI.UIHearts.SetActive(true);

        if (Inventory.Contains("PowerCell"))
            UI.PowerCellUI.gameObject.SetActive(true);

       
        if (UI.MeteorTimer._started)
            UI.MeteorTimer.gameObject.SetActive(true);

        UI.SetPauseMenu(false);
        _gameState = GameState.Playing;  
    }

    public void PauseGame()
    {     
        UI.UIHearts.SetActive(false);

        if (Inventory.Contains("PowerCell"))
            UI.PowerCellUI.gameObject.SetActive(false);
    
        if(UI.MeteorTimer._started)
            UI.MeteorTimer.gameObject.SetActive(false);

        UI.SetPauseMenu(true);
        _gameState = GameState.Paused;
    }

    public void QuitGame()
    {
        int SceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (SceneIndex == 0)
            Application.Quit();
        else
            SceneManager.LoadScene(0);
    }
}

