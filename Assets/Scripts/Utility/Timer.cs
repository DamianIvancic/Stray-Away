using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    private float _minutes;
    private float _seconds;

    private Text _display;

    private float _timer = 0;
    [HideInInspector]
    public bool _started = false;

    public void Initialize()
    {
        gameObject.SetActive(true);

        _display = GetComponent<Text>();

        int numCells = GameManager._GM.Inventory.GetCount("PowerCell");

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

        _started = true;
    }
	
	void Update ()
    {
        if(GameManager._GM._gameState == GameManager.GameState.Playing)
        {
            if (_minutes > 0 || _seconds > 0)
            {
                if (_minutes > 0 && _seconds <= 0)
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

                _display.text = _minutes + ":" + _seconds;
            }
            else
            {
                _display.text = _minutes + ":" + _seconds;
                GameManager._GM.SetState(GameManager.GameState.GameOver);
            }
        }  
	}

    public void SetTimer(float minutes)
    {
        _minutes = minutes;
    }
}
