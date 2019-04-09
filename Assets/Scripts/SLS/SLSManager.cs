using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

public class SLSManager : MonoBehaviour
{
    [HideInInspector]
    public Settings Settings;

    void Start()
    {
        if (GameManager.GM.SaveLoadSystem == this)
        {
            string path = Application.persistentDataPath + "/Settings.dat";

            if (File.Exists(path))
                LoadSettings();
        }
        else
            Destroy(gameObject);     
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
