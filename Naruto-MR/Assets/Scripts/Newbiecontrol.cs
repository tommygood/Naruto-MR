using UnityEngine;
using Meta.XR.MRUtilityKit;
using System.Collections.Generic;

public class NewbieControl : MonoBehaviour
{
    public MRUK mruk;
    public OVRInput.Controller controller;
    public GameObject modelPrefab; // 可忽略不使用

    private bool sceneHasBeenLoaded;
    private MRUKRoom currentRoom;

    private readonly List<GameObject> anchorObjectsCreated = new();

    private bool SceneAndRoomInfoAvailable => currentRoom != null && sceneHasBeenLoaded;

    void Start()
    {
        Debug.Log("NewbieControl Start()");
        EnableMRUKManager();
        CreateTargetsBasedOnName("TABLE_EffectMesh");
    }

    void Update()
    {
        if (anchorObjectsCreated.Count == 0)
        {
            Debug.Log("Creating anchors...");
            EnableMRUKManager();
            CreateTargetsBasedOnName("TABLE_EffectMesh");
        }

    }

    private void CreateTargetsBasedOnName(string targetName)
    {
        var potentialTargets = new List<GameObject>();
        var allObjects = FindObjectsOfType<GameObject>();

        foreach (var obj in allObjects)
        {
            if (obj.name.Equals(targetName))
            {
                potentialTargets.Add(obj);
            }
        }

        if (potentialTargets.Count == 0)
        {
            Debug.LogWarning($"未找到 {targetName}");
            return;
        }

        foreach (var target in potentialTargets)
        {
            if (modelPrefab == null) continue;

            var model = Instantiate(modelPrefab, target.transform);
            model.transform.localPosition = Vector3.zero;
            model.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
            model.transform.localScale = Vector3.one * 0.3f;

            var collider = model.AddComponent<BoxCollider>();
            collider.isTrigger = true;
            collider.size = new Vector3(collider.size.x, collider.size.y * 10, collider.size.z);

            var rb = model.AddComponent<Rigidbody>();
            rb.isKinematic = true;

            if (model.GetComponent<AnchorCollisionHandler>() == null)
            {
                model.AddComponent<AnchorCollisionHandler>();
            }

            anchorObjectsCreated.Add(model);
        }

        Debug.Log($"{anchorObjectsCreated.Count} anchors created.");
    }

    private void OnEnable()
    {
        if (mruk != null)
        {
            mruk.RoomCreatedEvent.AddListener(BindRoomInfo);
        }
    }

    private void OnDisable()
    {
        if (mruk != null)
        {
            mruk.RoomCreatedEvent.RemoveListener(BindRoomInfo);
        }
    }

    public void EnableMRUKManager()
    {
        sceneHasBeenLoaded = true;
        Debug.Log("MRUKManager enabled");
    }

    private void BindRoomInfo(MRUKRoom room)
    {
        currentRoom = room;
        Debug.Log("Room bound");

        // 一進入自動建立圖片
        CreateImageRowOnWallFace();
    }


    private void CreateImageRowOnWallFace()
    {
    string[] texturePaths = { "Image/a", "Image/b", "Image/c", "Image/d", "Image/e", "Image/f" };

    GameObject[] windows = GameObject.FindGameObjectsWithTag("WINDOW"); // 建議用 Tag 管理
    if (windows.Length == 0)
    {
        // 若沒有使用 Tag，可用名稱搜尋
        List<GameObject> namedWindows = new();
        foreach (var obj in FindObjectsOfType<GameObject>())
        {
            if (obj.name == "WINDOW")
            {
                namedWindows.Add(obj);
            }
        }
        windows = namedWindows.ToArray();

        if (windows.Length == 0)
        {
            Debug.LogWarning("找不到任何名為 WINDOW 的物件");
            return;
        }
    }

    int textureIndex = 0;
    foreach (GameObject window in windows)
    {
        if (textureIndex >= texturePaths.Length)
        {
            Debug.Log("圖片數量不足以覆蓋所有 WINDOW");
            break;
        }

        Texture2D tex = Resources.Load<Texture2D>(texturePaths[textureIndex]);
        if (tex == null)
        {
            Debug.LogWarning($"無法載入圖片 {texturePaths[textureIndex]}");
            textureIndex++;
            continue;
        }

        Material mat = new Material(Shader.Find("Unlit/Transparent"));
        mat.mainTexture = tex;

        GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        quad.GetComponent<Renderer>().material = mat;

        // 在前方稍微往外一點貼圖
        Vector3 forward = window.transform.forward;
        quad.transform.position = window.transform.position + forward * 0.05f;

        quad.transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
        quad.transform.localScale = Vector3.one * 0.3f;
        quad.transform.SetParent(window.transform);

        Debug.Log($"已在 WINDOW 上貼圖：{texturePaths[textureIndex]}");
        textureIndex++;
    }

    Debug.Log("圖片貼圖作業完成");
    }



}
