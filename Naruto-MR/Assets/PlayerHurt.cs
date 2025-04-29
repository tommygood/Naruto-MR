using UnityEngine;

public class PlayerHurt : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // on collision enter
    void OnCollisionEnter(Collision collision)
    {
        // If the player is hit by the effect of ninjutsu
        if (collision.gameObject.name.Contains("Effect") && collision.gameObject.tag == "naruto_attack")
        {
            // Play the hurt animation
            Debug.Log("WWW Player is hurt!");
        }
    }
}
