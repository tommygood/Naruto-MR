using Oculus.Interaction;
using UnityEngine;

public class SceneCollisionHandler : MonoBehaviour
{
    public GameObject burnMark;
    public GameObject bulletHole;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision!");
        if (!collision.gameObject.name.Contains("Effect_")) return;

        foreach (ContactPoint contact in collision.contacts)
        {
            Quaternion rot = Quaternion.LookRotation(contact.normal);
            Vector3 pos = contact.point + contact.normal * 0.01f;

            GameObject sticker;
            // Fireball
            if (collision.gameObject.name.Contains("Effect_07_OneHandSmash"))
            {
                sticker = Instantiate(burnMark, pos, rot);
                Debug.Log("Fireball");
            }
            // Other ninjutsus
            else
            {
                sticker = Instantiate(bulletHole, pos, rot);
                Debug.Log("Other ninjutsus");
            }

            sticker.transform.SetParent(transform);
        }
    }
}
