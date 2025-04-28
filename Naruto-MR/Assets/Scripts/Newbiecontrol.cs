using UnityEngine;
using Meta.XR.MRUtilityKit;
using System.Collections.Generic;

public class Newbiecontrol : MonoBehaviour
{
    public MRUK mruk;
    public OVRInput.Controller controller;
    public GameObject modelPrefab; // 在Inspector中設定Prefab

    private bool sceneHasBeenLoaded;
    public MRUKRoom currentRoom;

    public List<GameObject> anchorObjectsCreated = new();

    private bool SceneAndRoomInfoAvailable => currentRoom != null && sceneHasBeenLoaded;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Newbiecontrol Start()");
        
        // 自動啟用MRUKManager場景標記
        EnableMRUKManager();

        var meshes = FindObjectsOfType<Meta.XR.MRUtilityKit.MRUKAnchor>();
        Debug.Log($"Found {meshes.Length} anchors.");

        var meshFilters = FindObjectsOfType<MeshFilter>();
        Debug.Log($"Found {meshFilters.Length} meshes.");

        if (modelPrefab == null)
        {
            Debug.LogWarning("ModelPrefab has not been assigned in the Inspector.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            Debug.Log("Primary Index Trigger Pressed!");

            if (anchorObjectsCreated.Count == 0)
            {
                Debug.Log("Creating anchors...");

                // 強制設置場景為可用
                EnableMRUKManager();

                // 改用TABLE_EffectMesh
                CreateTargetsBasedOnName("TABLE_EffectMesh");
            }
            else
            {
                Debug.Log("Destroying anchors...");
                foreach (var anchorObject in anchorObjectsCreated)
                {
                    Destroy(anchorObject);
                }
                anchorObjectsCreated.Clear();
            }
        }
    }

    void CreateTargetsBasedOnName(string targetName)
    {
        Debug.Log($"CreateTargetsBasedOnName method called with name: {targetName}");

        var potentialTargets = new List<GameObject>();
        var allObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (var obj in allObjects)
        {
            if (obj.name == targetName)
            {
                potentialTargets.Add(obj);
            }
        }

        Debug.Log($"{potentialTargets.Count} potential targets found with name '{targetName}'.");

        if (potentialTargets.Count == 0)
        {
            Debug.LogWarning("No potential targets found with the specified name!");
        }

        foreach (var target in potentialTargets)
        {
            if (modelPrefab != null)
            {
                GameObject model = Instantiate(modelPrefab, target.transform);

                // 調整位置到父物件中心
                model.transform.localPosition = Vector3.zero;

                // 把rotation改成直立式
                model.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);

                // 尺寸正常顯示
                model.transform.localScale = Vector3.one * 0.5f;

                // ★ 確保有Collider（避免Prefab沒有Collider時出錯）
                if (model.GetComponent<Collider>() == null)
                {
                    model.AddComponent<BoxCollider>(); 
                    model.GetComponent<Collider>().isTrigger = true; // 設成Trigger比較柔和
                }

                // ★ 加上自動銷毀碰撞腳本
                if (model.GetComponent<AnchorCollisionHandler>() == null)
                {
                    model.AddComponent<AnchorCollisionHandler>();
                }

                anchorObjectsCreated.Add(model);
            }
            else
            {
                Debug.LogWarning("Model Prefab is null! Please assign a prefab in the Inspector.");
            }
        }
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
        Debug.Log($"{nameof(MRUKManager)} has been enabled due to scene availability");
    }

    private void BindRoomInfo(MRUKRoom room)
    {
        currentRoom = room;
        Debug.Log($"{nameof(MRUKManager)} room was bound to current room");

        if (currentRoom.Anchors != null && currentRoom.Anchors.Count > 0)
        {
            Debug.Log("Anchors found in current room!");
        }
        else
        {
            Debug.Log("No anchors found in current room!");
        }
    }
}
