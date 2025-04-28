using UnityEngine;

public class AnchorCollisionHandler : MonoBehaviour
{
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
     Debug.Log($"Trigger enter! I am {gameObject.name}, hit {other.gameObject.name}");
       if (gameObject.activeSelf)
{
    Debug.Log($"Destroying {gameObject.name}");
    Destroy(gameObject);
}
else
{
    Debug.Log($"Cannot destroy {gameObject.name} because it is inactive.");
}

    }

}
