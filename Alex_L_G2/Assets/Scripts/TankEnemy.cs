using UnityEngine;

public class TankEnemy : EnemyBase
{
    public float crawlSpeed = 2.2f;

    // Initialize tank enemy with modified stats
    protected override void Awake()
    {
        // beefy tank
        maxHealth *= 2.0f;
        touchDamage *= 1.6f;
        meleeRange = Mathf.Max(meleeRange, 2.2f);
        attackCooldown = Mathf.Max(attackCooldown, 1.2f);

        base.Awake();
        if (agent) agent.speed = crawlSpeed;
    }

    // Move towards the player
    public override void Move()
    {
        if (agent && player) agent.SetDestination(player.position);
        FacePlayerFlat();
    }

    // Attack by dealing melee damage if in range
    public override void Attack()
    {
        DealMeleeIfInRange();
    }
}
