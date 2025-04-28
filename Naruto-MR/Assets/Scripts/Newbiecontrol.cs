using UnityEngine;
using Meta.XR.MRUtilityKit;
using System.Collections.Generic;

public class NewbieControl : MonoBehaviour
{
    public MRUK mruk;
    public OVRInput.Controller controller;
    public GameObject modelPrefab; // 在Inspector中設定Prefab

    private bool sceneHasBeenLoaded;
    private MRUKRoom currentRoom;

    private readonly List<GameObject> anchorObjectsCreated = new();

    private bool SceneAndRoomInfoAvailable => currentRoom != null && sceneHasBeenLoaded;

    void Start()
    {
        Debug.Log("NewbieControl Start()");

        EnableMRUKManager();

        var anchors = FindObjectsOfType<MRUKAnchor>();
        Debug.Log($"Found {anchors.Length} MRUKAnchors.");

        var meshFilters = FindObjectsOfType<MeshFilter>();
        Debug.Log($"Found {meshFilters.Length} MeshFilters.");

        if (modelPrefab == null)
        {
            Debug.LogWarning("ModelPrefab has not been assigned in the Inspector.");
        }
        {
            Debug.Log("Primary Index Trigger Pressed!");

            if (anchorObjectsCreated.Count == 0)
            {
                Debug.Log("Creating anchors...");
                EnableMRUKManager();
                CreateTargetsBasedOnName("BED");
            }
        }
    }

    void Update()
    {
        {
            if (anchorObjectsCreated.Count == 0)
            {
                Debug.Log("Creating anchors...");
                EnableMRUKManager();
                CreateTargetsBasedOnName("BED");
            }
        }
    }

    private void CreateTargetsBasedOnName(string targetName)
    {
        Debug.Log($"CreateTargetsBasedOnName called with name: {targetName}");

        var potentialTargets = new List<GameObject>();
        var allObjects = FindObjectsOfType<GameObject>();

        foreach (var obj in allObjects)
        {
            if (obj.name.Equals(targetName))
            {
                potentialTargets.Add(obj);
            }
        }

        Debug.Log($"{potentialTargets.Count} potential targets found with name '{targetName}'.");

        if (potentialTargets.Count == 0)
        {
            Debug.LogWarning($"No potential targets found with name '{targetName}'.");
            return;
        }
        foreach (var target in potentialTargets)
        {
            if (modelPrefab == null)
            {
                Debug.LogWarning("ModelPrefab is null, skipping instantiation.");
                continue;
            }

            var model = Instantiate(modelPrefab, target.transform);

            model.transform.localPosition = Vector3.zero;
            model.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
            model.transform.localScale = Vector3.one * 0.3f;

            // 加上Collider
            var collider = model.AddComponent<BoxCollider>();
            collider.isTrigger = true;
            collider.size = new Vector3(collider.size.x, collider.size.y * 10, collider.size.z);

            // 加上Rigidbody
            var rb = model.AddComponent<Rigidbody>();
            rb.isKinematic = true; // 設成Kinematic，避免被重力影響，只負責觸發碰撞

            // 加上碰撞處理
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
        Debug.Log("MRUKManager enabled: Scene is now available.");
    }

    private void BindRoomInfo(MRUKRoom room)
    {
        currentRoom = room;
        Debug.Log("MRUK Room bound successfully.");

        if (currentRoom.Anchors != null && currentRoom.Anchors.Count > 0)
        {
            Debug.Log($"Anchors found: {currentRoom.Anchors.Count}");
        }
        else
        {
            Debug.Log("No anchors found in the current room!");
        }
    }
}
