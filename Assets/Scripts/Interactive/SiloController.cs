using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SiloController : Interactable
{
    public SiloGuard Guard;
    public Animator RocketAnim;

    #region Options UI
    private GameObject OptionsPanel;
    private Button EscapeButton;
    private Button DefendButton;
    private Text EscapeText;
    private Text DefendText;
    #endregion

    private TextScroller TextScroller;
  
    [HideInInspector]
    public BoxCollider2D _siloTrigger;
    private Animator _siloAnim;

    private bool _animationPlayed = false;
  
    void Start()
    {
        
        TextScroller = GameManager.GM.UI.DialogueBox.GetComponentInChildren<TextScroller>();
        _siloAnim = GetComponent<Animator>();

        BoxCollider2D[] colliders = GetComponents<BoxCollider2D>();
        foreach(BoxCollider2D collider in colliders)
        {
            if (collider.isTrigger)
                _siloTrigger = collider;
        }

        InitializeOptions();

        ItemPickup.OnItemPickedUpCallback += PowerCellListener;
        Cutscene.OnCutsceneFinishedCallback += SiloCutsceneListener;
    }

    void OnDestroy()
    {
        ItemPickup.OnItemPickedUpCallback -= PowerCellListener;
        Cutscene.OnCutsceneFinishedCallback -= SiloCutsceneListener;
    }

    #region Collisions -> all the collision/trigger functions go here

    void OnCollisionEnter2D(Collision2D collision)
    {
       //empty but needs to be declared to overwrite base
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        //empty but needs to be declared to overwrite base
    }

    void OnCollisionExit2D(Collision2D collision)
    {
       //empty but needs to be declared to overwrite base
    }
    #endregion

    public override void DoOnInteract()
    {
        if (_animationPlayed == false)
        {
            GameManager.GM.CutsceneManager.PlayCutscene(1);
            _animationPlayed = true;
        }
        else if (_animationPlayed == true)
        {
            DisplayOptions();
        }
    }

    void PowerCellListener(Item item)
    {
        if (item.ItemName == "PowerCell" && GameManager.GM.Inventory.GetCount(item) == 1)
        {
            _siloTrigger.enabled = true;
        }
    }

    void SiloCutsceneListener(string cutsceneName) // listener for OnCutsceneFinishedCallback
    {
        if (cutsceneName == "SiloCutscene")
        {
            _siloAnim.SetBool("IsOpen", true);
            RocketAnim.SetTrigger("RocketRise");
            Invoke("CloseSilo", 5f);
        }
    }
   
    void CloseSilo()
    {
        _siloAnim.SetBool("IsOpen", false);
    }

    void InitializeOptions()
    {
        OptionsPanel = GameManager.GM.UI.RocketOptions;

        Button[] buttons = OptionsPanel.GetComponentsInChildren<Button>();

        foreach(Button button in buttons)
        {
            if (button.gameObject.name == "EscapeButton")
                EscapeButton = button;

            if (button.gameObject.name == "DefendButton")
                DefendButton = button;
        }

        Text[] texts = OptionsPanel.GetComponentsInChildren<Text>();

        foreach (Text text in texts)
        {
            if (text.gameObject.name == "EscapeText")
                EscapeText = text;

            if (text.gameObject.name == "DefendText")
                DefendText = text;
        }
    }

    void DisplayOptions()
    {
        if (GameManager.GM.Inventory.Contains("PowerCell"))
        {
            GameManager.GM.SetState(GameManager.GameState.Menu);
            OptionsPanel.SetActive(true);

            int numPowerCells = GameManager.GM.Inventory.GetCount("PowerCell");

            if (numPowerCells < 3)
            {
                EscapeButton.interactable = false;
                DefendButton.interactable = false;
                EscapeButton.GetComponentInChildren<Text>().text = "Unavailable";
                DefendButton.GetComponentInChildren<Text>().text = "Unavailable";
                EscapeText.text = "You need 3 Power cells to fuel the escape rocket";
                DefendText.text = "You need 6 Power cells to bring the system to full power";

            }
            else if (numPowerCells < 6)
            {
                EscapeButton.interactable = true;
                DefendButton.interactable = false;
                EscapeButton.GetComponentInChildren<Text>().text = "Escape";
                DefendButton.GetComponentInChildren<Text>().text = "Unavailable";
                EscapeText.text = "The rocket is ready to leave the planet";
                DefendText.text = "You need 6 power cells to bring the system to full power";
            }
            else
            {
                EscapeButton.interactable = true;
                DefendButton.interactable = true;
                EscapeButton.GetComponentInChildren<Text>().text = "Escape";
                DefendButton.GetComponentInChildren<Text>().text = "Save your home";
                EscapeText.text = "The rocket is ready to leave the planet";
                DefendText.text = "Your deeds will be remembered";
            }
        }
    }
}
