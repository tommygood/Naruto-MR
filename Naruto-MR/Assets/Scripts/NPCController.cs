using UnityEngine;
using UnityEngine.AI;
using TMPro;
using System.Collections;
using AnimationNamespace;

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

    public bool isAttacking = false;

    public float minDistance = 1.75f;
    public float rotationSpeed = 10f;

    private float taijutsuCooldown;

    AnimationManager animationManager;

    public NarutoAttack narutoAttack;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!player) player = Camera.main.transform;
        currentCoolDown = coolDown;
        taijutsuCooldown = 0f;

        agent = GetComponent<NavMeshAgent>();
        if (!agent) Debug.LogError("NavMeshAgent is missing!");
        animationManager = new AnimationManager();
        // find narutoAttack by tag
        narutoAttack = GameObject.FindGameObjectWithTag("NarutoAttack").GetComponent<NarutoAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        // NPC will look at the player
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        if (isAttacking) {

            // check agent is active Stop" can only be called on an active agent that has been placed on a NavMesh.
            if (!agent.isOnNavMesh)
            {
                Debug.LogWarning("NavMeshAgent is not active or enabled!");
                return;
            }
            agent.isStopped = true;
            return;
        }
        

        // Decrement the timer
        currentCoolDown -= Time.deltaTime;
        taijutsuCooldown -= Time.deltaTime;
        //Debug.Log("Current Cool Down: " + currentCoolDown);

        float distance = Vector3.Distance(agent.transform.position, player.position);
        Debug.Log("Distance to player: " + distance);

        // Perform taijutsu if the player is close enough
        if (distance < taijutsuThreshold && taijutsuCooldown <= 0)
        {
            Debug.Log("xxx Performing Taijutsu!");
            StartCoroutine(PerformTaijutsu());
        }
        // When the timer reaches 0, cast ninjutsu
        else if (currentCoolDown <= 0f)
        {
            Debug.Log("xxx Casting Ninjutsu!");
            StartCoroutine(CastNinjutsu());
        }
        // NPC will run toward the player or run away from the player
        else {
            Vector3 destination = player.position - (player.position - transform.position).normalized * minDistance;
            destination.y = agent.transform.position.y;
            agent.SetDestination(destination);
            Debug.Log("xxx NPC is running toward the player!");
        }
    }

    IEnumerator CastNinjutsu()
    {
        isAttacking = true;
        // check agent is active Stop" can only be called on an active agent that has been placed on a NavMesh.
        if (!agent.isOnNavMesh)
        {
            Debug.LogWarning("NavMeshAgent is not active or enabled!");
            yield break;
        }
        agent.isStopped = true;
        float distance = Vector3.Distance(transform.position, player.transform.position);
        Ninjutsu currentNinjutsu;
        if (distance <= CloseRangeThreshold)
        {
            // Close range ninjutsu
            currentNinjutsu = Rasengan;
            Debug.Log("xxx Rasengan");
            animationManager.SetAnimation("CastingSpell", true);
            yield return new WaitForSeconds(3);
            animationManager.SetAnimation("CastingSpell", false);
        }
        else
        {
            // await this narutoAttack.LongDistanceAttack(narutoAttack.fireEffect, "clapping"); 
            // Long range ninjutsu
            yield return StartCoroutine(narutoAttack.LongDistanceAttack(narutoAttack.fireEffect, "clapping"));
            
        }
        yield return new WaitForSeconds(3);
        isAttacking = false;
        agent.isStopped = false;
        currentCoolDown = coolDown;
    }

    IEnumerator PerformTaijutsu()
    {
        // check agent is active Stop" can only be called on an active agent that has been placed on a NavMesh.
        if (!agent.isOnNavMesh)
        {
            Debug.LogWarning("NavMeshAgent is not active or enabled!");
            yield break;
        }
        isAttacking = true;
        agent.isStopped = true;
        Debug.Log("xxx Taijutsu");
        animationManager.SetAnimation("boxing", true);
        yield return new WaitForSeconds(3);
        animationManager.SetAnimation("boxing", false);
        isAttacking = false;
        agent.isStopped = false;
        taijutsuCooldown = coolDown;
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
