using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public PlayerController Player;
    public InputManager InputManager;
    public SLSManager SaveLoadSystem;
    public UIManager UI;
    public HealthManager HPManager;
    public InventoryManager Inventory;
    //[HideInInspector]
    public CameraController MainCam;

    public enum GameState
    {
        Start,
        Playing,
        Paused,
        GameOver
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
                    _gameState = GameState.Start;
                    break;
                default:
                    _gameState = GameState.Playing;
                    break;
            }

            if (Inventory != null)
                Inventory.Initialize();

            SceneManager.sceneLoaded += OnSceneLoaded;

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
	}

    public void InitializeSettings() //saves settings into a file in binary format
    {
        SaveLoadSystem.Settings = new Settings(InputManager.KeyBindings);
        SaveLoadSystem.SaveSettings();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) //callback for SceneManager.sceneLoaded
    {
        MainCam = FindObjectOfType<CameraController>();

        int SceneIndex = SceneManager.GetActiveScene().buildIndex;

        switch (SceneIndex)
        {
            case (0):
                _gameState = GameState.Start;
                break;
            default:
                StartGame();
                break;
        }
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
        _gameState = GameState.Playing;
        UI.UIHearts.SetActive(true);
        UI.SetPauseMenu(false);
    }

    public void PauseGame()
    {
        _gameState = GameState.Paused;
        UI.UIHearts.SetActive(false);
        UI.SetPauseMenu(true);
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

