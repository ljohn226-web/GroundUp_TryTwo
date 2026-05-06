using UnityEngine;

public class AnimationController1 : MonoBehaviour
{
    [Header("References")]
    public Animator anim;
    public PlayerMovement pm;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Animate();
    }
        
    public void Animate()
    {
        anim.SetFloat("XAxis", pm.horizontalInput);
        anim.SetFloat("YAxis", pm.verticalInput);
        if (pm.isWalking)
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);

        }

        if (pm.isClimbing)
        {
            anim.SetBool("isClimbing", true);
        }
        else
        {
            anim.SetBool("isClimbing", false);
        }
       
    }
}
