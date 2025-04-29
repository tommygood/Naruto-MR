using UnityEngine;
using Meta.XR.MRUtilityKit;
using System.Collections;
using Unity.AI.Navigation;

public class RoomManager : MonoBehaviour
{
    public MRUK mruk;
    public OVRInput.Controller controller;

    public bool isBuilt = false;
    public NavMeshSurface surface;
    public GameObject NPC;

    private bool sceneHasBeenLoaded;
    public MRUKRoom currentRoom;

    private bool SceneAndRoomInfoAvailable => currentRoom != null && sceneHasBeenLoaded;

    private void Start()
    {
        StartCoroutine(Initialization());
    }

    // Initialize the room
    private IEnumerator Initialization()
    {
        while (true)
        {
            var floor = GameObject.Find("FLOOR_EffectMesh");
            if (floor)
            {
                //floor.layer = LayerMask.NameToLayer("Floor");
                //surface.layerMask = LayerMask.GetMask("Floor");
                break;
            }
            yield return null;
        }
        Debug.Log("The mesh of the room is ready");

        BuildNavMesh();

        AdjustObjectComponents();
    }

    private void BuildNavMesh()
    {
        surface.BuildNavMesh();
        Debug.Log("NavMesh building is done");
    }

    private void DisableMeshRenderers()
    {
        MeshRenderer[] renderers = FindObjectsOfType<MeshRenderer>();
        foreach (var renderer in renderers)
        {
            renderer.enabled = false;
        }
        Debug.Log("Disable all the MeshRenderers in the room");
    }

    private void AdjustObjectComponents()
    {
        Debug.Log(currentRoom.gameObject.GetType());
        foreach (Transform child in currentRoom.gameObject.transform)
        {
            Debug.Log(child.gameObject.name);
            foreach (Transform grandChild in child)
            {
                Debug.Log(grandChild.gameObject.name);
                grandChild.gameObject.AddComponent<SceneCollisionHandler>();
                grandChild.gameObject.GetComponent<MeshRenderer>().enabled = false;
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
        Debug.Log($"{nameof(RoomManager)} has been enabled due to scene availability");
    }

    private void BindRoomInfo(MRUKRoom room)
    {
        currentRoom = room;
        Debug.Log($"{nameof(RoomManager)} room was bound to current room");
    }
}
