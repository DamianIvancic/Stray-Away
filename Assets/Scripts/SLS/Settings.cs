using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Settings 
{
    public List<InputManager.Action> KeyBindings;

    public Settings(List<InputManager.Action> keyBindings)
    {
        KeyBindings = keyBindings;
    }
}
