using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpDrop : MonoBehaviour
{
    public float rayDist;
    public float castRadius;
    public LayerMask whatIsMoveable;
    private CharacterController _controller;

    void Start()
    {
        //use the freaking controller
        _controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if ( Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E Pressed");
            //pick up item
            Vector3 origin = transform.position + Vector3.up * (_controller.height * 0.5f);
            //rayDist = 2f;

            //what is wrong with you
            Debug.Log($"SphereCast Origin: {origin}, Direction: {transform.forward}, Radius: {castRadius}, Distance: {rayDist}");
            Debug.Log($"whatIsMoveable mask: {whatIsMoveable.value}");

            //query trigger interaction ignores trigger items
            //spherecast logic from char. controller
            if (Physics.SphereCast(origin, castRadius, transform.forward, 
                out RaycastHit hitObject, rayDist, whatIsMoveable, QueryTriggerInteraction.Ignore))
            {
                Debug.Log("Hit MEOW EMOW MEOW WHERE ARE YOU" + hitObject.collider.name);
                //pick up item
            }
            else
            {
                Debug.Log("SphereCast missed");
            }
        }
         if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Q Pressed");
            //drop item
        }   
    }

    //make it visible in scene view during play mode
    private void OnDrawGizmosSelected()
    {
        if (_controller == null) _controller = GetComponent<CharacterController>();

        Vector3 origin = transform.position + Vector3.up * (_controller.height * 0.5f);

        // Draw the sphere at origin
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(origin, castRadius);

        // Draw the ray direction
        Gizmos.color = Color.red;
        Gizmos.DrawLine(origin, origin + transform.forward * rayDist);
    }
}
