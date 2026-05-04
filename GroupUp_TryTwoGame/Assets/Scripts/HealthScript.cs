using UnityEngine;

public class HealthScript : MonoBehaviour
{

    public int playerHealth = 100;

    // if player hit deadly ground, lose health in accordance to height of fall
    //on trigger enter deadly ground
    //when to turn on deadly ground?
    //when to turn off deadly ground?

   private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DeadlyGround"))
        {
            //calculate fall damage based on height of fall height - ground height
            float fallHeight = transform.position.y - other.transform.position.y;
            int damage = Mathf.RoundToInt(fallHeight * 10); //scale dmg with fall
            playerHealth -= damage;
            Debug.Log("Player took " + damage + " damage from falling. Current health: " + playerHealth);
        }
    }
}
