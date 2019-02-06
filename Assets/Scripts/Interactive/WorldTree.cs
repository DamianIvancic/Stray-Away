using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTree : Interactable {


    public TextScroller TextScroller;

    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        TextScroller = (TextScroller)FindObjectOfType(typeof(TextScroller));
    }

    public override void DoOnInteract()
    {
        base.DoOnInteract();
        TextScroller.ScrollText("You're safe here - this is your home. You feel good. ");
        GameManager._GM.HPManager.RestoreHP();
    }
}
