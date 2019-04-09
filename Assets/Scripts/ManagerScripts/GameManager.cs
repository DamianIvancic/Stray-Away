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
    public GameState gameState;
    [HideInInspector]
    public static GameManager GM;

    void Awake()
    {
        if (GM == null)
        {        
            GM = this;

            int SceneIndex = SceneManager.GetActiveScene().buildIndex;

            switch (SceneIndex)
            {
                case (0):
                    SetState(GameState.Menu);
                    break;
                default:
                    SetState(GameState.Playing);
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
        if(gameState == GameState.Playing)
        {
            if(Input.GetKeyDown(KeyCode.Escape))   
                PauseGame();      
        }   
        else if(gameState == GameState.GameOver)
        {
            UI.SetGameOverMenu(true);
        }
        else if(gameState == GameState.Finished)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {   
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
                SetState(GameState.Menu);
                break;
            case (1):
                 MainCam = FindObjectOfType<CameraController>();
                 if(MainAudio == null) MainAudio = GameObject.FindWithTag("MainAudio").GetComponent<AudioSource>();
                 CutsceneManager.PlayCutscene(0);     
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
        if (state == GameState.Menu || state == GameState.Paused || state == GameState.GameOver)
            Cursor.visible = true;
        else
            Cursor.visible = false;

        gameState = state;
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

    public void PlayGame()
    {     
        UI.UIHearts.SetActive(true);

        if (Inventory.Contains("PowerCell"))
            UI.PowerCellUI.gameObject.SetActive(true);
    
        if (UI.MeteorTimer.started)
            UI.MeteorTimer.gameObject.SetActive(true);

        UI.SetMenu(false);
        SetState(GameState.Playing);
    }

    public void PauseGame()
    {     
        UI.UIHearts.SetActive(false);

        if (Inventory.Contains("PowerCell"))
            UI.PowerCellUI.gameObject.SetActive(false);
    
        if(UI.MeteorTimer.started)
            UI.MeteorTimer.gameObject.SetActive(false);

        UI.SetMenu(true);
        SetState(GameState.Paused);
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

