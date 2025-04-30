using UnityEngine;
using UnityEngine.UI;

public class FinalControl : MonoBehaviour
{
    public Transform cameraRig;        // 玩家頭部（或相機）的位置
    public GameObject textUI;          // 顯示「低頭看看」的 UI（Text 或 Canvas）
    public GameObject dropPrefab;      // 要掉落的 Prefab（A）
    public Transform dropPoint;        // 掉落起始點（選擇性）

    private bool inTargetZone = false;
    private bool hasLookedDown = false;
    private float lookDownThreshold = 40f; // 低頭的角度條件
    private GameObject droppedObject;

    void Start()
    {
        if (textUI != null)
            textUI.SetActive(false);
    }

    void Update()
    {
        Vector3 pos = transform.position;
        float x = pos.x;
        float z = pos.z;

        // 檢查是否在區域內（X,Z）：(-0.8,-3.8), (-1.4,-3.8), (-1.4,-2.9), (-0.8,-2.9)
        bool inside = (x >= -1.4f && x <= -0.8f) && (z >= -3.8f && z <= -2.9f);
        if (inside && !inTargetZone)
        {
            inTargetZone = true;
            if (textUI != null)
                textUI.SetActive(true);
        }

        if (inTargetZone && !hasLookedDown)
        {
            Vector3 camForward = cameraRig.forward;
            float angle = Vector3.Angle(Vector3.down, camForward);

            if (angle < lookDownThreshold)
            {
                hasLookedDown = true;
                if (textUI != null)
                    textUI.SetActive(false);

                StartCoroutine(DropPrefabSlowly());
            }
        }
    }

    private System.Collections.IEnumerator DropPrefabSlowly()
    {
        Vector3 start = dropPoint != null ? dropPoint.position : transform.position + Vector3.up * 1.5f;
        droppedObject = Instantiate(dropPrefab, start, Quaternion.identity);

        float duration = 2.0f;
        float elapsed = 0f;
        Vector3 end = start + Vector3.down * 1.5f;

        while (elapsed < duration)
        {
            if (droppedObject == null) yield break;
            droppedObject.transform.position = Vector3.Lerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (droppedObject != null)
            droppedObject.transform.position = end;
    }
}
