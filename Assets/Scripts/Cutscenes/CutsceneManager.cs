using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneManager : Interactable
{
    public List<Cutscene> Cutscenes;

    void Start()
    {
        if (GameManager.GM.CutsceneManager == this)
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
        {
            GameManager.GM.UI.EndingScreen.SetActive(true);
            GameManager.GM.gameState = GameManager.GameState.Finished;
        }
        else
        {
            if (GameManager.GM.MainAudio != null)
                GameManager.GM.MainAudio.enabled = true;

            if (GameManager.GM.UI.PowerCellUI != null && GameManager.GM.Inventory.Contains("PowerCell"))
                GameManager.GM.UI.PowerCellUI.gameObject.SetActive(true);

            GameManager.GM.PlayGame();
        }
    }

    public void PlayCutscene(int cutsceneIdx)
    {          
        GameManager.GM.SetState(GameManager.GameState.Cutscene);
      
        if(GameManager.GM.MainAudio != null)
            GameManager.GM.MainAudio.enabled = false;
       
        if (GameManager.GM.UI.UIHearts != null)
            GameManager.GM.UI.UIHearts.SetActive(false);

        if (GameManager.GM.UI.PowerCellUI != null)
            GameManager.GM.UI.PowerCellUI.gameObject.SetActive(false);

        if (GameManager.GM.UI.MeteorTimer.started)
            GameManager.GM.UI.MeteorTimer.gameObject.SetActive(false);

        if (Cutscenes != null && cutsceneIdx < Cutscenes.Count)
        {
            Cutscenes[cutsceneIdx].Play();
        }
    }
}
