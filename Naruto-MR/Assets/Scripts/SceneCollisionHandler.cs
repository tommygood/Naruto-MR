using System.Collections;
using Oculus.Interaction;
using UnityEngine;

public class SceneCollisionHandler : MonoBehaviour
{
    public GameObject burnMark;
    public GameObject bulletHole;

    public float delay = 5;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision!");
        if (!collision.gameObject.name.Contains("Effect_")) return;

        Vector3 point = Vector3.zero;
        Vector3 normal = Vector3.zero;
        foreach (ContactPoint contact in collision.contacts)
        {
            point += contact.point;
            normal += contact.normal;
        }
        point /= collision.contacts.Length;
        normal /= collision.contacts.Length;
        normal.Normalize();

        Quaternion rot = Quaternion.LookRotation(normal);
        Vector3 pos = point + normal * 0.01f;

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
            pos -= normal * 0.05f;
            sticker = Instantiate(bulletHole, pos, rot);
            Debug.Log("Other ninjutsus");
        }

        sticker.transform.SetParent(transform);
        
        StartCoroutine(DelayedDestroy(sticker));
    }

    IEnumerator DelayedDestroy(GameObject objectToDestroy)
    {
        yield return new WaitForSeconds(delay);
        Destroy(objectToDestroy);
    }
}
