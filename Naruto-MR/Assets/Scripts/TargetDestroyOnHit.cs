using UnityEngine;

public class TargetDestroyOnHit : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("effect"))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("effect"))
        {
            Destroy(gameObject);
        }
    }
}
