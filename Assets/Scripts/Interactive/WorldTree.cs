using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTree : Interactable {


    private TextScroller TextScroller;

    void Start()
    {
        TextScroller = GameManager.GM.UI.DialogueBox.GetComponentInChildren<TextScroller>();
    }


    public override void DoOnInteract()
    {
        TextScroller.DisplayText("You're safe here - this is your home. You feel good and your HP is restored.");
        GameManager.GM.HPManager.RestoreHP();
    }
}
