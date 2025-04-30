using UnityEngine;

public class SceneCollisionHandler : MonoBehaviour
{
    public GameObject burnMark;
    public GameObject bulletHole;

    private void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Quaternion rot = Quaternion.LookRotation(contact.normal);
            Vector3 pos = contact.point + contact.normal * 0.01f;

            //GameObject sticker;
            // fire ninjutsus
            //sticker = Instantiate(burnMark, pos, rot);
            // other ninjutsus
            //sticker = Instantiate(bulletHole, pos, rot);

            //sticker.transform.SetParent(contact.otherCollider.transform);
        }
    }
}
