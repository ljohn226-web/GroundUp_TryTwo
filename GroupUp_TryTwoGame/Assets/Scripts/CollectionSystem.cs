using System.Collections.Generic;
using System.Collections;
using UnityEngine;


public class CollectionSystem : MonoBehaviour
{
   private int essenceCount = 0; //exists in GM, just a shell

    

    void Start()
    {
        //GameManager.Instance += SyncfromGameManager;
        //SyncFromGameManager();
    }

    void Update()
    {
        essenceCount = GameManager.Instance.essenceCollected;
    }


    private void OnTriggerEnter(Collider other)
    {
    //    int essenceCount = GameManager.essenceCount;
        if (other.CompareTag("Essence"))
        {
            GameManager.Instance.essenceCollected++;
            essenceCount = GameManager.Instance.essenceCollected;
            GameManager.Instance.essenceCollected = essenceCount;

            Debug.Log("Essence Collected");
            Debug.Log(GameManager.Instance.essenceCollected);
            Destroy(other.gameObject);
        }
    }
}
