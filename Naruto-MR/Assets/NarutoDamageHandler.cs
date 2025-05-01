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

    public float damageInterval = 5; // Interval between damage checks
    private float damageCount = 0; // Counter for damage taken

    private bool startCountDamage = false; // Flag to start counting damage

    public GameObject qq_naruto; // Reference to the Naruto GameObject

    public GameObject origin_naruto; // Reference to the original Naruto GameObject

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
        if (startCountDamage)
        {
            damageCount += Time.deltaTime;
            if (damageCount >= damageInterval)
            {
                // Check if Naruto is alive and update the health bar
                if (HP > 0)
                {
                    UpdateHealthBar();
                }
                damageCount = 0; // Reset the counter
                startCountDamage = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (damageCount > 0)
        {
            return; // Ignore if damage is not taken
        }

        if (other.name.Contains("Effect_") && other.tag != "naruto_attack")
        {
            Debug.Log("NNN Naruto is hurt!" + other.name + " " + other.tag);
            StartCoroutine(playDropDamageAnimation());
            TakeDamage(10);
            startCountDamage = true; // Start counting damage
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
            // if the tag of this object is "naruto", then destroy this object
            if (gameObject.CompareTag("qq_naruto"))
            {
                Debug.Log("QQQ Naruto is defeated!");
                // use SceneLoader to load the next scene
                StartCoroutine(BGMManager.Instance.FadeOut());
                SceneLoader.Instance.LoadNewScene("Assets/Scenes/Final.unity");
                // Destroy the current scene
                StartCoroutine(BGMManager.Instance.FadeIn(3));
                SceneLoader.Instance.UnloadCurrentScene("Assets/Scenes/test.unity");
            }
            else
            {
                StartCoroutine(BGMManager.Instance.FadeOut());
                qq_naruto.SetActive(true); // Show the QQ Naruto prefab
                StartCoroutine(BGMManager.Instance.FadeIn(2));
                origin_naruto.SetActive(false); // Hide the original Naruto prefab
                
            }

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