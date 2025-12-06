using UnityEngine;

public class RangedEnemy : EnemyBase
{
    [Header("Ranged")]
    public float preferredRange = 10f;
    public GameObject projectilePrefab;
    public float projectileSpeed = 16f;
    public float shotDamage = 7f;
    public float fireCooldown = 1.2f;

    private float nextShot;

    // Move to maintain preferred distance from player
    public override void Move()
    {
        if (!agent || !player) return;

        float d = Vector3.Distance(transform.position, player.position);
        Vector3 away = (transform.position - player.position).normalized;

        if (d < preferredRange * 0.8f)
            agent.SetDestination(transform.position + away * 2f);
        else if (d > preferredRange * 1.2f)
            agent.SetDestination(player.position);
        else
            agent.ResetPath();

        FacePlayerFlat();
    }

    // Attack by shooting projectiles at the player
    public override void Attack()
    {
        if (Time.time < nextShot || !projectilePrefab || !player) return;
        nextShot = Time.time + fireCooldown;

        // spawn slightly forward and up from the enemy
        Vector3 muzzle = transform.position + transform.forward * 1.0f + Vector3.up * 0.9f;
        Vector3 aim = (player.position + Vector3.up * 0.6f - muzzle).normalized;

        var go = Instantiate(projectilePrefab, muzzle, Quaternion.LookRotation(aim));

        if (go.TryGetComponent<Rigidbody>(out var rb))
        rb.linearVelocity = aim * projectileSpeed;

        // mark as enemy + set owner so the projectile ignores its shooter
        if (go.TryGetComponent<Projectile>(out var p))
        {
        p.fromEnemy = true;
        p.damage = shotDamage;
        p.owner = transform;
        // set to EnemyProjectile layer
        int enemyProjLayer = LayerMask.NameToLayer("EnemyProjectile");
        if (enemyProjLayer != -1) go.layer = enemyProjLayer;
        }

        // avoid self-hit
        var myCol = GetComponentInChildren<Collider>();
        var projCol = go.GetComponent<Collider>();
        if (myCol && projCol) Physics.IgnoreCollision(myCol, projCol, true);

        MarkAttackedNow();
    }
}
