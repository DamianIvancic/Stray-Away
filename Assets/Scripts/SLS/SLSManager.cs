using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

public class SLSManager : MonoBehaviour
{
    [HideInInspector]
    public Settings Settings;

    public static SLSManager Instance;

    void Awake()                                                                   //if there's no settings to load from, default settings get initialized from InputManager.Start();
    {                                                                              //if there are settings to load from, InputManager.Keybindings get initialized from settings
        if (Instance == null)                                                      //settings get reinitialized every time a key is re-binded
        {                                                                          //before that all callbacks need to be cleared because functions can't be serialized into a file
            Instance = this;                                                       //after reinitializing settings callbacks need to be re-registered immediately
            DontDestroyOnLoad(gameObject);

            string path = Application.persistentDataPath + "/Settings.dat";
            if (File.Exists(path))                
                LoadSettings();         
        }
        else
            Destroy(gameObject);     
    }

    public void SaveSettings() //saves settings into a file in binary format
    {
        Settings = new Settings(InputManager.Instance.KeyBindings);
        BinarySerializer.SaveSettings(Settings);
    }


    public void LoadSettings()
    {     
        Settings = BinarySerializer.LoadSettings();
    }
}
