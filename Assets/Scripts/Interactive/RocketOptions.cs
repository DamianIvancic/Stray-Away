using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RocketOptions : MonoBehaviour
{
  
    public Button escapeButton;
    public Button saveButton;
    public Text escapeText;
    public Text saveText;


    private int _numOfPowerCells;


  /*  private void OnEnable()
    {
        _numOfPowerCells = GameManager._GM.Inventory.GetItem("Powercell");

        if(_numOfPowerCells < 3)
        {
            escapeButton.interactable = false;
            saveButton.interactable = false;
            saveButton.GetComponentInChildren<Text>().text = "Unavailable";
            escapeText.text = "You need 3 Power cells to fuel the rocket";
            saveText.text = "This option is unavailable";

        }
        else if(_numOfPowerCells < 6)
        {
            escapeButton.interactable = true;
            saveButton.interactable = false;
            saveButton.GetComponentInChildren<Text>().text = "Save your home!";
            escapeText.text = "The rocket is ready to leave the planet";
            saveText.text = "You need 6 power cells for the explosion to be powerful enough";
        }
        else
        {
            escapeButton.interactable = true;
            saveButton.interactable = true;
            saveButton.GetComponentInChildren<Text>().text = "Save your home!";
            escapeText.text = "The rocket is ready to leave the planet";
            saveText.text = "Your sacrifice will be remembered";
        }
    }*/

}
