using UnityEngine;

public class AnchorCollisionHandler : MonoBehaviour
{
    private static int destroyedCount = 0; // Anchor的總數

    private void Start()
    {
        if (GetComponent<Rigidbody>() == null)
        {
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true; // 不被物理影響，只負責碰撞事件
        }
        StartCoroutine(BGMManager.Instance.FadeIn(0));
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger enter! {gameObject.name} hit {other.gameObject.name} (tag: {other.tag})");

        // comppare the name contqains "Effect_"
        if (other.name.Contains("Effect_"))
        {
            if (gameObject.activeSelf)
            {
                Debug.Log($"Destroying {gameObject.name} it was hit by the effect.");
                Destroy(gameObject);

                // 計數+1
                destroyedCount++;

                if (destroyedCount == 6)
                {
                    Debug.Log("新手村結束！");
                    // use SceneLoader to load the next scene
                    StartCoroutine(BGMManager.Instance.FadeOut());
                    DontDestroyOnLoad(this.gameObject);//the animator gameObject
                    SceneLoader.Instance.LoadNewScene("Assets/Scenes/test.unity");
                    // Destroy the current scene
                    StartCoroutine(BGMManager.Instance.FadeIn(1));
                    SceneLoader.Instance.UnloadCurrentScene("Assets/Scenes/newbie.unity");
                }
            }
            else
            {
                Debug.Log($"Cannot destroy {gameObject.name} because it is inactive.");
            }
        }
    }
}
