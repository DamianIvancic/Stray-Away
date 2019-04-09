using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceGuard : Interactable
{
    public Transform ReturnPoint;

    private List<string> _dialogue;
    private TextScroller _dialogueText;
    private int _dialogueIdx;

    private BoxCollider2D ReturnTrigger;

    private bool _passCondition = false; // 3 power cells & activating the silo to let deactivate the trigger which blocks the player. this is set to true by whichever of those 2 is done first
                                         // and then checked by the 2nd when turning the trigger off
    void Start()
    {
        _dialogueText = GameManager.GM.UI.DialogueText;
        InitializeDialogue();


        BoxCollider2D[] colliders = GetComponents<BoxCollider2D>();
        foreach(BoxCollider2D collider in colliders)
        {
            if (collider.isTrigger)
                ReturnTrigger = collider;
        }

        ItemPickup.OnItemPickedUpCallback += PowerCellListener;
        Cutscene.OnCutsceneFinishedCallback += SiloCutsceneListener;
    }

    void OnDestroy()
    {
        ItemPickup.OnItemPickedUpCallback -= PowerCellListener;
        Cutscene.OnCutsceneFinishedCallback -= SiloCutsceneListener;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Player")
        {
            AutoMove Mover = collider.gameObject.GetComponent<AutoMove>();
            PlayerController Player = collider.gameObject.GetComponent<PlayerController>();
            DoOnInteract();
            Player.ResetMovement();
            Mover.MoveUp();      
        }
    }

    void InitializeDialogue()
    {
        _dialogue = new List<string>();
        _dialogue.Add("Report back to me if you find anything unusual near the village. When you're done with the patrol you can go wherever you want. Oh and if you get hurt just go to back to the village to fix yourself up.");
    }

    #region callbacks
    void PowerCellListener(Item item)
    {
        if (item.ItemName == "PowerCell")
        {
            if(GameManager.GM.Inventory.GetCount(item) == 1)
            {
                _dialogue.Clear();
                _dialogue.Add("That item you found's interesting for sure. Did you try talking to the silo guard about it? He knows a lot about these things - that's why he was chosen to guard the tech.");
            }

            if (GameManager.GM.Inventory.GetCount(item) == 3)
            {
                if (_passCondition)
                    ReturnTrigger.enabled = false;
                else
                    _passCondition = true;
            }

            if (GameManager.GM.Inventory.GetCount(item) == 6)
            {
                _dialogue.Clear();
                _dialogue.Add("Looks like you've got all the cells you could possibly need. Hurry, get to the silo!");
            }
        }      
    }

    void SiloCutsceneListener(string cutsceneName)
    {        
        if(cutsceneName == "SiloCutscene")
        {
            _dialogue.Clear();
            _dialogue.Add("Keep searching the woods for those energy cells.");

            if (_passCondition)
                ReturnTrigger.enabled = false;
            else
                _passCondition = true;
        }  
    }

    #endregion

    public override void DoOnInteract()
    {
        _dialogueText.DisplayText(_dialogue[_dialogueIdx]);

        if (_dialogue.Count > 1)
        {
            _dialogueIdx++;

            if (_dialogueIdx == _dialogue.Count)
                _dialogueIdx = 0;
        }
    }
}
