using UnityEngine;

public class AnchorCollisionHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("effect"))
        {
            Debug.Log($"{gameObject.name} was hit by {other.gameObject.name}, destroying this object.");
            Destroy(gameObject);
        }
    }
}
