using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FinalControl : MonoBehaviour
{
    public GameObject player;       // Player 或 CameraRig
    public GameObject prefabA;      // 要掉落的物件
    public Canvas uiCanvas;         // UI Canvas
    public Text textUI;             // 顯示文字的 UI
    public float fallSpeed = 0.2f;  // 掉落速度（每秒多少公尺）

    private bool hasDropped = false;

    void Update()
    {
        if (player == null) return;

        Vector3 playerPos = player.transform.position;
        Vector2 flatPos = new Vector2(playerPos.x, playerPos.z);

        // 區域放大 2 倍：X [-1.7, -0.5]，Z [-4.25, -2.45]
        bool inArea = flatPos.x >= -1.7f && flatPos.x <= -0.5f &&
                      flatPos.y >= -4.25f && flatPos.y <= -2.45f;

        Debug.Log($"Player position: {flatPos}, InArea: {inArea}");

        if (inArea && !hasDropped)
        {
            ShowUIText("低頭看看");
            StartCoroutine(SlowDropPrefab());
            hasDropped = true;
        }
    }

    void ShowUIText(string message)
    {
        if (textUI != null)
        {
            textUI.text = message;
            textUI.gameObject.SetActive(true);
            Debug.Log("顯示 UI 文字：低頭看看");
        }
    }

    IEnumerator SlowDropPrefab()
    {
        if (prefabA == null || player == null)
            yield break;

        // 從相機前方 0.5m、上方 1.0m 掉落
        Vector3 forward = player.transform.forward;
        Vector3 dropPosition = player.transform.position + forward * 2f + Vector3.up * 1.0f;

        GameObject obj = Instantiate(prefabA, dropPosition, Quaternion.identity);
        Debug.Log("開始緩慢掉落 Prefab A");

        float fallSpeed = 0.5f; // 每秒掉落速度
        bool hasLanded = false;

        while (!hasLanded)
        {
            // 射線偵測是否碰到 Plane
            if (Physics.Raycast(obj.transform.position, Vector3.down, out RaycastHit hit, 0.05f))
            {
                if (hit.collider.gameObject.name == "Plane")
                {
                    Debug.Log("Prefab A 已落在 Plane 上");
                    hasLanded = true;
                    break;
                }
            }

            obj.transform.position += Vector3.down * fallSpeed * Time.deltaTime;
            yield return null;
        }

        Debug.Log("Prefab A 掉落完成！");
    }
}
