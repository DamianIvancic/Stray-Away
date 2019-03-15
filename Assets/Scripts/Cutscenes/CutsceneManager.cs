using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    public List<Cutscene> Cutscenes;

    public static CutsceneManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            Cutscene.OnCutsceneFinishedCallback += CutsceneEndingCallback;
        }
        else
            Destroy(gameObject);      
    }

    public void CutsceneEndingCallback(string cutsceneName)
    {
        Debug.Log(cutsceneName);

        if (cutsceneName == "EscapeCutscene" || cutsceneName == "DefendCutscene")
            GameManager.GM.LoadScene("EndingScreen");
        else
        {         
            if (GameManager.GM.MainAudio != null)
                GameManager.GM.MainAudio.enabled = true;

            GameManager.GM.StartGame();
        }
    }

    public void PlayCutscene(int cutsceneIdx)
    {
        GameManager.GM.gameState = GameManager.GameState.Cutscene;

        if(GameManager.GM.MainAudio != null)
            GameManager.GM.MainAudio.enabled = false;
       
        if (UIManager.Instance.HealthDisplay != null)
            UIManager.Instance.HealthDisplay.SetActive(false);

        if (Cutscenes != null && cutsceneIdx < Cutscenes.Count)
        {
            Cutscenes[cutsceneIdx].Play();
        }
    }
}
