using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class BinarySerializer
{
    public static void SaveSettings(Settings settings)
    {
        string path = Application.persistentDataPath + "/Settings.dat";

        FileStream fileStream = File.Create(path);
      
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize(fileStream, settings);

        fileStream.Close();
        Debug.Log("Settings saved at " + path);       
    }
	
    public static Settings LoadSettings()
    {
        string path = Application.persistentDataPath + "/Settings.dat";

        if(File.Exists(path))
        {
            FileStream fileStream = File.Open(path, FileMode.Open);

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            Settings settings = (Settings)binaryFormatter.Deserialize(fileStream);

            fileStream.Close();
            Debug.Log("Settings loaded from " + path);

            return settings;
        }

        Debug.LogWarning("No file found at " + path);
        return null;    
    }
}
