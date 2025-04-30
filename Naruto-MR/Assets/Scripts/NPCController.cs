using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;
using TMPro;
using System.Collections;
using UnityEngine.UIElements;

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

        // NPC will look at the player
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Decrement the timer
        currentCoolDown -= Time.deltaTime;

        float distance = Vector3.Distance(agent.transform.position, player.position);

        // Perform taijutsu if the player is close enough
        if (distance < taijutsuThreshold && currentCoolDown >= 0)
        {
            StartCoroutine(PerformTaijutsu());
            return;
        }


        // When the timer reaches 0, cast ninjutsu
        if (currentCoolDown <= 0f)
        {
            StartCoroutine(CastNinjutsu());
            return;
        }

        // NPC will run toward the player or run away from the player
        Vector3 destination = player.position - (player.position - transform.position).normalized * minDistance;
        destination.y = agent.transform.position.y;
        agent.SetDestination(destination);
    }

    IEnumerator CastNinjutsu()
    {
        isAttacking = true;
        agent.isStopped = true;
        float distance = Vector3.Distance(transform.position, player.transform.position);
        Ninjutsu currentNinjutsu;
        if (distance <= CloseRangeThreshold)
        {
            // Close range ninjutsu
            currentNinjutsu = Rasengan;
            Debug.Log("Rasengan");
        }
        else
        {
            // Long range ninjutsu
            int i = Random.Range(0, 6);
            switch (i)
            {
                case 0:
                    currentNinjutsu = Fireball;
                    Debug.Log("Fireball");
                    break;
                case 1:
                    currentNinjutsu = Raikiri;
                    Debug.Log("Raikiri");
                    break;
                case 2:
                    currentNinjutsu = GalePalm;
                    Debug.Log("GalePalm");
                    break;
                case 3:
                    currentNinjutsu = RockShelterCollapse;
                    Debug.Log("RockShelterCollapse");
                    break;
                case 4:
                    currentNinjutsu = Waterfall;
                    Debug.Log("Waterfall");
                    break;
                case 5:
                    currentNinjutsu = PhoenixFire;
                    Debug.Log("PhoenixFire");
                    break;
            }
        }
        yield return new WaitForSeconds(3);
        isAttacking = false;
        agent.isStopped = false;
        currentCoolDown = coolDown;
    }

    IEnumerator PerformTaijutsu()
    {
        isAttacking = true;
        agent.isStopped = true;
        Debug.Log("Taijutsu");
        yield return new WaitForSeconds(3);
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
