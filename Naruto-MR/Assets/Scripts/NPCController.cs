using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;
using TMPro;

public class NPCController : MonoBehaviour
{
    public Transform player;
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

    public float coolDown = 5f;
    private float currentCoolDown;

    public TextMeshProUGUI text;

    private float track_interval = 0.5f;
    private float track_timer = 0f;

    private bool isAttacking = false;

    public float minDistance = 1.75f;
    public float rotationSpeed = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!player) player = Camera.main.transform;

        agent = GetComponent<NavMeshAgent>();
        if (!agent) Debug.LogError("NavMeshAgent is missing!");
    }

    // Update is called once per frame
    void Update()
    {
        // check the timer
        track_timer += Time.deltaTime;
        if (track_timer >= track_interval)
        {
            track_timer = 0f;
        }
        else 
        {
            return;
        }

        if (isAttacking) return;

        Vector3 directionToPlayer = player.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        
        float distance = Vector3.Distance(agent.transform.position, player.position);
        if (distance < taijutsuThreshold) PerformTaijutsu();
        else {
            Vector3 newPos = player.position;
            newPos.y = agent.transform.position.y;
            newPos.x += player.forward.x * minDistance;
            newPos.z += player.forward.z * minDistance;
            agent.SetDestination(newPos);
        }

        currentCoolDown -= Time.deltaTime;
        if (currentCoolDown <= 0f) CastNinjutsu();
    }

    void CastNinjutsu()
    {
        isAttacking = true;
        agent.isStopped = true;
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance <= CloseRangeThreshold)
        {
            // Close range ninjutsu
        }
        else
        {
            // Long range ninjutsu
        }
        isAttacking = false;
        agent.isStopped = false;
        currentCoolDown = coolDown;
    }

    void PerformTaijutsu()
    {
        isAttacking = true;
        agent.isStopped = true;
        isAttacking = false;
        agent.isStopped = false;
        currentCoolDown = coolDown;
    }
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
