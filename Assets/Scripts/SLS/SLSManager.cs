using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

public class SLSManager : MonoBehaviour
{
    [HideInInspector]
    public Settings Settings;

    void Awake()
    {
        string path = Application.persistentDataPath + "/Settings.dat";

        if (File.Exists(path))
            LoadSettings();
    }

    public void SaveSettings()
    {
        BinarySerializer.SaveSettings(Settings);
    }

    public void LoadSettings()
    {     
        Settings = BinarySerializer.LoadSettings();
    }
}
