using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public CameraController MainCam;
    public AudioSource MainAudio;
    public PlayerController Player;

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
            DontDestroyOnLoad(gameObject);

            int SceneIndex = SceneManager.GetActiveScene().buildIndex;

            switch (SceneIndex)
            {
                case (0):
                    gameState = GameState.Menu;
                    break;
                case (2):
                    gameState = GameState.Finished;
                    break;
                case (3):
                    MainCam = FindObjectOfType<CameraController>();
                    Player = FindObjectOfType<PlayerController>();
                    StartGame();
                    break;
                default:
                    MainCam = FindObjectOfType<CameraController>();
                    Player = FindObjectOfType<PlayerController>();
                    MainAudio = GameObject.FindWithTag("MainAudio").GetComponent<AudioSource>();
                    StartGame();
                    break;
            }

            SceneManager.sceneLoaded += OnSceneLoadedListener;            
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
            UIManager.Instance.SetGameOverMenu(true);
        }
        else if(gameState == GameState.Finished)
        {
            Debug.Log("Finished");

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
                gameState = GameState.Menu;
                break;
            case (1):
                gameState = GameState.Finished;
                break;
            case (2):
                MainCam = FindObjectOfType<CameraController>();             
                Player = FindObjectOfType<PlayerController>();
                StartGame();
                break;
            default:
                MainCam = FindObjectOfType<CameraController>();
                Player = FindObjectOfType<PlayerController>();
                MainAudio = GameObject.FindWithTag("MainAudio").GetComponent<AudioSource>();
                StartGame();
                break;
        }
    }

    public void SetState(GameState state)
    {
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

    public void StartGame()
    {
        UIManager.Instance.SetMenu(false);
        UIManager.Instance.HealthDisplay.SetActive(true);
  
        gameState = GameState.Playing;  
    }

    public void PauseGame()
    {
        UIManager.Instance.SetMenu(true);
        UIManager.Instance.HealthDisplay.SetActive(false);

        gameState = GameState.Paused;
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

