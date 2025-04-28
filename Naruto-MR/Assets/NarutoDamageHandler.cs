using UnityEngine;

public class NarutoDamageHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // on collision enter, check what the object is collided with
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("QQ Naruto collided with: " + collision.gameObject.name);
    }
}
