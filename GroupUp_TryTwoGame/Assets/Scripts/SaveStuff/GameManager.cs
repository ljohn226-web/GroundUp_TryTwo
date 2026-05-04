using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //create an instance of GM, only 1 can ever exist at a time
    public static GameManager Instance;

    [Header("Player")]
    public Vector3 lastCheckpoint;

    [Header("Collection")]
    public int essenceCollected;

    [Header("Dirty Flag")]
    public bool unsavedChanges = false;
   
    private void Awake()
    {
        if (Instance != null)       //if instance exists, destroy gameObj + exit
        {
            Destroy(gameObject);
            return;
        }
        //otherwise, this is GM, do not destroy it
        Instance = this; //FIRST ERROR
        DontDestroyOnLoad(gameObject);

        LoadGame(); //obvi
    }

    public void SaveGame()
    {
        SaveSystem.SaveGame();
        unsavedChanges = false; //SECOND ERROR
    }

    public void LoadGame()
    {
        SaveData data = SaveSystem.LoadGame();

        if (data == null) return;

        lastCheckpoint = data.lastCheckpoint;
        essenceCollected = data.essenceCollected;

        ApplyToScene();   //apply to objects in scene

        unsavedChanges = false; //loaded state is clean
    }

    // new method
    private void ApplyToScene()
    {
        //Player
        GameObject player = GameObject.FindWithTag("Player");
        //if (player != null)
        //    player.transform.position = lastCheckpoint;     //ayoooooo
               
    }

    public void UpdatePlayerPosition(Vector3 newPos)
    {
        //Do i need this?
    }

    public void ResetMemory()
    {
        //lastCheckpoint = Vector3.zero;
        essenceCollected = 0;
        unsavedChanges = true;
        Debug.Log("MemoryReset");
    }

    public void ResetSaveFile()
    {
        SaveSystem.DeleteSave();
        ResetMemory();
        Debug.Log("Disk Save Reset");
    }

    private void MarkDirty()
    {
        unsavedChanges = true;
    }

    private void MarkClean()
    {
        unsavedChanges = false;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplyToScene();
    }

}
