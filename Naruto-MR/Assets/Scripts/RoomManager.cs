using UnityEngine;
using Meta.XR.MRUtilityKit;
using System.Collections;
using Unity.AI.Navigation;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class RoomManager : MonoBehaviour
{
    public MRUK mruk;
    public OVRInput.Controller controller;

    public bool isBuilt = false;
    public NavMeshSurface surface;
    public GameObject NPC;

    public Vector3 spawnOffset;
    public float maxDistance;

    public MRUKRoom room;
    
    public GameObject burnMark;
    public GameObject bulletHole;

    private void spawnNPC()
    {
        Vector3 spawnPosition = room.gameObject.transform.position + spawnOffset;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(spawnPosition, out hit, maxDistance, NavMesh.AllAreas))
        {
            NPC.transform.position = hit.position;
            NPC.SetActive(true);
        }
        else
        {
            Debug.LogWarning("No valid NavMesh position found near: " + spawnPosition);
        }
    }

    private void BuildNavMesh()
    {
        surface.BuildNavMesh();
        Debug.Log("NavMesh building is done");
    }

    private void AdjustObjectComponents()
    {
        Debug.Log(room.gameObject.GetType());
        foreach (Transform child in room.gameObject.transform)
        {
            Debug.Log(child.gameObject.name);
            foreach (Transform grandChild in child)
            {
                Debug.Log(grandChild.gameObject.name);
                grandChild.gameObject.AddComponent<SceneCollisionHandler>();
                var handler = grandChild.gameObject.GetComponent<SceneCollisionHandler>();
                handler.burnMark = burnMark;
                handler.bulletHole = bulletHole;
                //grandChild.gameObject.GetComponent<MeshRenderer>().enabled = false;
                if (grandChild.gameObject.name != "FLOOR_EffectMesh")
                {
                    var modider = grandChild.gameObject.AddComponent<NavMeshModifier>();
                    modider.overrideArea = true;
                    modider.area = NavMesh.GetAreaFromName("Not Walkable");
                }
            }
            
        }
    }

    private IEnumerator WaitForRoomCreated()
    {
        while (true)
        {
            room = FindAnyObjectByType<MRUKRoom>();
            if (room) break;
            yield return null;
        }
        Debug.Log("The room is ready");
    }

    private IEnumerable WaitForFloorCreated()
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
    }


    // Initialize the room
    private IEnumerator Initialization()
    {
        yield return WaitForRoomCreated();

        yield return WaitForFloorCreated();

        AdjustObjectComponents();

        BuildNavMesh();

        spawnNPC();
    }

    public void EnableMRUKManager()
    {
        Debug.Log($"{nameof(RoomManager)} has been enabled due to scene availability");
        StartCoroutine(Initialization());
    }
}
