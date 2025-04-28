using UnityEngine;
using Meta.XR.MRUtilityKit;
using System.Collections.Generic;
using TMPro;
using System.Collections;
using UnityEditor.Search;
using Unity.AI.Navigation;

public class MRUKManager : MonoBehaviour
{
    public MRUK mruk;
    public OVRInput.Controller controller;

    public GameObject floor;
    public NavMeshSurface surface;
    public GameObject NPC;

    private bool sceneHasBeenLoaded;
    public MRUKRoom currentRoom;

    public List<GameObject> anchorObjectsCreated = new();

    private bool SceneAndRoomInfoAvailable => currentRoom != null && sceneHasBeenLoaded;

    public bool isBuilt = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Press the right trigger to create or destroy anchor objects showing the anchor index
        //if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) && SceneAndRoomInfoAvailable)
        //{
        //    if (anchorObjectsCreated.Count == 0)
        //    {
        //        foreach (var anchor in currentRoom.Anchors)
        //        {
        //            var gameObject = Instantiate(new GameObject(), Vector3.zero, Quaternion.identity, anchor.transform);
        //            gameObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        //            gameObject.transform.localScale = Vector3.one * 0.1f;
        //            gameObject.AddComponent<TextMeshPro>();
        //            gameObject.GetComponent<TextMeshPro>().text = anchor.transform.position.ToString();
        //            anchorObjectsCreated.Add(gameObject);
        //        }
        //    }
        //    else
        //    {
        //        foreach (var anchorObject in anchorObjectsCreated)
        //        {
        //            Destroy(anchorObject);
        //        }
        //        anchorObjectsCreated.Clear();
        //    }
        //}

        if (!isBuilt)
        {
            MeshFilter mesh = FindAnyObjectByType<MeshFilter>();
            if (mesh)
            {
                Debug.Log("Meshes are ready!");
                isBuilt = true;
                BuildNavMesh();
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

    public void BuildNavMesh()
    {
        floor = GameObject.Find("FLOOR_EffectMesh");
        surface.BuildNavMesh();
        NPC.SetActive(true);
        NPC.transform.position = floor.transform.position + new Vector3(2, 10, 2);
    }
}
