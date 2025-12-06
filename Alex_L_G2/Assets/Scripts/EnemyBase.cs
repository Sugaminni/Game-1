using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class EnemyBase : MonoBehaviour
{
    [Header("Stats")]
    public float maxHealth = 100f;
    public float moveSpeed = 3.5f;

    [Header("Perception / Combat")]
    public float detectRange = 20f;
    public float loseSightRange = 30f;
    public float meleeRange = 1.8f;
    public float touchDamage = 10f;
    public float attackCooldown = 1.0f;

    [Header("FX (optional)")]
    public GameObject deathVfx;

    protected float health;
    protected Transform player;
    protected NavMeshAgent agent;

    private float nextAttackTime;

    // Initialize enemy
    protected virtual void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        agent = GetComponent<NavMeshAgent>();
        if (agent) agent.speed = moveSpeed;
        health = maxHealth;

        WorldRegistry.I?.Register(this);
    }

    protected virtual void OnEnable() { WorldRegistry.I?.Register(this); }
    protected virtual void OnDisable() { WorldRegistry.I?.Unregister(this); }
    protected virtual void OnDestroy() { WorldRegistry.I?.Unregister(this); }

    // Main AI loop
    protected virtual void Update()
    {
        if (!player) return;

        float d = Vector3.Distance(transform.position, player.position);
        if (d <= detectRange) { Move(); Attack(); }
        else if (d >= loseSightRange) { if (agent) agent.ResetPath(); }
        else { Move(); } // soft track
    }

    // Handle taking damage and death
    public virtual void TakeDamage(float dmg)
    {
        if (dmg <= 0f) return;
        health -= dmg;
        if (health <= 0f) OnDeath();
    }

    protected virtual void OnDeath()
    {
        if (deathVfx) Instantiate(deathVfx, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    // Helpers for attack timing and melee damage
    protected bool CanAttackNow() => Time.time >= nextAttackTime;
    protected void MarkAttackedNow() => nextAttackTime = Time.time + attackCooldown;

    protected void DealMeleeIfInRange()
    {
        if (!player) return;
        if (Vector3.Distance(transform.position, player.position) <= meleeRange && CanAttackNow())
        {
            var ph = player.GetComponentInChildren<PlayerHealth>();
            if (ph) ph.TakeDamage(touchDamage);
            MarkAttackedNow();
        }
    }

    // Face the player on the horizontal plane
    protected void FacePlayerFlat()
    {
        if (!player) return;
        Vector3 look = player.position; look.y = transform.position.y;
        transform.rotation = Quaternion.LookRotation((look - transform.position).normalized);
    }

    public abstract void Move();
    public abstract void Attack();

    // Debug gizmos to visualize ranges
#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow; Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.color = new Color(1f, 0.7f, 0.2f); Gizmos.DrawWireSphere(transform.position, loseSightRange);
        Gizmos.color = Color.red; Gizmos.DrawWireSphere(transform.position, meleeRange);
    }
#endif
}
