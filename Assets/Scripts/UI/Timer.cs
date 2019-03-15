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

    public void Initialize(float minutes)
    {
        gameObject.SetActive(true);

        _display = GetComponent<Text>();

        _minutes = minutes;
        _seconds = 0;

        _started = true;
    }
	
	void Update ()
    {
        if(GameManager.GM.gameState == GameManager.GameState.Playing)
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
                GameManager.GM.SetState(GameManager.GameState.GameOver);
            }
        }  
	}

    public void SetTimer(float minutes)
    {
        _minutes = minutes;
    }
}
