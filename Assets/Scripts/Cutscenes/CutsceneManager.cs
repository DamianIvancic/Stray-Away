using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneManager : Interactable
{
    public List<Cutscene> Cutscenes;


    void Start()
    {
        if (GameManager._GM.CutsceneManager == this)
        {        
            Cutscene.OnCutsceneFinishedCallback += EndingCutsceneCallback;

            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);      
    }

    void EndingCutsceneCallback(string cutsceneName)
    {
        if (cutsceneName == "EscapeCutscene" || cutsceneName == "DefendCutscene")
            GameManager._GM.LoadScene("EndingScreen");
        else
        {
            if (GameManager._GM.MainAudio != null)
                GameManager._GM.MainAudio.enabled = true;

            if (GameManager._GM.UI.PowerCellUI != null && GameManager._GM.Inventory.Contains("PowerCell"))
                GameManager._GM.UI.PowerCellUI.gameObject.SetActive(true);

            GameManager._GM.StartGame();
        }
    }

    public void PlayCutscene(int cutsceneIdx)
    {
        GameManager._GM._gameState = GameManager.GameState.Cutscene;

        if(GameManager._GM.MainAudio != null)
            GameManager._GM.MainAudio.enabled = false;
       
        if (GameManager._GM.UI.UIHearts != null)
            GameManager._GM.UI.UIHearts.SetActive(false);

        if (GameManager._GM.UI.PowerCellUI != null)
            GameManager._GM.UI.PowerCellUI.gameObject.SetActive(false);

        if (GameManager._GM.UI.MeteorTimer._started)
            GameManager._GM.UI.MeteorTimer.gameObject.SetActive(false);

        if (Cutscenes != null && cutsceneIdx < Cutscenes.Count)
        {
            Cutscenes[cutsceneIdx].Play();
        }
    }
}
