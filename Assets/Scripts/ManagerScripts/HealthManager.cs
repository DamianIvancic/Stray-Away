﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour {

    public int _maxHealth = 6;
    public float InvulTimer = 1;
    public float InvulMax = 1;

    public Image HPHeart;
    public Sprite FullHeart;
    public Sprite EmptyHeart;

    public static HealthManager Instance;

    private int _currentHealth;
    private List<Image> _HPHearts;
    private AudioSource _damageSound;

    public delegate void OnDamageTaken();
    public static OnDamageTaken OnDamageTakenCallback;
 
    void Awake ()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            _damageSound = GetComponent<AudioSource>();

            _currentHealth = _maxHealth;
            _HPHearts = new List<Image>();       
            for (int i = 0; i < _maxHealth; i++)
            {
                _HPHearts.Add(Instantiate(HPHeart, gameObject.transform));
            }

            SceneManager.sceneLoaded += OnSceneLoadedListener;
        }
        else
            Destroy(gameObject);     
    }

    void Update()
    {
        InvulTimer += Time.deltaTime;
    }

    void OnSceneLoadedListener(Scene scene, LoadSceneMode mode)
    {
        RestoreHP();
    }

    public void TakeDamage(int damage=1)
    {
        if(GameManager.GM.gameState == GameManager.GameState.Playing)
        {
            if (InvulTimer > InvulMax)
            {
                InvulTimer = 0;

                _currentHealth -= damage;
                _damageSound.Play();
                RefreshHearts();

                if (_currentHealth <= 0)
                {
                    GameManager.GM.SetState(GameManager.GameState.GameOver);                 
                }
            }
        }      
    }

    public void RestoreHP()
    {
        _currentHealth = _maxHealth;
        RefreshHearts();
    }

    public void RefreshHearts()
    {
        for (int i = 0; i<_maxHealth; i++)
        {
            if (i < _currentHealth)          
                _HPHearts[i].sprite = FullHeart;         
            else
                _HPHearts[i].sprite = EmptyHeart;
        }
    }
}
