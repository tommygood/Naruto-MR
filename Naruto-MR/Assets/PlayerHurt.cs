using UnityEngine;
using LinesNamespace;
using RedBorderFlashNamespace;

public class PlayerHurt : MonoBehaviour
{
    private LinesManager linesManager;
    private RedBorderFlash redBorderFlash; // Reference to the red border flash script

    public GameObject redBorderFlashObject; // Drag the GameObject that has RedBorderFlash on it in Inspector
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        linesManager = new LinesManager();
        redBorderFlash = redBorderFlashObject.GetComponent<RedBorderFlash>();
    }

    // on collision enter
    void OnCollisionEnter(Collision collision)
    {
        // If the player is hit by the effect of ninjutsu
        if (collision.gameObject.name.Contains("Effect") && collision.gameObject.tag == "naruto_attack")
        {
            // Play the hurt animation
            Debug.Log("WWW Player is hurt!");
            linesManager.Play("S_scream");
            redBorderFlash.Play();
        }
    }
}
