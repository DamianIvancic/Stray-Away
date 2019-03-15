using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTree : Interactable {

    private TextScroller TextScroller;

    void Start()
    {
        TextScroller = UIManager.Instance.DialogueBox.GetComponentInChildren<TextScroller>();
    }


    public override void DoOnInteract()
    {
        TextScroller.DisplayText("You're safe here - this is your home. You feel good and your HP is restored.");
        HealthManager.Instance.RestoreHP();
    }
}
