using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiloGuard : Interactable {

 
    private List<string> _dialogue;
    private TextScroller _dialogueText;
    private int _dialogueIdx;

    private bool _siloActivated = false;

    void Start()
    {
        _dialogueText = GameManager._GM.UI.DialogueText;
        _dialogue = new List<string>();
        _dialogue.Add("Patrolling the woods beats guarding this old silo any day. I doubt we'll ever get it back up and running anyway...");


        ItemPickup.OnItemPickedUpCallback += PowerCellListener;
        Cutscene.OnCutsceneFinishedCallback += SiloCutsceneListener;    
    }

    void OnDestroy()
    {

        ItemPickup.OnItemPickedUpCallback -= PowerCellListener;
        Cutscene.OnCutsceneFinishedCallback -= SiloCutsceneListener;
    }

    #region callbacks
    void PowerCellListener(Item item)
    {
        if (item.ItemName == "PowerCell" && GameManager._GM.Inventory.GetCount(item) == 1)
        {
            _dialogue.Clear();
            _dialogue.Add("Try putting that object you found into the silo's fusion core. The old technology has advanced safety measures - if it finds it too unstable it will just abort the process.");
          //  Silo._siloTrigger.enabled = true;
        }

        if (item.ItemName == "PowerCell" && GameManager._GM.Inventory.GetCount(item) == 3)
        {
            if(_siloActivated)
            {
                _dialogue.Clear();
                _dialogue.Add("Did you try looking in the outer parts of the forest for more? With 6 cells we would re-activate our weapon.");
            }       
        }

        if (item.ItemName == "PowerCell" && GameManager._GM.Inventory.GetCount(item) == 6)
        {
            _dialogue.Clear();
            _dialogue.Add("You've got enough of those to power up the defense system. Way to go!");
        }
    }

    void SiloCutsceneListener(string cutsceneName)
    {
        if (cutsceneName == "SiloCutscene")
        {
            _siloActivated = true;

            if (GameManager._GM.Inventory.GetCount("PowerCell") < 3)
            {
                _dialogue.Clear();
                _dialogue.Add("If we had more of those energy cells our system could do something about the incoming threat... do you think there might be more out there?");
            }
            else if (GameManager._GM.Inventory.GetCount("PowerCell") < 6)
            {
                _dialogue.Clear();
                _dialogue.Add("Did you try looking in the outer parts of the forest for more? With 6 cells we would re-activate our weapon.");
            }
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

    public void SetDialogue(string dialogue)
    {
        _dialogue.Clear();
        _dialogue.Add(dialogue);
    }

}
