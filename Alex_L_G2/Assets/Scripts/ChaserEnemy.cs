using UnityEngine;

public class ChaserEnemy : EnemyBase
{
    public float chaseSpeed = 3.8f;

    protected override void Awake()
    {
        base.Awake();
        if (agent) agent.speed = chaseSpeed;
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
