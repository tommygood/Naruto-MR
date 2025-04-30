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

        // 按 A 鍵 (OVR) 或 L 鍵 (PC) 建立圖片列
        if (OVRInput.GetDown(OVRInput.Button.One) || Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("建立圖片列！");
            CreateImageRowOnWallFace();
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
    string[] texturePaths = { "Image/a", "Image/b", "Image/c" };

    GameObject wallFace = GameObject.Find("WINDOW_FRAME");
    if (wallFace == null)
    {
        Debug.LogWarning("找不到WINDOW_FRAME");
        return;
    }

    Vector3 forward = wallFace.transform.forward;
    Vector3 right = wallFace.transform.right;
    Vector3 basePosition = wallFace.transform.position + forward * 0.05f;

    float spacing = 0.35f;
    Vector3 startPosition = basePosition - right * spacing;

    for (int i = 0; i < texturePaths.Length; i++)
    {
        Texture2D tex = Resources.Load<Texture2D>(texturePaths[i]);
        if (tex == null)
        {
            Debug.LogWarning($"無法載入圖片 {texturePaths[i]}");
            continue;
        }

        Material mat = new Material(Shader.Find("Unlit/Transparent"));
        mat.mainTexture = tex;

        GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        quad.GetComponent<Renderer>().material = mat;

        Vector3 offset = right * spacing * i;
        quad.transform.position = startPosition + offset;

        // 垂直貼在前方
        quad.transform.rotation = Quaternion.LookRotation(forward, Vector3.up);

        quad.transform.localScale = Vector3.one * 0.3f;
        quad.transform.SetParent(wallFace.transform);

        Debug.Log($"貼圖成功：{texturePaths[i]}");
    }

    Debug.Log("已將三張圖片垂直貼在 LAMP 前方");
    }


}
