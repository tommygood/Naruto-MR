using UnityEngine;
using Meta.XR.MRUtilityKit;
using System.Collections.Generic;

public class MRUKManager : MonoBehaviour
{
    [SerializeField] private MRUK mruk;
    [SerializeField] private OVRInput.Controller controller;
    [SerializeField] private GameObject objectForWallAnchorsPrefab;

    private bool sceneHasBeenLoaded;
    private MRUKRoom currentRoom;
    private List<GameObject> wallAnchorObjectsCreated = new();

    private bool SceneAndRoomInfoAvailable => currentRoom != null && sceneHasBeenLoaded;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) && SceneAndRoomInfoAvailable)
        {
            if (wallAnchorObjectsCreated.Count == 0)
            {
                foreach (var wallAnchor in currentRoom.WallAnchors)
                {
                    var createdWallObject = Instantiate(objectForWallAnchorsPrefab, Vector3.zero, Quaternion.identity, wallAnchor.transform);
                    createdWallObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                    wallAnchorObjectsCreated.Add(createdWallObject);
                    Debug.Log($"{nameof(MRUKManager)} wall object created with Uuid: {wallAnchor.Anchor.Uuid}");
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

    // Update is called once per frame
    void Update()
    {
        
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
