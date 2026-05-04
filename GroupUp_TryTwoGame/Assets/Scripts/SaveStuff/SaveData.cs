using System;
using System.Collections.Generic;
using UnityEngine;

//not a monobehaviour
[Serializable]
public class SaveData
    //only really need one class of save data for this game :3
    //you got this!
{
    //my little shells
    public Vector3 lastCheckpoint;
    public int playerHealth;
    public int essenceCollected;
}
