using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {

    public virtual void DoOnInteract()
    {
        GameManager._GM.UI.EnableDialogue(true);
    }
}
