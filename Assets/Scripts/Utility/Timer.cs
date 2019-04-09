using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour {


    [HideInInspector]
    public bool started = false;

    private float _minutes;
    private float _seconds;
    private float _timer = 0;

    private Text _display;


    public void Initialize()
    {
        gameObject.SetActive(true);

        _display = GetComponent<Text>();

        int numCells = GameManager.GM.Inventory.GetCount("PowerCell");

        switch (numCells)
        {
            case 1:
                _minutes = 16;
                break;
            case 2:
                _minutes = 13;
                break;
            case 3:
                _minutes = 10;
                break;
        }

        _seconds = 0;

        started = true;

        SceneManager.sceneLoaded += OnSceneLoadedListener;
    }
	
	void Update ()
    {
        if(GameManager.GM.gameState == GameManager.GameState.Playing)
        {
            if (_minutes > 0 || _seconds > 0)
            {
                if (_minutes > 0 && _seconds < 0)
                {
                    _minutes--;
                    _seconds = 59;
                }

                _timer += Time.deltaTime;

                if (_timer >= 1)
                {
                    _timer -= 1;
                    _seconds -= 1;
                }

                if(_seconds < 10)
                    _display.text = _minutes + ":0" + _seconds;
                else
                    _display.text = _minutes + ":" + _seconds;
            }
            else
            {          
                _display.text = _minutes + ":" + _seconds;
                GameManager.GM.SetState(GameManager.GameState.GameOver);
            }
        }  
	}

    void OnSceneLoadedListener(Scene scene, LoadSceneMode mode)
    {
        started = false;
    }
}
