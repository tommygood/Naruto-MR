using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class NPCController : MonoBehaviour
{
    public Transform player;
    public NavMeshSurface surface;
    public float attackDistance;

    private NavMeshAgent agent;
    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //animator = GetComponent<Animator>();

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent is missing!");
        }

        if (player == null)
        {
            player = Camera.main.transform;
        }

        if (surface == null)
        {
            Debug.LogError("NavMeshSurface is missing!");
        }
        surface.BuildNavMesh();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(agent.transform.position, player.position);
        if (distance < attackDistance)
        {
            agent.isStopped = true;
            //animator.SetBool("Attack", true);
        } else
        {
            agent.isStopped = false;
            //animator.SetBool("Attack", false);
            agent.destination = player.position;
        }
    }

    //private void OnAnimatorMove()
    //{
    //    if (animator.GetBool("Attack") == false)
    //    {
    //        agent.speed = (animator.deltaPosition / Time.deltaTime).magnitude;
    //    }
    //}
}
