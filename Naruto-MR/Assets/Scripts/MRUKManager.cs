using UnityEngine;
using Meta.XR.MRUtilityKit;
using System.Collections.Generic;
using TMPro;

public class MRUKManager : MonoBehaviour
{
    public MRUK mruk;
    public OVRInput.Controller controller;
    public GameObject objectForWallAnchorsPrefab;

    private bool sceneHasBeenLoaded;
    private MRUKRoom currentRoom;
    private List<GameObject> wallAnchorObjectsCreated = new();

    private bool SceneAndRoomInfoAvailable => currentRoom != null && sceneHasBeenLoaded;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) && SceneAndRoomInfoAvailable)
        {
            if (wallAnchorObjectsCreated.Count == 0)
            {
                int i = 0;
                foreach (var wallAnchor in currentRoom.WallAnchors)
                {
                    var createdWallObject = Instantiate(objectForWallAnchorsPrefab, Vector3.zero, Quaternion.identity, wallAnchor.transform);
                    createdWallObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                    createdWallObject.AddComponent<TextMeshPro>();
                    createdWallObject.GetComponent<TextMeshPro>().text = i.ToString();
                    wallAnchorObjectsCreated.Add(createdWallObject);
                    Debug.Log($"{nameof(MRUKManager)} wall object created with Uuid: {wallAnchor.Anchor.Uuid}");
                    i++;
                }
                Debug.Log($"{nameof(MRUKManager)} wall objects added to all walls");
            }
            else
            {
                foreach (var wallObject in wallAnchorObjectsCreated)
                {
                    Destroy(wallObject);
                }
                wallAnchorObjectsCreated.Clear();
                Debug.Log($"{nameof(MRUKManager)} wall objects were deleted");
            }
        }
    }

    private void OnEnable()
    {
        mruk.RoomCreatedEvent.AddListener(BindRoomInfo);
    }

    private void OnDisable()
    {
        mruk.RoomCreatedEvent.RemoveListener(BindRoomInfo);
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
    }
}
