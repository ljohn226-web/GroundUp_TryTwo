using UnityEngine;

public class CheckPointSystem : MonoBehaviour
{
    public Vector3 currentCheckPoint;
    public Vector3 lastCheckPoint;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        lastCheckPoint = currentCheckPoint;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CheckPoint"))
        {
            currentCheckPoint = other.gameObject.transform.position;
            Destroy(other.gameObject);
        }
    }
}
