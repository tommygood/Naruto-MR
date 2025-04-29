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
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger enter! {gameObject.name} hit {other.gameObject.name} (tag: {other.tag})");

        if (other.CompareTag("effect"))
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
                }
            }
            else
            {
                Debug.Log($"Cannot destroy {gameObject.name} because it is inactive.");
            }
        }
    }
}
