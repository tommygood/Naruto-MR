using UnityEngine;
using UnityEngine.UI; // Needed for UI
using AnimationNamespace;
using System.Collections;

public class NarutoDamageHandler : MonoBehaviour
{
    AnimationManager animationManager;
    public int HP = 20; // HP of Naruto
    public int MaxHP = 20; // Maximum HP

    [Header("UI Blood Bar")]
    public Canvas worldSpaceCanvas; // Reference to the WorldSpace Canvas
    public Image healthBarFill; // Reference to the Image component inside the Canvas

    void Start()
    {
        animationManager = new AnimationNamespace.AnimationManager();
    }

    void Update()
    {
        // Always make the canvas face the camera
        if (worldSpaceCanvas != null)
        {
            worldSpaceCanvas.transform.rotation = Quaternion.LookRotation(worldSpaceCanvas.transform.position - Camera.main.transform.position);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("Effect_") && collision.gameObject.tag != "naruto_attack")
        {
            Debug.Log("NNN Naruto is hurt!" + collision.gameObject.name + " " + collision.gameObject.tag);
            StartCoroutine(playDropDamageAnimation());
            TakeDamage(10);
        }
    }

    private void TakeDamage(int amount)
    {
        HP -= amount;
        HP = Mathf.Clamp(HP, 0, MaxHP);

        UpdateHealthBar();

        Debug.Log("Naruto HP: " + HP);

        if (HP <= 0)
        {
            Debug.Log("Q Naruto is defeated!");
            // FIXME: drop a game object to the ground
            // DropGameObject();
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = (float)HP / MaxHP;
        }
        else
        {
            Debug.LogError("Health bar fill image is not assigned in the inspector.");
        }
    }

    private IEnumerator playDropDamageAnimation()
    {
        animationManager.SetAnimation("stumble_backward", true);
        yield return Sleep(2f);
        animationManager.SetAnimation("stumble_backward", false);
        yield return Sleep(1f);
        animationManager.SetAnimation("backflip", true);
        yield return Sleep(2.5f);
        animationManager.SetAnimation("backflip", false);
        yield return Sleep(1f);
        animationManager.SetAnimation("running", true);
    }

    private IEnumerator Sleep(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
}