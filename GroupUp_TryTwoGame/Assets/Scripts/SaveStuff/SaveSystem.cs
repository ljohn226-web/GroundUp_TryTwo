using System.IO;
using UnityEngine;
using System;
using System.Collections.Generic;

//static class, not monobehaviour
public static class SaveSystem 
    //save system reads json file 2 and from GM and SaveData, but cannot load it
{
    //save file path to JSON
    private static string path => Application.persistentDataPath + "/save.json";

    public static void SaveGame()
    {
        var gm = GameManager.Instance;
        if (gm == null) return;

        //update SaveData with values from game snapshot
        SaveData data = new SaveData
        {
            lastCheckpoint = gm.lastCheckpoint,
            essenceCollected = gm.essenceCollected,
        };

        string json = JsonUtility.ToJson(data, true);  //turn all data to strings
        File.WriteAllText(path, json);          //write strings to json file 

        Debug.Log("Saved:\n" + json);
    }

    public static SaveData LoadGame()
    {
        if (!File.Exists(path)) return null; //protect against first time game is played
        //no save file or path exists until after u play once

        string json = File.ReadAllText(path);
        Debug.Log("Loaded:\n" + json);

        return JsonUtility.FromJson<SaveData>(json);    //only reading the JSON file
        //nothing updated in game, update happens in GM after this loads
    }

    //Delete / Reset Save File
    public static void DeleteSave()
    {
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Save file deleted");
        }
        else
        {
            Debug.Log("No save file to delete");
        }
    }

    // Reset Everything (optional helper)
    //what does it help with

    public static void ResetAll()
    {
        DeleteSave();
        var gm = GameManager.Instance;
        if (gm != null)
        {
            gm.ResetMemory(); // clear runtime state
        }
        Debug.Log("All game data reset");
    }

}
