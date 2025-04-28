using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;
using Meta.XR.MRUtilityKit;
using NUnit.Framework;

public class NPCController : MonoBehaviour
{
    public Transform player;
    public NavMeshSurface surface;
    public float taijutsuThreshold;
    public float CloseRangeThreshold;
    public int blood = 30;

    private NavMeshAgent agent;
    private Animator animator;

    public Ninjutsu Fireball = new Ninjutsu(NinjutsuName.Fireball, 3, ElementType.Fire);
    public Ninjutsu Raikiri = new Ninjutsu(NinjutsuName.Raikiri, 3, ElementType.Lightning);
    public Ninjutsu GalePalm = new Ninjutsu(NinjutsuName.GalePalm, 3, ElementType.Wind);
    public Ninjutsu RockShelterCollapse = new Ninjutsu(NinjutsuName.RockShelterCollapse, 3, ElementType.Earth);
    public Ninjutsu Waterfall = new Ninjutsu(NinjutsuName.Waterfall, 3, ElementType.Water);
    public Ninjutsu PhoenixFire = new Ninjutsu(NinjutsuName.PhoenixFire, 3, ElementType.Fire);
    public Ninjutsu Rasengan = new Ninjutsu(NinjutsuName.Rasengan, 2, ElementType.None);
    // There are still other physical attacks needed to be add ...

    public int currentNinjutsu;

    public MRUKManager mrukManager;
    public MRUKRoom currentRoom;

    public float coolDown = 5f;
    private float currentCoolDown;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!player)
            player = Camera.main.transform;

        if (!surface)
            Debug.LogError("NavMeshSurface is missing!");

        agent = GetComponent<NavMeshAgent>();
        if (!agent)
            Debug.LogError("NavMeshAgent is missing!");

        //animator = GetComponent<Animator>();
        //if (!animator)
        //    Debug.Log("Animator is missing!");

        currentRoom = mrukManager.currentRoom;
        // surface.BuildNavMesh();
    }

    // Update is called once per frame
    void Update()
    {
        //float distance = Vector3.Distance(agent.transform.position, player.position);
        //if (distance < taijutsuThreshold)
        //{
        //    PerformTaijutsu();
        //    currentCoolDown = coolDown;
        //}
        //else
        //{
        //    agent.isStopped = false;
        //    //animator.SetBool("Attack", false);
        //    agent.destination = player.position;
        //}

        //currentCoolDown -= Time.deltaTime;
        //if (currentCoolDown <= 0f)
        //{
        //    CastNinjutsu();

        //}
        agent.destination = player.position;
    }

    //void OnCollisionEnter(Collision collision)
    //{
    //    // If the NPC is hit by the player of the effect of ninjutsu
    //    if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Effect"))
    //    {
    //        // ...
    //    }
    //}

    void CastNinjutsu()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance <= CloseRangeThreshold)
        {
            // Close range ninjutsu
        } else
        {
            // Long range ninjutsu
        }
    }

    void PerformTaijutsu()
    {
        agent.isStopped = true;
        //animator.SetBool("Attack", true);
    }

    //private void OnAnimatorMove()
    //{
    //    if (animator.GetBool("Attack") == false)
    //    {
    //        agent.speed = (animator.deltaPosition / Time.deltaTime).magnitude;
    //    }
    //}
}

public enum NinjutsuName
{
    Fireball,
    Raikiri,
    GalePalm,
    RockShelterCollapse,
    Waterfall,
    PhoenixFire,
    Rasengan
}

public enum ElementType
{
    Fire,
    Water,
    Wind,
    Earth,
    Lightning,
    None
}

public struct Ninjutsu
{
    public NinjutsuName name;
    public int damage;
    public ElementType element;

    public Ninjutsu(NinjutsuName name, int damage, ElementType element)
    {
        this.name = name;
        this.damage = damage;
        this.element = element;
    }
}
