using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [Header("Stats")]
    public float fireRate = 5f;            // shots/sec
    public float projectileSpeed = 40f;
    public int damage = 10;
    public GameObject projectilePrefab;

    [Header("Aiming")]
    public float spawnOffset = 0.5f;       // meters in front of camera

    protected float lastShotTime;
    public System.Action OnFired;

    // Checks if the weapon can fire based on fire rate
    protected bool CanFire()
    {
        return Time.time >= lastShotTime + (1f / Mathf.Max(0.0001f, fireRate));
    }

    // Marks the weapon as having fired, updating last shot time and invoking OnFired event
    protected void MarkFired()
    {
        lastShotTime = Time.time;
        OnFired?.Invoke(); 
    }

    // Spawns projectiles from the camera's forward direction with optional spread and multiple pellets
    protected void SpawnFromCameraForward(int pellets, float spreadDeg)
    {
        var cam = Camera.main;
    if (!cam) { Debug.LogError("[Weapon] No Camera.main"); return; }
    if (!projectilePrefab) { Debug.LogError($"[Weapon] {name} missing projectilePrefab"); return; }


        Vector3 forward = cam.transform.forward;
        Vector3 basePos = cam.transform.position + forward * spawnOffset;

        for (int i = 0; i < pellets; i++)
        {
            Quaternion jitter = (spreadDeg > 0f)
                ? Quaternion.AngleAxis(Random.Range(-spreadDeg, spreadDeg), cam.transform.up)
                  * Quaternion.AngleAxis(Random.Range(-spreadDeg, spreadDeg), cam.transform.right)
                : Quaternion.identity;

            Quaternion rot = Quaternion.LookRotation(jitter * forward);
            var go = Instantiate(projectilePrefab, basePos, rot);

            if (go.TryGetComponent<Rigidbody>(out var rb))
                rb.linearVelocity = rot * Vector3.forward * projectileSpeed;  
                var proj = go.GetComponent<Projectile>();
                if (proj)
    {
        proj.damage = damage;
        proj.owner = transform.root;   // Ignore ALL player colliders
        proj.fromEnemy = false;
    }
        }

        MarkFired();
    }

    // Subclasses implement this to define firing behavior
    public abstract void Use();
}
